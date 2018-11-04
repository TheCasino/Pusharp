using Model = Pusharp.Models.WebSocket.PushReceivedModel;

namespace Pusharp.Entities.WebSocket
{
    public sealed class ReceivedCopy
    {
        private readonly Model _model;

        public ReceivedPushType Type => ReceivedPushType.Clip;

        public string Body => _model.Body;
        public string SourceUserIdentifier => _model.SourceUserIdentifier;
        public string SourceDeviceIdentifier => _model.SourceDeviceIdentifier;

        internal ReceivedCopy(Model model)
        {
            _model = model;
        }
    }
}
