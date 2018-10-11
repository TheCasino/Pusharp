using Pusharp.Utilities;
using System;
using Voltaic.Serialization.Json;

namespace Pusharp.RequestParameters
{
    internal class FileUploadParameters : ParametersBase
    {
        internal override string Encoding => "multipart/form-data";

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
