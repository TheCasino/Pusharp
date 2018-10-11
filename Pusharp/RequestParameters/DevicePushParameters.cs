using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class DevicePushParameters : ParametersBase
    {
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
