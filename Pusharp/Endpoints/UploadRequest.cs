using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public sealed partial class PushBulletClient
    {
        public async Task<UploadRequest> CreateUploadRequestAsync(Action<UploadRequestParameters> uploadRequestParameters)
        {
            var parameters = new UploadRequestParameters();
            uploadRequestParameters(parameters);

            var uploadModel = await RequestClient
                .SendAsync<UploadRequestModel>("/v2/upload-request", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            return new UploadRequest(uploadModel, RequestClient);
        }
    }
}
