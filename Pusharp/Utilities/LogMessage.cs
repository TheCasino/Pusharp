namespace Pusharp.Utilities
{
    public class LogMessage
    {
        public string Message { get; }
        public LogLevel Level { get; }

        public override string ToString()
        {
            return $"{Level:G}: {Message}";
        }

        public LogMessage(LogLevel level, string message)
        {
            Message = message;
            Level = level;
        }
    }
}