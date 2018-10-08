using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.Utilities;

namespace Pusharp
{
    /// <summary>
    ///     The base class for interfacing with the Pushbullet API.
    /// </summary>
    public partial class PushBulletClient
    {
        internal readonly RequestClient RequestClient;

        private PushBulletClient(RequestClient requestClient, CurrentUserModel model)
        {
            RequestClient = requestClient;
            CurrentUser = new CurrentUser(model);
            RequestClient.Client = this;
        }

        /// <summary>
        ///     The current user that is logged into this client.
        /// </summary>
        public CurrentUser CurrentUser { get; }

        /*
        internal List<Device> Devices { get; } = new List<Device>();

        /// <summary>
        ///     Attempts to retrieve a <see cref="Device"/> from this client's cache. This method will NOT download a device if it is not stored in the cache.
        /// </summary>
        /// <param name="identifier">The identifier of the device to search for.</param>
        /// <returns>The found device. <c>null</c> if no device with the supplied identifier exists in the cache.</returns>
        public Device GetDevice(string identifier)
        {
            return Devices.FirstOrDefault(a => a.Identifier == identifier);
        }
        */

        /// <summary>
        ///     This event is invoked when the client wishes to log a message. (i.e. an endpoint is invoked)
        /// </summary>
        public event Func<LogMessage, Task> Log;

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

            var requests = new RequestClient(accessToken, config);
            var ping = await requests.SendAsync<PingModel>(string.Empty).ConfigureAwait(false);

            if (!ping.IsHappy)
                throw new Exception($"{ping.Cat} The ping request is not happy!");

            var authentication = await requests.SendAsync<CurrentUserModel>("/users/me", HttpMethod.Get, true, 1, null).ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);
        }
    }
}