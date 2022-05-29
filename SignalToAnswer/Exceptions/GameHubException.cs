using System;

namespace SignalToAnswer.Exceptions
{
    public class GameHubException : Exception
    {
        public GameHubException(string message) : base(message)
        {
        }
    }
}
