using System.Threading.Tasks;
using PBSharp.Models;
using PBSharp.Results;

namespace PBSharp
{
    public class PushBulletClient
    {
        private readonly Requests _requests;

        public AuthenticationResult Authentication { get; }

        private PushBulletClient(Requests requests, AuthenticationModel model)
        {
            _requests = requests;
            Authentication = new AuthenticationResult(model);
        }

        public static async Task<PushBulletClient> CreateClientAsync(string accessToken)
        {
            var requests = new Requests(accessToken);
            var authentication = await requests.SendRequestAsync<AuthenticationModel>("/v2/users/me").ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);
        }
    }
}
