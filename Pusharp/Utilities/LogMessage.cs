using System;

namespace Pusharp.Utilities
{
    public sealed class LogMessage
    {
        //TODO LogSource + LogHandler class -> filter logs by level

        public LogMessage(LogLevel level, string message)
        {
            Message = message;
            Level = level;
        }

        public string Message { get; }
        public LogLevel Level { get; }

        public override string ToString()
        {
            var time = DateTime.UtcNow;
            var niceTime = $"{(time.Hour < 10 ? "0" : "")}{time.Hour}:{(time.Minute < 10 ? "0" : "")}{time.Minute}" +
                           $":{(time.Second < 10 ? "0" : "")}{time.Second}";

            return $"[{niceTime}] [{Level, -8}] {Message}";
        }
    }
}