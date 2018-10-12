using Voltaic.Serialization;

namespace Pusharp.RequestParameters
{
    public abstract class BasePush : ParametersBase
    { 
        [ModelProperty("device_iden")]
        internal string DeviceIndentifier { get; set; }

        [ModelProperty("email")]
        internal string Email { get; set; }

        [ModelProperty("channel_tag")]
        internal string ChannelTag { get; set; }

        [ModelProperty("client_iden")]
        internal string ClientIdentifier { get; set; }

        internal abstract string Type { get; set; }
        public abstract string Body { get; set; }
    }
}
