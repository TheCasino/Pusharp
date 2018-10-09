using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class ChatsListModel
    {
        [ModelProperty("chats")]
        public ChatModel[] Chats { get; set; }
    }
}
