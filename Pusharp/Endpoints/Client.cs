using Pusharp.Entities;
using Pusharp.Models;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    /// <summary>
    ///     The base class for interfacing with the Pushbullet API.
    /// </summary>
    public partial class PushBulletClient
    {
        private readonly PushBulletClientConfig _config;
        private readonly JsonSerializer _serializer;

        internal readonly RequestClient RequestClient;
        /// <summary>
        ///     The current user that is logged into this client.
        /// </summary>
        public CurrentUser CurrentUser { get; private set; }

        public PushBulletClient(PushBulletClientConfig config)
        {
            if(string.IsNullOrWhiteSpace(config.Token))
                throw new NoNullAllowedException("Token can't be null or empty");

            _config = config;
            _serializer = new JsonSerializer();

            RequestClient = new RequestClient(this, _config, _serializer);
        }

        public async Task ConnectAsync()
        {
            var authentication = await RequestClient.SendAsync<CurrentUserModel>("/v2/users/me", HttpMethod.Get, null).ConfigureAwait(false);
            CurrentUser = new CurrentUser(authentication, RequestClient);

            var socket = new WebSocket(this, _config, _serializer);
            socket.Connect();
        }
    }
}