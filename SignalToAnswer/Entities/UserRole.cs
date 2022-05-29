using Microsoft.AspNetCore.Identity;
using System;

namespace SignalToAnswer.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    { 
        public Role Role { get; set; }

        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool? Active { get; set; }
    }
}
