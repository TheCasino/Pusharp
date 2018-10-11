using Model = Pusharp.Models.WebSocket.PushReceivedModel;

namespace Pusharp.Entities.WebSocket
{
    public class DismissedPush
    {
        private readonly Model _model;

        public string NotificationId => _model.NotificationId;
        public string NotificationTag => _model.NotificationTag;
        public string PackageName => _model.PackageName;
        public string SourceUserIden => _model.SourceUserIdentifier;

        public ReceivedPushType Type => ReceivedPushType.Dismissal;

        internal DismissedPush(Model model)
        {
            _model = model;
        }
    }
}
