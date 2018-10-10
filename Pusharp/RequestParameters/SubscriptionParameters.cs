using Pusharp.Utilities;
using System;
using Voltaic.Serialization;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public class SubscriptionParameters : ParametersBase
    {
        [ModelProperty("channel_tag")]
        public string ChannelTag { get; set; }

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
