using Model = Pusharp.Models.WebSocket.PushReceivedModel;

namespace Pusharp.Entities.WebSocket
{
    public sealed class ReceivedPush
    {
        private readonly Model _model;

        public ReceivedPushType Type => ReceivedPushType.Mirror;

        //TODO parse icon
        public string Icon => _model.Icon;
        public string Title => _model.Title;
        public string Body => _model.Body;
        public string SourceUserIdentifier => _model.SourceUserIdentifier;
        public string SourceDeviceIdentifier => _model.SourceDeviceIdentifier;
        public string ApplicationName => _model.ApplicationName;
        public string PackageName => _model.PackageName;
        public string NotificationId => _model.NotificationId;
        public string NotificationTag => _model.NotificationTag;

        public bool IsDismissable => _model.IsDismissable;
        public bool RootedDevice => _model.IsRooted;

        public int ClientVersion => _model.ClientVersion;

        internal ReceivedPush(Model model)
        {
            _model = model;
        }
    }
}
