using Pusharp.Utilities;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public abstract class BaseRequest
    {
        internal abstract void VerifyParameters(ParameterBuilder builder);

        internal abstract string BuildContent(JsonSerializer serializer);
    }
}