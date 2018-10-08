using Pusharp.Entities;
using Pusharp.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Utilities;

namespace Pusharp
{
    /// <summary>
    ///     The base class for interfacing with the Pushbullet API.
    /// </summary>
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

        /// <summary>
        ///     Creates a Pushbullet API client from an access token.
        /// </summary>
        /// <param name="accessToken">The Pushbullet access token to authorise with.</param>
        /// <param name="config">The optional client configuration to customise API requests from this client.</param>
        /// <returns>A configured and set-up Pushbullet API client.</returns>
        /// <exception cref="Exception">Thrown if the initial API ping request fails.</exception>
        public static async Task<PushBulletClient> CreateClientAsync(string accessToken, PushBulletClientConfig config = null)
        {
            config = config ?? new PushBulletClientConfig();

            var requests = new Requests(accessToken, config);
            var ping = await requests.SendAsync<PingModel>(string.Empty).ConfigureAwait(false);

            if(!ping.IsHappy)
                throw new Exception($"{ping.Cat} The ping request is not happy!");

            var authentication = await requests.SendAsync<CurrentUserModel>("/users/me", HttpMethod.Get, true, 1, null).ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);

        }
    }
}
