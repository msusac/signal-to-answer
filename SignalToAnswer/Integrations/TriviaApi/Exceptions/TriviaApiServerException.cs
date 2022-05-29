using System;

namespace SignalToAnswer.Integrations.TriviaApi.Exceptions
{
    public class TriviaApiServerException : Exception
    {
        public TriviaApiServerException(string message) : base(message)
        {
        }
    }
}
