using System;

namespace SignalToAnswer.Options
{
    public class UserOption
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public UserOption(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
