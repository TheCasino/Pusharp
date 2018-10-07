using System;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pusharp
{
    public class PushBulletClient
    {
        //TODO log event

        private readonly Requests _requests;

        public CurrentUser CurrentUser { get; }

        private PushBulletClient(Requests requests, CurrentUserModel model)
        {
            _requests = requests;
            CurrentUser = new CurrentUser(model);
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

        public async Task<IReadOnlyCollection<Device>> GetDevicesAsync()
        {
            var devicesModel = await _requests.SendAsync<DevicesModel>("/v2/devices", HttpMethod.Get, true, 1, null).ConfigureAwait(false);
            return devicesModel.Models.ToDevices().ToList();
        }

        public async Task<Device> CreateDeviceAsync(DeviceCreationParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>("/v2/devices", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }
    }
}
