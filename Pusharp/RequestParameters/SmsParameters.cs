using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class SmsParameters : ParametersBase
    {
        [ModelProperty("type")]
        public string Type { get; set; } = "push";

        [ModelProperty("push")]
        public SmsPush Push { get; set; }

        internal override void VerifyParameters(ParameterBuilder builder)
        {
            builder
                .AddRequirement($"{nameof(Type)} != null", Type != null)
                .AddRequirement($"{nameof(Push)} != null", Push != null);
            // todo: Check push fields/props
        }

        internal override string BuildContent(JsonSerializer serializer)
        {
            return serializer.WriteUtf16String(this);
        }
    }

    public class SmsPush
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