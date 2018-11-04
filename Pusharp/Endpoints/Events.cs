using Pusharp.Entities;
using Pusharp.Entities.WebSocket;
using System;
using System.Threading.Tasks;

namespace Pusharp
{
    public sealed partial class PushBulletClient
    {
        public event Func<LogMessage, Task> Log;

        internal Task InternalLogAsync(LogMessage message)
        {
            if (message.Level >= _config.LogLevel)
                return Log is null ? Task.CompletedTask : Log.Invoke(message);

            return Task.CompletedTask;
        }

        public event Func<ReceivedPush, Task> PushReceived;

        internal Task InternalPushReceivedAsync(ReceivedPush push)
        {
            return PushReceived is null ? Task.CompletedTask : PushReceived.Invoke(push);
        }

        public event Func<DismissedPush, Task> PushDismissed;

        internal Task InternalPushDismissedAsync(DismissedPush push)
        {
            return PushDismissed is null ? Task.CompletedTask : PushDismissed.Invoke(push);
        }

        public event Func<ReceivedCopy, Task> CopyReceived;

        internal Task InternalCopyReceivedAsync(ReceivedCopy copy)
        {
            return CopyReceived is null ? Task.CompletedTask : CopyReceived.Invoke(copy);
        }

        public event Func<Task> Ready;

        private Task InternalReadyAsync()
        {
            return Ready is null ? Task.CompletedTask : Ready.Invoke();
        }

        public event Func<Device, Device, Task> DeviceUpdated;

        internal Task InternalDeviceUpdatedAsync(Device before, Device after)
        {
            return DeviceUpdated is null ? Task.CompletedTask : DeviceUpdated.Invoke(before, after);
        }

        public event Func<Push, Push, Task> PushUpdated;

        internal Task InternalPushUpdatedAsync(Push before, Push after)
        {
            return PushUpdated is null ? Task.CompletedTask : PushUpdated(before, after);
        }
    }
}
