using Pusharp.Utilities;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    internal class EmptyParameters : ParametersBase
    {
        private EmptyParameters()
        {
        }

        internal override void VerifyParameters(ParameterBuilder builder)
        {
        }

        internal override string BuildContent(JsonSerializer _)
        {
            return string.Empty;
        }

        public static EmptyParameters Create()
        {
            return new EmptyParameters();
        }
    }
}