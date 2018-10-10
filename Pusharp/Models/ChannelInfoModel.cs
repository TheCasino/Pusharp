using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class ChannelInfoModel
    {
        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("name")]
        public string Name { get; set; }

        [ModelProperty("description")]
        public string Description { get; set; }

        [ModelProperty("image_url")]
        public string ImageUrl { get; set; }

        [ModelProperty("subscriber_count")]
        public int Subscribers { get; set; }

        [ModelProperty("tag")]
        public string Tag { get; set; }

        [ModelProperty("recent_pushes")]
        public PushModel[] Pushes { get; set; }
    }
}
