using System;
using System.Data;
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

        /// <summary>
        ///     The current user that is logged into this client.
        /// </summary>
        public CurrentUser CurrentUser { get; private set; }

        //public delegate void LogEvent(LogMessage message);

        /// <summary>
        ///     This event is invoked when the client wishes to log a message. (i.e. an endpoint is invoked)
        /// </summary>
        public event Func<LogMessage, Task> Log; 

        internal Task InternalLogAsync(LogMessage message)
        {
            return Log is null ? Task.CompletedTask : Log.Invoke(message);
        }
        
        public PushBulletClient(PushBulletClientConfig config)
        {
            if(string.IsNullOrWhiteSpace(config.Token))
                throw new NoNullAllowedException("Token can't be null or empty");

            RequestClient = new RequestClient(config, this);
        }

        public async Task AuthenticateAsync()
        {
            var ping = await RequestClient.SendAsync<PingModel>(string.Empty).ConfigureAwait(false);

            if(!ping.IsHappy)
                throw new Exception($"{ping.Cat} the ping request is not happy");

            var authentication = await RequestClient.SendAsync<CurrentUserModel>("/v2/users/me", HttpMethod.Get, true, 1, null).ConfigureAwait(false);
            CurrentUser = new CurrentUser(authentication);
        }
    }
}