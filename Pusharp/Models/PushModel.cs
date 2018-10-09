using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class PushModel
    {
        [ModelProperty("iden")]
        public string Identifier { get; set; }

        [ModelProperty("active")]
        public bool IsActive { get; set; }
        
        [ModelProperty("created")]
        public double Created { get; set; }

        [ModelProperty("type")]
        public string Type { get; set; }

        [ModelProperty("dismissed")]
        public bool IsDismissed { get; set; }

        [ModelProperty("guid")]
        public string Guid { get; set; }

        [ModelProperty("direction")]
        public string Direction { get; set; }

        [ModelProperty("sender_iden")]
        public string SendIdentity { get; set; }

        [ModelProperty("sender_email")]
        public string SenderEmail { get; set; }

        [ModelProperty("sender_email_normalized")]
        public string SenderEmailNormalized { get; set; }

        [ModelProperty("sender_name")]
        public string SendName { get; set; }

        [ModelProperty("receiver_iden")]
        public string ReceiverIdentifier { get; set; }

        [ModelProperty("receiver_email")]
        public string ReceiverEmail { get; set; }

        [ModelProperty("receiver_email_normalized")]
        public string ReceiverEmailNormalized { get; set; }

        [ModelProperty("target_device_identifier")]
        public string TargetDeviceIdentifier { get; set; }

        [ModelProperty("source_device_identifier")]
        public string SourceDeviceIdentifier { get; set; }

        [ModelProperty("client_identifier")]
        public string ClientIdentifier { get; set; }

        [ModelProperty("awake_app_guids")]
        public string[] AwakeAppGuids { get; set; }

        [ModelProperty("title")]
        public string Title { get; set; }
        
        [ModelProperty("body")]
        public string Body { get; set; }

        [ModelProperty("url")]
        public string Url { get; set; }

        [ModelProperty("file_name")]
        public string FileName { get; set; }

        [ModelProperty("file_type")]
        public string FileType { get; set; }

        [ModelProperty("file_url")]
        public string FileUrl { get; set; }

        [ModelProperty("image_url")]
        public string ImageUrl { get; set; }

        [ModelProperty("image_width")]
        public int ImageWidth { get; set; }

        [ModelProperty("image_height")]
        public int ImageHeight { get; set; }
    }
}
