using System;

namespace SignalToAnswer.Entities
{
    public class Connection
    {
        public int? Id { get; set; }

        public int GroupId { get; set; }

        public Guid UserId { get; set; }

        public string UserIdentifier { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Group Group { get; set; }

        public User User { get; set; }
    }
}
