using Voltaic.Serialization;

namespace Pusharp.Models.WebSocket
{
    internal class MessageReceivedModel
    {
        [ModelProperty("type")]
        public string Type { get; set; }

        [ModelProperty("subtype")]
        public string SubType { get; set; }

        [ModelProperty("push")]
        public string Push { get; set; }
    }
}
