using Model = Pusharp.Models.PushModel;

namespace Pusharp.Entities
{
    public class Sender
    {
        private readonly Model _model;

        public string Name => _model.SendName;
        public string Identity => _model.SendIdentity;
        public string Email => _model.SenderEmail;
        public string EmailNormalized => _model.SenderEmailNormalized;

        internal Sender(Model model)
        {
            _model = model;
        }
    }
}
