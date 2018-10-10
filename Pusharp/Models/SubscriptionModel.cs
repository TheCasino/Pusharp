using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class SubscriptionModel
    {
        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("active")]
        public bool IsActive { get; set; }

        [ModelProperty("created")]
        public double Created { get; set; }

        [ModelProperty("modified")]
        public double Modified { get; set; }

        [ModelProperty("muted")]
        public bool IsMuted { get; set; }

        [ModelProperty("channel")]
        public ChannelModel Channel { get; set; }
    }

    internal class ChannelModel
    {
        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("tag")]
        public string Tag { get; set; }

        [ModelProperty("name")]
        public string Name { get; set; }

        [ModelProperty("description")]
        public string Description { get; set; }

        [ModelProperty("image_url")]
        public string ImageUrl { get; set; }

        [ModelProperty("website_url")]
        public string WebsiteUrl { get; set; }
    }
}
