using System;

namespace Pusharp
{
    public sealed class RatelimitMessage : LogMessage
    {
        public int RateLimit { get; }
        public int Remaining { get; }
        public DateTimeOffset Reset { get; }

        public RatelimitMessage(string message, LogLevel level, int rateLimit, int remaining, DateTimeOffset reset) : base(level, message)
        {
            RateLimit = rateLimit;
            Remaining = remaining;
            Reset = reset;
        }
    }
}
