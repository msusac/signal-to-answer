using System.Collections.Generic;

namespace SignalToAnswer.Integrations.TriviaApi.Models
{
    public class TAQuestion
    {
        public string Category { get; set; }

        public string Difficulty { get; set; }

        public string Question { get; set; }

        public string CorrectAnswer { get; set; }

        public List<string> IncorrectAnswers { get; set; }
    }
}
