using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Match
    {
        public int? Id { get; set; }

        public int GameId { get; set; }

        public bool? IsOngoing { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Game Game { get; set; }

        public List<Answer> Answers { get; set; }

        public List<Result> Results { get; set; }

        public List<Question> Questions { get; set; }
    }
}
