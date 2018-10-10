using Pusharp.Utilities;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    public abstract class ParametersBase
    {
        //maybe make exposed so user can check before sending request
        internal abstract void VerifyParameters(ParameterBuilder builder);

        internal abstract string BuildContent(JsonSerializer serializer);
    }
}