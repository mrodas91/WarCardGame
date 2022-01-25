using System.Collections.Generic;
using System.Threading.Tasks;
using WarCardGame.DTO;
using WarCardGame.Entities;

namespace WarCardGame.Services
{
    public interface IHttpClientServiceImplementation
    {
        //Player
        Task<List<Player>> GetAllPlayers();
        Task<Player> GetPlayerById(int id);
        Task UpdatePlayerResults(PlayersResultsDTO player);

        //Gets a new Shuffled Deck
        Task<List<string>> StartNewGame();

        //Match
        Task InsertGameResults(Match match);
    }
}
