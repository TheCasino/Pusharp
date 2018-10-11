using Pusharp.Utilities;
using System;
using System.Threading.Tasks;
using Pusharp.Entities.WebSocket;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public event Func<LogMessage, Task> Log;

        internal Task InternalLogAsync(LogMessage message)
        {
            return Log is null ? Task.CompletedTask : Log.Invoke(message);
        }

        public event Func<ReceivedPush, Task> PushReceived;

        internal void InternalPushReceivedAsync(ReceivedPush push)
        {
            PushReceived?.Invoke(push);
        }

        public event Func<DismissedPush, Task> PushDismissed;

        internal void InternalPushDismissedAsync(DismissedPush push)
        {
            PushDismissed?.Invoke(push);
        }
    }
}
