using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Models;
using Pusharp.RequestParameters;
using Push = Pusharp.Entities.Push;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task<IReadOnlyCollection<Push>> GetPushesAsync(PushFilterParameters parameters)
        {
            var pushesModel = await RequestClient.SendAsync<PushesModel>("/v2/pushes", HttpMethod.Get, parameters)
                .ConfigureAwait(false);

            var pushes = pushesModel.Pushes.Select(x => new Push(x, RequestClient));

            return pushes.ToImmutableList();
        }

        public async Task<Push> SendPushAsync(PushParameters parameters)
        {
            var pushModel = await RequestClient.SendAsync<PushModel>("/v2/pushes", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            return new Push(pushModel, RequestClient);
        }
    }
}
