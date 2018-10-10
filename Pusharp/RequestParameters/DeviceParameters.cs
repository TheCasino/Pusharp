using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class DeviceParameters : ParametersBase
    {
        [ModelProperty("nickname")]
        public string Nickname { get; set; }

        [ModelProperty("model")]
        public string Model { get; set; }

        [ModelProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [ModelProperty("push_token")]
        public string PushToken { get; set; }

        [ModelProperty("app_version")]
        public int AppVersion { get; set; }

        [ModelProperty("icon")]
        public string Icon { get; set; }

        [ModelProperty("has_sms")]
        public bool HasSms { get; set; }

        internal override void VerifyParameters(ParameterBuilder builder)
        {
            builder
                .AddRequirement($"{nameof(Nickname)} != null", Nickname != null)
                .AddRequirement($"{nameof(Model)} != null", Model != null)
                .AddRequirement($"{nameof(Manufacturer)} != null", Manufacturer != null)
                .AddRequirement($"{nameof(PushToken)} != null", PushToken != null)
                //.AddRequirement($"{nameof(AppVersion)} != something")
                .AddRequirement($"{nameof(Icon)} != null", Icon != null);
            //.AddRequirement($"{nameof(HasSms)} != null", HasSms != null);
        }

        internal override string BuildContent(JsonSerializer serializer)
        {
            return serializer.WriteUtf16String(this);
        }
    }
}