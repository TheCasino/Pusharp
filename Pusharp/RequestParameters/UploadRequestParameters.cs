using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class UploadRequestParameters : ParametersBase
    {
        [ModelProperty("file_name")]
        public string FileName { get; set; }

        [ModelProperty("file_type")]
        public string FileType { get; set; }

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
