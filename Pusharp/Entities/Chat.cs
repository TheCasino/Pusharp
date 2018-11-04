using Pusharp.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Model = Pusharp.Models.ChatModel;

namespace Pusharp.Entities
{
    public sealed class Chat
    {
        private Model _model;
        private readonly PushBulletClient _client;

        public string Identifier => _model.Identifier;

        public bool IsActive => _model.IsActive;
        public bool IsMuted => _model.Muted;

        public DateTimeOffset Created => DateTimeHelpers.ToDateTime(_model.CreatedAt);
        public DateTimeOffset Modified => DateTimeHelpers.ToDateTime(_model.ModifiedAt);

        private Recipient _recipient;
        //stops a new recipient being made each call
        public Recipient Recipient => _recipient ?? (_recipient = new Recipient(_model.With));

        internal Chat(Model model, PushBulletClient client)
        {
            _model = model;
            _client = client;
        }

        public Task MuteAsync()
        {
            return ModifyAsync(true);
        }

        public Task UnMuteAsync()
        {
            return ModifyAsync(false);
        }

        private async Task ModifyAsync(bool isMuted)
        {
            var model = await _client.RequestClient.SendAsync<Model>("/v2/chats", HttpMethod.Post, new MutedParameter
            {
                IsMuted = isMuted
            }).ConfigureAwait(false);

            _model = model;
        }

        public async Task DeleteAsync()
        {
            if (!IsActive)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Error, "Only active chats can be deleted"))
                    .ConfigureAwait(false);

                return;
            }

            await _client.RequestClient.SendAsync($"/v2/chats/{Identifier}", HttpMethod.Delete, null)
                .ConfigureAwait(false);
        }
    }
}
