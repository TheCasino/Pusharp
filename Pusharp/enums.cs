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

    public enum PushDirection
    {
        Self,
        Outgoing,
        Incoming
    }

    public enum PushType
    {
        Note,
        Link,
        File
    }
}
