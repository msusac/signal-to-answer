using System;

namespace SignalToAnswer.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
