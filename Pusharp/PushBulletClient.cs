using Pusharp.Entities;
using Pusharp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public class PushBulletClient
    {
        private readonly Requests _requests;

        public CurrentUser CurrentUser { get; }

        private PushBulletClient(Requests requests, CurrentUserModel model)
        {
            _requests = requests;
            CurrentUser = new CurrentUser(model);
        }

        public static async Task<PushBulletClient> CreateClientAsync(string accessToken, PushBulletClientConfig config = null)
        {
            var requests = new Requests(accessToken, config ?? new PushBulletClientConfig());

            var authentication = await requests.SendAsync<CurrentUserModel>("/v2/users/me", HttpMethod.Get).ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            var devicesModel = await _requests.SendAsync<DevicesModel>("/v2/devices", HttpMethod.Get).ConfigureAwait(false);
            return devicesModel.Models.ToDevices();
        }

        public async Task<Device> CreateDeviceAsync(DeviceCreationParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>("/v2/devices", HttpMethod.Post, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }
    }
}
