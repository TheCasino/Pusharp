namespace Pusharp.Utilities
{
    internal class Logger
    {
        private readonly PushBulletClient _client;
        private readonly LogLevel _level;

        public Logger(PushBulletClient client, LogLevel level)
        {
            _client = client;
            _level = level;
        }

        public void InvokeLog(LogMessage message)
        {
            if (message.Level >= _level)
                _client.InternalLogAsync(message);
        }
    }
}
