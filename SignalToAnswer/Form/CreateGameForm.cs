using System.Collections.Generic;

namespace SignalToAnswer.Form
{
    public class CreateGameForm
    {
        public int? Limit { get; set; }

        public int? Difficulty { get; set; }

        public List<int> Categories { get; set; } = new List<int>();

        public List<string> InviteUsers { get; set; }
    }
}
