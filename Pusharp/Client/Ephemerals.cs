using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task SendSmsAsync(SmsParameters parameters)
            => await _requests.SendAsync<EmptyModel>("/v2/ephemerals", HttpMethod.Post, true, 1, parameters)
                .ConfigureAwait(false);
    }
}
