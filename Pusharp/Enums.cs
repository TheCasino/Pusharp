namespace Pusharp
{
    public enum LogLevel
    {
        Critical = 5,
        Error = 4,
        Warning = 3,
        Info = 2,
        Verbose = 1,
        Debug = 0
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

    public enum ReceivedPushType
    {
        Mirror,
        Dismissal,
        Clip
    }

    public enum PushTarget
    {
        Device,
        Email,
        Channel,
        Client
    }
}
