using System;
namespace WarCardGame.Entities
{
    public class Match
    {
        public int Id { get; set; }
        
        public int PlayerId1 { get; set; }
        
        public int PlayerId2 { get; set; }
        
        public int PlayedCards { get; set; }
        
        public int Winner { get; set; }
    }
}
