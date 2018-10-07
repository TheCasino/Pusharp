using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task<IReadOnlyCollection<Device>> GetDevicesAsync()
        {
            var devicesModel = await _requests.SendAsync<DevicesModel>("/v2/devices", HttpMethod.Get, true, 1, null).ConfigureAwait(false);
            return devicesModel.Models.ToDevices().ToList();
        }

        public async Task<Device> CreateDeviceAsync(DeviceParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>("/v2/devices", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }

        public Task<Device> UpdateDeviceAsync(Device device, DeviceParameters parameters)
            => UpdateDeviceAsync(device.Identifier, parameters);

        public async Task<Device> UpdateDeviceAsync(string identifier, DeviceParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>($"/v2/devices/{identifier}", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }

        public Task DeleteDeviceAsync(Device device)
        {
            if(!device.IsActive)
                throw new Exception("device must be active");

            return DeleteDeviceAsync(device.Identifier);
        }

        public async Task DeleteDeviceAsync(string identifier)
            => await _requests.SendAsync<EmptyModel>($"/v2/devices/{identifier}", HttpMethod.Delete, true, 1, null).ConfigureAwait(false);
    }
}
