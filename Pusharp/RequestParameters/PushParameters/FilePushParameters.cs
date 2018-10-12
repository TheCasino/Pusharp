using Pusharp.Utilities;
using System;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class FilePushParameters : BasePush
    {
        [ModelProperty("type")]
        internal override string Type { get; set; }

        [ModelProperty("body")]
        public override string Body { get; set; }

        [ModelProperty("file_name")]
        public string FileName { get; set; }

        [ModelProperty("file_type")]
        public string FileType { get; set; }

        [ModelProperty("file_url")]
        public string FileUrl { get; set; }

        internal override void VerifyParameters(ParameterBuilder builder)
        {
            throw new NotImplementedException();
        }

        internal override string BuildContent(JsonSerializer serializer)
        {
            return serializer.WriteUtf16String(this);
        }
    }
}
