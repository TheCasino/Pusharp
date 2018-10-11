using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task<UploadRequest> CreateUploadRequestAsync(UploadRequestParameters parameters)
        {
            var uploadModel = await RequestClient
                .SendAsync<UploadRequestModel>("/v2/upload-request", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            return new UploadRequest(uploadModel, RequestClient);
        }
    }
}
