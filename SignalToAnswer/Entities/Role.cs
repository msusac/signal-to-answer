using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SignalToAnswer.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
