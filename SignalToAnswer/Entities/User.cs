using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public string Description { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }

        public Connection Connection { get; set; }

        public UserRole UserRole { get; set; }

        public List<Player> Players { get; set; }
    }
}
