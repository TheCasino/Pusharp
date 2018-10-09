using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task<IReadOnlyCollection<Chat>> GetChatsAsync()
        {
            var chatsListModel = await RequestClient.SendAsync<ChatsListModel>("/v2/chats", HttpMethod.Get, null)
                .ConfigureAwait(false);
            var chatsList = chatsListModel.Chats.Select(x => new Chat(x, this));

            return chatsList.ToImmutableList();
        }

        public Task<Chat> CreateChatAsync(string email)
        {
            return CreateChatAsync(new ChatParameters
            {
                Email = email
            });
        }

        public async Task<Chat> CreateChatAsync(ChatParameters parameters)
        {
            var chatModel = await RequestClient.SendAsync<ChatModel>("/v2/chats", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            return new Chat(chatModel, this);
        }
    }
}
