using System;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class DeviceParameters : BaseRequest
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

        public override bool VerifyParameters()
        {
            throw new NotImplementedException();
        }

        public override string BuildContent(JsonSerializer serializer)
            => serializer.WriteUtf16String(this);
    }
}
