namespace Pusharp
{
    public enum LogLevel
    {
        Critical,
        Error,
        Warning,
        Info,
        Verbose,
        Debug
    }

    internal enum MessageType
    {
        HEARTBEAT,
        TICKLE,
        PUSH
    }

    internal enum SubType
    {
        NONE,
        PUSH,
        DEVICE
    }
}
