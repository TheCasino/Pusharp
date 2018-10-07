using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public abstract class BaseRequest
    {
        public abstract bool VerifyParameters();

        public abstract string BuildContent(JsonSerializer serializer);
    }
}
