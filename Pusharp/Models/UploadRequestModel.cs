using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class UploadRequestModel
    {
        [ModelProperty("file_name")]
        public string FileName { get; set; }

        [ModelProperty("file_type")]
        public string FileType { get; set; }

        [ModelProperty("file_url")]
        public string FileUrl { get; set; }

        [ModelProperty("upload_url")]
        public string UploadUrl { get; set; }
    }
}
