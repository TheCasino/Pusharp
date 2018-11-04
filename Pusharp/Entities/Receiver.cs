using Model = Pusharp.Models.PushModel;

namespace Pusharp.Entities
{
    public sealed class Receiver
    {
        private readonly Model _model;

        public string Identifier => _model.ReceiverIdentifier;
        public string Email => _model.ReceiverEmail;
        public string EmailNormalized => _model.ReceiverEmailNormalized;

        internal Receiver(Model model)
        {
            _model = model;
        }
    }
}
