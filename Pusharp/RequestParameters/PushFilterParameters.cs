using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class PushFilterParameters : BaseRequest
    {
        [ModelProperty("modified_after")]
        public string ModifiedAfter { get; set; }

        [ModelProperty("active")]
        public bool IsActive { get; set; }

        [ModelProperty("cursor")]
        public string Cursor { get; set; }

        [ModelProperty("limit")]
        public int Limit { get; set; }

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
