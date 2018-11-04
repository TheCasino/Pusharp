using System;

namespace Pusharp
{
    public sealed class ExceptionMessage : LogMessage
    {
        public Exception Exception { get; }

        public ExceptionMessage(LogLevel level,
            Exception exception) : base(level, exception.ToString())
        {
            Exception = exception;
        }
    }
}
