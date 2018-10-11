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
        public PushReceivedModel Push { get; set; }
    }

    internal class PushReceivedModel
    {
        [ModelProperty("type")]
        public string Type { get; set; }

        [ModelProperty("icon")]
        public string Icon { get; set; }

        [ModelProperty("title")]
        public string Title { get; set; }

        [ModelProperty("body")]
        public string Body { get; set; }

        [ModelProperty("source_user_iden")]
        public string SourceUserIdentifier { get; set; }

        [ModelProperty("source_device_iden")]
        public string SourceDeviceIdentifier { get; set; }

        [ModelProperty("application_name")]
        public string ApplicationName { get; set; }

        [ModelProperty("dismissable")]
        public bool IsDismissable { get; set; }

        [ModelProperty("package_name")]
        public string PackageName { get; set; }

        [ModelProperty("notification_id")]
        public string NotificationId { get; set; }

        [ModelProperty("notification_tag")]
        public string NotificationTag { get; set; }

        [ModelProperty("has_root")]
        public bool IsRooted { get; set; }

        [ModelProperty("client_version")]
        public int ClientVersion { get; set; }
    }
}
