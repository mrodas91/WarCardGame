using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WarCardGame.DTO;
using WarCardGame.Entities;
using WarCardGame.Services;

namespace WarCardGame
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            bool argsPassed = args.Length != 0 ? true : false;
            bool verboseArg = false;

            if (argsPassed && (args[0] == "V" || args[0] == "v"))
                verboseArg = true;

            try
            {
                Console.WriteLine("Starting New Game...");
                Console.WriteLine("Shuffling Deck...");
                List<string> shuffledDeck = await provider.GetRequiredService<IHttpClientServiceImplementation>().StartNewGame();

                Console.WriteLine("Selecting Players...");
                List<Player> players = await provider.GetRequiredService<IHttpClientServiceImplementation>().GetAllPlayers();
                Random rnd = new Random();
                List<Player> ramdomplayer = players.OrderBy(item => rnd.Next()).ToList();
                int player1Id = ramdomplayer[0].Id;
                int player2Id = ramdomplayer[1].Id;
                Console.WriteLine("Player 1: " + ramdomplayer[0].Nombre + "  VS  " + "Player 2: " + ramdomplayer[1].Nombre);

                List<string> deckPlayer1 = new List<string>();
                List<string> deckPlayer2 = new List<string>();
                Console.WriteLine("Dealing cards...");
                provider.GetRequiredService<Game>().DealDecks(ref shuffledDeck, ref deckPlayer1, ref deckPlayer2, verboseArg);

                Console.WriteLine("GAME TIME!");
                int playedCards = provider.GetRequiredService<Game>().Play(ref deckPlayer1, ref deckPlayer2, verboseArg);

                int winnerId; 
                int loserId;

                if (deckPlayer1.Count > deckPlayer2.Count)
                {
                    Console.WriteLine("*** PLAYER 1 WINS! ***");
                    winnerId = player1Id;
                    loserId = player2Id;
                }
                else
                {
                    Console.WriteLine("*** PLAYER 2 WINS! ***");
                    winnerId = player2Id;
                    loserId = player1Id;
                }

                Player winnerPlayer = await provider.GetRequiredService<IHttpClientServiceImplementation>().GetPlayerById(winnerId);
                Console.WriteLine($"WINER: {winnerPlayer.Nombre}  TOTAL WON GAMES: {winnerPlayer.WonGames + 1}  TOTAL LOST GAMES: {winnerPlayer.LostGames}");
                Player loserPlayer = await provider.GetRequiredService<IHttpClientServiceImplementation>().GetPlayerById(loserId);
                Console.WriteLine($"LOSER: {loserPlayer.Nombre}   TOTAL WON GAMES: {loserPlayer.WonGames}  TOTAL LOST GAMES: {loserPlayer.LostGames + 1}");

                //Insert Results
                Entities.Match matchResults = new Entities.Match
                {
                    PlayerId1 = player1Id,
                    PlayerId2 = player2Id,
                    PlayedCards = playedCards,
                    Winner = winnerId
                };

                await provider.GetRequiredService<IHttpClientServiceImplementation>().InsertGameResults(matchResults);

                //Update Players Results
                List<PlayersResultsDTO> playersResults = new List<PlayersResultsDTO>();
                playersResults.Add(new PlayersResultsDTO { Id = winnerId, WonGames = winnerPlayer.WonGames + 1});
                playersResults.Add(new PlayersResultsDTO { Id = loserId, LostGames = loserPlayer.LostGames + 1});

                await provider.GetRequiredService<IHttpClientServiceImplementation>().UpdatePlayerResults(playersResults[0]);
                await provider.GetRequiredService<IHttpClientServiceImplementation>().UpdatePlayerResults(playersResults[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }

            Console.Write("Press Enter to exit...");
            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            //services.AddHttpClient("WarGameApiClient", config =>
            //{
            //    config.BaseAddress = new Uri("https://localhost:5001/api/");
            //    config.Timeout = new TimeSpan(0, 0, 30);
            //    config.DefaultRequestHeaders.Clear();
            //});
            services.AddScoped<IHttpClientServiceImplementation, HttpClientFactoryService>();

            services.AddHttpClient<WarGameApiClient>();

            services.AddScoped<Game>();
        }
    }    

    /*
    public class GameOld
    {
        public void Start()
        {
            List<string> deck = new List<string>() { "A♣", "K♣", "Q♣", "J♣", "10♣", "9♣", "8♣", "7♣", "6♣", "5♣", "4♣", "3♣", "2♣",
                                                     "A♦", "K♦", "Q♦", "J♦", "10♦", "9♦", "8♦", "7♦", "6♦", "5♦", "4♦", "3♦", "2♦",
                                                     "A♥", "K♥", "Q♥", "J♥", "10♥", "9♥", "8♥", "7♥", "6♥", "5♥", "4♥", "3♥", "2♥",
                                                     "A♠", "K♠", "Q♠", "J♠", "10♠", "9♠", "8♠", "7♠", "6♠", "5♠", "4♠", "3♠", "2♠"};
                
            Random rnd = new Random();
            var shuffledDeck = deck.OrderBy(item => rnd.Next()).ToList();
            shuffledDeck = shuffledDeck.OrderBy(item => rnd.Next()).ToList();
            shuffledDeck = shuffledDeck.OrderBy(item => rnd.Next()).ToList();
            

            Console.WriteLine("ShuffledDeck");
            shuffledDeck.ForEach(x => Console.Write(x + ", "));
            Console.WriteLine();

            //WarGameAPI api = new WarGameAPI();
            //var p1 =  api.GetPlayerInfo(1);

            //Dealing deck
            List<string> deckPlayer1 = new List<string>();
            List<string> deckPlayer2 = new List<string>();

            int playerTurn = rnd.Next(1, 3);
            Console.WriteLine("Start Player " + playerTurn);
            Console.WriteLine("Dealing cards...");

            while (shuffledDeck.Count > 0)
            {
                if (playerTurn == 1)
                {
                    deckPlayer1.Add(shuffledDeck[0]);
                    playerTurn = 2;
                }
                else
                {
                    deckPlayer2.Add(shuffledDeck[0]);
                    playerTurn = 1;
                }
                shuffledDeck.RemoveAt(0);
            }

            Console.WriteLine("Player 1 Deck");
            deckPlayer1.Reverse();
            deckPlayer1.ForEach(x => Console.Write(x + ", "));

            Console.WriteLine();
            Console.WriteLine("Player 2 Deck");
            deckPlayer2.Reverse();
            deckPlayer2.ForEach(x => Console.Write(x + ", "));

            Console.WriteLine();
            Console.WriteLine("Game Time!");

            List<string> turnedUpCards = new List<string>();
            int cardP1 = 0;
            int cardP2 = 0;
            int count = 0;
            bool noCardsToPlay = false;
            while (deckPlayer1.Count > 0 && deckPlayer2.Count > 0 && !noCardsToPlay)
            {
                count++;
                DrawNextCards(ref turnedUpCards, ref deckPlayer1, ref deckPlayer2);
                //Validate if a War is in progress
                while(turnedUpCards.Count > 0)
                {
                    cardP1 = GetCardValue(Regex.Match(turnedUpCards[^2], @"[AKQJ0-9]+").Value);
                    cardP2 = GetCardValue(Regex.Match(turnedUpCards.Last(), @"[AKQJ0-9]+").Value);

                    Console.Write(count +".- " +turnedUpCards[^2] + "  VS  " + turnedUpCards.Last());

                    int warResult = GetWarResult(cardP1, cardP2);
                    if (warResult == 1)
                    {
                        Console.WriteLine("     Win: P1");
                        turnedUpCards = turnedUpCards.OrderBy(item => rnd.Next()).ToList();
                        deckPlayer1.AddRange(turnedUpCards);
                        turnedUpCards.Clear();
                        deckPlayer1.ForEach(x => Console.Write(x + ", "));
                        Console.WriteLine();
                        deckPlayer2.ForEach(x => Console.Write(x + ", "));
                        Console.WriteLine();
                    }
                    else if (warResult == 2)
                    {
                        Console.WriteLine("     Win: P2");
                        turnedUpCards = turnedUpCards.OrderBy(item => rnd.Next()).ToList();
                        deckPlayer2.AddRange(turnedUpCards);
                        turnedUpCards.Clear();
                        deckPlayer1.ForEach(x => Console.Write(x + ", "));
                        Console.WriteLine();
                        deckPlayer2.ForEach(x => Console.Write(x + ", "));
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine(" *** W A R ***");
                        if (deckPlayer1.Count >= 2 && deckPlayer2.Count >= 2)
                        {
                            //Draw 1 face down card from both Decks
                            DrawNextCards(ref turnedUpCards, ref deckPlayer1, ref deckPlayer2);
                            //Draw 1 face up card from both Decks
                            DrawNextCards(ref turnedUpCards, ref deckPlayer1, ref deckPlayer2);
                        }
                        else
                        {
                            Console.WriteLine(deckPlayer1.Count < 2 ? "*** NO MORE CARDS TO PLAY FOR PLAYER 1 ***" :
                                " *** NO MORE CARDS TO PLAY FOR PLAYER 2 ***");
                            noCardsToPlay = true;
                            turnedUpCards.Clear();
                        }
                    }
                }
            }
            Console.WriteLine(deckPlayer1.Count > deckPlayer2.Count ?
                                    "*** PLAYER 1 WINS! ***" : "*** PLAYER 2 WINS! ***");
        }
    }*/
}
