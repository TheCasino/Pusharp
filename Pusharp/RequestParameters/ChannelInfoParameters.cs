using Pusharp.Utilities;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class ChannelInfoParameters : ParametersBase
    {
        [ModelProperty("tag")]
        public string Tag { get; set; }

        [ModelProperty("no_recent_pushes")]
        public bool GetRecentPushes { get; set; }

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
