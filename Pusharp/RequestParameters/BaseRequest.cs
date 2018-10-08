using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public abstract class BaseRequest
    {
        internal abstract bool VerifyParameters();

        internal abstract string BuildContent(JsonSerializer serializer);
    }
}
