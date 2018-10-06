using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class AuthenticationModel
    {
        [ModelProperty("active")]
        public bool Active { get; set; }

        [ModelProperty("created")]
        public double Created { get; set; }

        [ModelProperty("email")]
        public string Email { get; set; }

        [ModelProperty("email_normalized")]
        public string EmailNormalized { get; set; }

        [ModelProperty("iden")]
        public string Iden { get; set; }

        [ModelProperty("image_url")]
        public string ImageUrl { get; set; }

        [ModelProperty("max_upload_size")]
        public long MaxUploadSize { get; set; }

        [ModelProperty("modified")]
        public double Modified { get; set; }

        [ModelProperty("name")]
        public string Name { get; set; }
    }
}
