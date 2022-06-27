using System;

namespace SignalToAnswer.Entities
{
    public class Result
    {
        public int? Id { get; set; }

        public int GameId { get; set; }

        public int MatchId { get; set; }

        public int PlayerId { get; set; }

        public int TotalScore { get; set; }

        public int? WinnerStatus { get; set; }

        public string Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }
    
        public Game Game { get; set; }

        public Match Match { get; set; }

        public Player Player { get; set; }
    }
}
