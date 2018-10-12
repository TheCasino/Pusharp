using Pusharp.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Model = Pusharp.Models.SubscriptionModel;

namespace Pusharp.Entities
{
    public class Subscription
    {
        private Model _model;
        private readonly PushBulletClient _client;

        public string Identifier => _model.Identifier;

        public bool IsActive => _model.IsActive;
        public bool IsMuted => _model.IsMuted;

        public DateTimeOffset Created => DateTimeHelpers.ToDateTime(_model.Created);
        public DateTimeOffset Modified => DateTimeHelpers.ToDateTime(_model.Modified);

        private Channel _channel;
        public Channel Channel => _channel ?? (_channel = new Channel(_model.Channel, _client));

        internal Subscription(Model model, PushBulletClient client)
        {
            _client = client;
            _model = model;
        }

        public async Task ModifyAsync(bool isMuted)
        {
            var model = await _client.RequestClient
                .SendAsync<Model>($"/v2/subscriptions/{Identifier}", HttpMethod.Post, new MutedParameter
                {
                    IsMuted = isMuted
                }).ConfigureAwait(false);

            _model = model;
        }

        public async Task DeleteAsync()
        {
            await _client.RequestClient
                .SendAsync($"/v2/subscriptions/{Identifier}", HttpMethod.Delete, null)
                .ConfigureAwait(false);
        }
    }
}
