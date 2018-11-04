using System;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Model = Pusharp.Models.ChannelModel;

namespace Pusharp.Entities
{
    public sealed class Channel
    {
        private readonly Model _model;
        private readonly PushBulletClient _client;

        public string Identifier => _model.Identifier;
        public string Tag => _model.Tag;
        public string Name => _model.Name;
        public string Description => _model.Description;

        private Uri _imageUrl;
        public Uri ImageUrl => _imageUrl ?? (_imageUrl = new Uri(_model.ImageUrl));

        private Uri _websiteUrl;
        public Uri WebsiteUrl => _websiteUrl ?? (_websiteUrl = new Uri(_model.WebsiteUrl));

        internal Channel(Model model, PushBulletClient client)
        {
            _model = model;
            _client = client;
        }

        public Task<Push> SendNoteAsync(Action<NotePushParameters> parameterOperator)
        {
            var parameters = new NotePushParameters();
            parameterOperator(parameters);

            return _client.PushNoteAsync(parameters, PushTarget.Channel, Tag);
        }

        public Task<Push> SendLinkAsync(Action<LinkPushParameters> parameterOperator)
        {
            var parameters = new LinkPushParameters();
            parameterOperator(parameters);

            return _client.PushLinkAsync(parameters, PushTarget.Channel, Tag);
        }

        public Task<Push> SendFileAsync(Action<FilePushParameters> parameterOperator)
        {
            var parameters = new FilePushParameters();
            parameterOperator(parameters);

            return _client.PushFileAsync(parameters, PushTarget.Channel, Tag);
        }
    }
}
