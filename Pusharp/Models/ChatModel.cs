using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class ChatModel
    {
        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("active")]
        public bool IsActive { get; set; }

        [ModelProperty("created")]
        public double CreatedAt { get; set; }
        
        [ModelProperty("modified")]
        public double ModifiedAt { get; set; }

        [ModelProperty("muted")]
        public bool Muted { get; set; }

        [ModelProperty("with")]
        public With With { get; set; }
    }

    internal class With
    {
        [ModelProperty("email")]
        public string Email { get; set; }

        [ModelProperty("email_normalized")]
        public string NormalizedEmail { get; set; }

        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("image_url")]
        public string ImageUrl { get; set; }

        [ModelProperty("type")]
        public string Type { get; set; }

        [ModelProperty("name")]
        public string Name { get; set; }
    }
}
