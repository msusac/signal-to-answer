using SignalToAnswer.Entities;
using SignalToAnswer.Options;
using System.Collections.Generic;

namespace SignalToAnswer.Mappers.Option
{
    public class UserOptionMapper
    {
        public UserOption Map(User user)
        {
            return new UserOption(user.Id, user.UserName);
        }

        public List<UserOption> Map(List<User> users)
        {
            var options = new List<UserOption>();

            users.ForEach(u => options.Add(Map(u)));

            return options;
        }
    }
}
