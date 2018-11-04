using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    /// <summary>
    ///     The base class for interfacing with the Pushbullet API.
    /// </summary>
    public sealed partial class PushBulletClient
    {
        private readonly PushBulletClientConfig _config;
        private readonly JsonSerializer _serializer;

        internal readonly RequestClient RequestClient;

        /// <summary>
        ///     The current user that is logged into this client.
        /// </summary>
        public CurrentUser CurrentUser { get; private set; }

        private readonly ConcurrentDictionary<string, Device> _devices;
        public IReadOnlyCollection<Device> Devices => _devices.Values.Where(x => x.IsActive).ToImmutableList();

        private readonly ConcurrentDictionary<string, Push> _pushes;
        public IReadOnlyCollection<Push> Pushes => _pushes.Values.ToImmutableList();

        public PushBulletClient(PushBulletClientConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Token))
                throw new NoNullAllowedException("Token can't be null or empty");

            _config = config;
            _serializer = new JsonSerializer();

            _devices = new ConcurrentDictionary<string, Device>();
            _pushes = new ConcurrentDictionary<string, Push>();

            RequestClient = new RequestClient(this, _config, _serializer);
        }

        public async Task ConnectAsync()
        {
            var authentication = await RequestClient.SendAsync<CurrentUserModel>("/v2/users/me", HttpMethod.Get, null).ConfigureAwait(false);
            CurrentUser = new CurrentUser(authentication, RequestClient);

            var socket = new WebSocket(this, _config, _serializer);
            await socket.ConnectAsync();

            if (_config.UseCache)
            {
                await InternalLogAsync(new LogMessage(LogLevel.Verbose, "Building cache"));
                var devices = await GetDevicesAsync();

                foreach (var device in devices)
                    _devices[device.Identifier] = device;

                var pushes = await GetPushesAsync(null);

                foreach (var push in pushes)
                    _pushes[push.Identifier] = push;

                await InternalLogAsync(new LogMessage(LogLevel.Verbose, "Cache built"));
            }

            await InternalReadyAsync();
        }

        internal async Task UpdateDeviceCacheAsync()
        {
            if(!_config.UseCache)
                return;

            var ordered = Devices.OrderBy(x => x.Modified);
            var recent = ordered.LastOrDefault();

            if (recent is null)
                return;

            var deviceModels = await InternalGetDevicesAsync(recent.InternalModified);

            foreach (var model in deviceModels)
            {
                _devices[model.Iden] = new Device(model, this);
            }
        }

        internal async Task UpdatePushCacheAsync()
        {
            if(!_config.UseCache)
                return;

            var ordered = Pushes.OrderBy(x => x.Modified);
            var recent = ordered.LastOrDefault();

            if (recent is null)
                return;

            var pushModels = await InternalGetPushesAsync(null, recent.InternalModified);

            foreach (var model in pushModels)
            {
                _pushes[model.Identifier] = new Push(model, RequestClient);
            }
        }
    }
}