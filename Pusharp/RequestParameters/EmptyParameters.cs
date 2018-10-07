using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    internal class EmptyParameters : BaseRequest
    {
        private EmptyParameters() { }

        public override bool VerifyParameters()
            => true;

        public override string BuildContent(JsonSerializer _)
            => string.Empty;

        public static EmptyParameters Create()
            => new EmptyParameters();
    }
}
