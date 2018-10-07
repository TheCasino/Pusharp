using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public abstract class BaseRequest : IRequestParameters
    {
        public abstract bool VerifyParameters();

        public abstract string BuildContent(JsonSerializer serializer);
    }
}
