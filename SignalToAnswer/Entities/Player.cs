using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Player
    {
        public int? Id { get; set; }

        public int GameId { get; set; }

        public Guid UserId { get; set; }

        public int PlayerStatus { get; set; }

        public int? ReplayStatus { get; set; }

        public int? InviteStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Game Game { get; set; }

        public User User { get; set; }

        public List<Answer> Answers { get; set; }

        public List<Result> Results { get; set; }
    }
}
