using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;
using System.Collections.Immutable;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        /// <summary>
        ///     Retrieves a list of all devices under the client's account.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous get operation. This task will resolve with a list of <see cref="Device"/> entities, representing the devices under the client's account.</returns>
        public async Task<IReadOnlyCollection<Device>> GetDevicesAsync()
        {
            var devicesModel = await _requests.SendAsync<DevicesModel>("/devices", HttpMethod.Get, true, 1, null).ConfigureAwait(false);
            return devicesModel.Models.Select(a => new Device(a)).ToImmutableList();
        }

        /// <summary>
        ///     Creates a device under the client's account.
        /// </summary>
        /// <param name="parameters">The <see cref="DeviceParameters"/> to set up the new device with.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post operation. This task will resolve with a <see cref="Device"/> entity representing the created device.</returns>
        public async Task<Device> CreateDeviceAsync(DeviceParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>("/devices", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }

        /// <summary>
        ///     Updates a device's properties.
        /// </summary>
        /// <param name="device">The <see cref="Device"/> to update.</param>
        /// <param name="parameters">The <see cref="DeviceParameters"/> to update the device with.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post operation. This task will resolve with a <see cref="Device"/> entity representing the updated device.</returns>
        public Task<Device> UpdateDeviceAsync(Device device, DeviceParameters parameters)
            => UpdateDeviceAsync(device.Identifier, parameters);

        /// <summary>
        ///     Updates a device's properties.
        /// </summary>
        /// <param name="identifier">The string identifier of the device to update.</param>
        /// <param name="parameters">The <see cref="DeviceParameters"/> to update the device with.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post operation. This task will resolve with a <see cref="Device"/> entity representing the updated device.</returns>
        public async Task<Device> UpdateDeviceAsync(string identifier, DeviceParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>($"/devices/{identifier}", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            return new Device(deviceModel);
        }

        /// <summary>
        ///     Deletes a device from the client's account.
        /// </summary>
        /// <param name="device">The <see cref="Device"/> to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous delete operation.</returns>
        /// <exception cref="Exception">Thrown if the device is not active.</exception>
        public Task DeleteDeviceAsync(Device device)
        {
            if(!device.IsActive)
                throw new Exception("device must be active");

            return DeleteDeviceAsync(device.Identifier);
        }

        /// <summary>
        ///     Deletes a device from the client's account.
        /// </summary>
        /// <param name="identifier">The string identifier of the device to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous delete operation.</returns>
        /// <exception cref="Exception">Thrown if the device is not active.</exception>
        public async Task DeleteDeviceAsync(string identifier)
            => await _requests.SendAsync<EmptyModel>($"/devices/{identifier}", HttpMethod.Delete, true, 1, null).ConfigureAwait(false);
    }
}
