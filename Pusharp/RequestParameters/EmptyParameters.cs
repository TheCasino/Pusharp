using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    internal class EmptyParameters : BaseRequest
    {
        private EmptyParameters() { }

        internal override bool VerifyParameters()
            => true;

        internal override string BuildContent(JsonSerializer _)
            => string.Empty;

        public static EmptyParameters Create()
            => new EmptyParameters();
    }
}
