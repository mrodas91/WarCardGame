using System;
namespace WarCardGame.Entities
{
    public class Player
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int WonGames { get; set; }

        public int LostGames { get; set; }
    }
}
