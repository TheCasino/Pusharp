using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class NotePushParameters : BasePush
    {
        [ModelProperty("type")]
        internal override string Type { get; set; }

        [ModelProperty("body")]
        public override string Body { get; set; }

        [ModelProperty("title")]
        public string Title { get; set; }

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
