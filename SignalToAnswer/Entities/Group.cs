using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Group
    {
        public int? Id { get; set; }

        public string GroupName { get; set; }

        public int GroupType { get; set; }

        public bool? IsUnique { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public List<Connection> Connections { get; set; }
    }
}
