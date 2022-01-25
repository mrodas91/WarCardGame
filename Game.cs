using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WarCardGame
{
    public class Game
    {
        public void DealDecks(ref List<string> _shuffledDeck, ref List<string> _deckPlayer1, ref List<string> _deckPlayer2, bool verbose)
        {
            bool player1Turn = true;
            while (_shuffledDeck.Count > 0)
            {
                if (player1Turn)
                {
                    _deckPlayer1.Add(_shuffledDeck[0]);
                    player1Turn = false;
                }
                else
                {
                    _deckPlayer2.Add(_shuffledDeck[0]);
                    player1Turn = true;
                }
                _shuffledDeck.RemoveAt(0);
            }

            ////Print Both Decks before the game starts
            if (verbose)
            {
                Console.WriteLine("Player 1 Deck");
                _deckPlayer1.Reverse();
                _deckPlayer1.ForEach(x => Console.Write(x + ", "));

                Console.WriteLine();
                Console.WriteLine("Player 2 Deck");
                _deckPlayer2.Reverse();
                _deckPlayer2.ForEach(x => Console.Write(x + ", "));
                Console.WriteLine();
            }
        }

        public int Play(ref List<string> _deckPlayer1, ref List<string> _deckPlayer2, bool verbose)
        {
            Random rnd = new Random();
            List<string> turnedUpCards = new List<string>();
            int cardP1 = 0;
            int cardP2 = 0;
            int count = 0;
            bool noCardsToPlay = false;
            while (_deckPlayer1.Count > 0 && _deckPlayer2.Count > 0 && !noCardsToPlay)
            {
                count++;
                DrawNextCards(ref turnedUpCards, ref _deckPlayer1, ref _deckPlayer2);
                //Validate if a War is in progress
                while (turnedUpCards.Count > 0)
                {
                    cardP1 = GetCardValue(Regex.Match(turnedUpCards[^2], @"[AKQJ0-9]+").Value);
                    cardP2 = GetCardValue(Regex.Match(turnedUpCards.Last(), @"[AKQJ0-9]+").Value);

                    Console.Write(count + ".- " + turnedUpCards[^2] + "  VS  " + turnedUpCards.Last());

                    int warResult = GetWarResult(cardP1, cardP2);
                    if (warResult == 1)
                    {
                        Console.WriteLine("     Win: P1");
                        turnedUpCards = turnedUpCards.OrderBy(item => rnd.Next()).ToList();
                        _deckPlayer1.AddRange(turnedUpCards);
                        turnedUpCards.Clear();

                        //Print Decks after someone wins
                        if (verbose)
                        {
                            _deckPlayer1.ForEach(x => Console.Write(x + ", "));
                            Console.WriteLine();
                            _deckPlayer2.ForEach(x => Console.Write(x + ", "));
                            Console.WriteLine();
                        }
                    }
                    else if (warResult == 2)
                    {
                        Console.WriteLine("     Win: P2");
                        turnedUpCards = turnedUpCards.OrderBy(item => rnd.Next()).ToList();
                        _deckPlayer2.AddRange(turnedUpCards);
                        turnedUpCards.Clear();

                        //Print Decks after someone wins
                        if(verbose)
                        {
                            _deckPlayer1.ForEach(x => Console.Write(x + ", "));
                            Console.WriteLine();
                            _deckPlayer2.ForEach(x => Console.Write(x + ", "));
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine(" *** W A R ***");
                        if (_deckPlayer1.Count >= 2 && _deckPlayer2.Count >= 2)
                        {
                            //Draw 1 face down card from both Decks - Jump next card
                            DrawNextCards(ref turnedUpCards, ref _deckPlayer1, ref _deckPlayer2);
                            //Draw 1 face up card from both Decks
                            DrawNextCards(ref turnedUpCards, ref _deckPlayer1, ref _deckPlayer2);
                        }
                        else
                        {
                            Console.WriteLine(_deckPlayer1.Count < 2 ? "*** NO MORE CARDS TO PLAY FOR PLAYER 1 ***" :
                                " *** NO MORE CARDS TO PLAY FOR PLAYER 2 ***");
                            noCardsToPlay = true;
                            turnedUpCards.Clear();
                        }
                    }
                }
            }

            return count;
        }

        public int GetCardValue(string card)
        {
            int value = 0;
            switch (card)
            {
                case "A":
                    value = 14;
                    break;
                case "K":
                    value = 13;
                    break;
                case "Q":
                    value = 12;
                    break;
                case "J":
                    value = 11;
                    break;

                default:
                    value = Convert.ToInt32(card);
                    break;
            }
            return value;
        }

        public int GetWarResult(int cardP1, int cardP2)
        {
            if (cardP1 > cardP2)
                return 1;
            else if (cardP1 < cardP2)
                return 2;
            else
                return 0;
        }

        public void DrawNextCards(ref List<string> _turnedUpCards, ref List<string> _deckPlayer1, ref List<string> _deckPlayer2)
        {
            _turnedUpCards.Add(_deckPlayer1[0]);
            _turnedUpCards.Add(_deckPlayer2[0]);
            RemoveFromDecks(ref _deckPlayer1, ref _deckPlayer2);
        }

        public void RemoveFromDecks(ref List<string> _deckPlayer1, ref List<string> _deckPlayer2)
        {
            _deckPlayer1.RemoveAt(0);
            _deckPlayer2.RemoveAt(0);
        }
    }
}
