using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class PushParameters : ParametersBase
    {
        [ModelProperty("device_iden")]
        public string DeviceIdentifier { get; set; }

        [ModelProperty("source_device_iden")]
        public string SourceDeviceIdentifier { get; set; }

        //[ModelProperty("guid")]
        //internal string Guid { get; set; }

        [ModelProperty("email")]
        public string Email { get; set; }

        [ModelProperty("channel_tag")]
        public string ChannelTag { get; set; }

        [ModelProperty("client_iden")]
        public string ClientIdentifier { get; set; }

        [ModelProperty("type")]
        internal string InternalType => Type.ToString().ToLower();

        public PushType Type { get; set; }

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

        internal override void VerifyParameters(ParameterBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        internal override string BuildContent(JsonSerializer serializer)
        {
            return serializer.WriteUtf16String(this);
        }
    }
}
