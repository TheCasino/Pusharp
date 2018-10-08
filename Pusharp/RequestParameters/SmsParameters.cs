using System;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class SmsParameters : BaseRequest
    {
        [ModelProperty("type")]
        public string Type { get; set; } = "push";

        [ModelProperty("push")]
        public Push Push { get; set; }

        internal override bool VerifyParameters()
        {
            throw new NotImplementedException();
        }

        internal override string BuildContent(JsonSerializer serializer)
            => serializer.WriteUtf16String(this);
    }

    public class Push
    {
        [ModelProperty("conversation_iden")]
        public string ConversationIdentifier { get; set; }

        [ModelProperty("message")]
        public string Message { get; set; }

        [ModelProperty("target_device_iden")]
        public string DeviceIdentifier { get; set; }

        [ModelProperty("type")]
        public string Type { get; set; } = "messaging_extension_reply";

        [ModelProperty("package_name")]
        public string PackageName { get; set; } = "com.pushbullet.android";

        [ModelProperty("source_user_identifier")]
        public string UserIdentifier { get; set; }
    }
}
