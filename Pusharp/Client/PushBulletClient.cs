using Pusharp.Entities;
using Pusharp.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Utilities;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public event Func<LogMessage, Task> Log;

        private readonly Requests _requests;

        public CurrentUser CurrentUser { get; }

        private PushBulletClient(Requests requests, CurrentUserModel model)
        {
            _requests = requests;
            CurrentUser = new CurrentUser(model);
            _requests.PushBulletClient = this;
        }

        internal Task InternalLogAsync(LogMessage message)
        {
            return Log != null ? Log.Invoke(message) : Task.CompletedTask;
        }

        public static async Task<PushBulletClient> CreateClientAsync(string accessToken, PushBulletClientConfig config = null)
        {
            config = config ?? new PushBulletClientConfig();

            var requests = new Requests(accessToken, config);
            var ping = await requests.SendAsync<PingModel>(string.Empty).ConfigureAwait(false);

            if(!ping.IsHappy)
                throw new Exception("something");

            var authentication = await requests.SendAsync<CurrentUserModel>("/v2/users/me", HttpMethod.Get, true, 1, null).ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);

        }
    }
}
