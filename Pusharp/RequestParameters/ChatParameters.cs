using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class ChatParameters : ParametersBase
    {
        [ModelProperty("email")]
        public string Email { get; set; }

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
