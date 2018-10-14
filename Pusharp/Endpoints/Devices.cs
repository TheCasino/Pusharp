using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        /// <summary>
        ///     Retrieves a list of all devices under the client's account.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task" /> representing the asynchronous get operation. This task will resolve with a list of
        ///     <see cref="Device" /> entities, representing the devices under the client's account.
        /// </returns>
        public async Task<IReadOnlyCollection<Device>> GetDevicesAsync()
        {
            var devicesModel = await RequestClient.SendAsync<DevicesModel>("/v2/devices", HttpMethod.Get, null)
                .ConfigureAwait(false);
            var downloadedDevices = devicesModel.Models;

            return downloadedDevices.Select(x => new Device(x, this)).ToImmutableList();
        }

        internal async Task<IReadOnlyCollection<DeviceModel>> InternalGetDevicesAsync(double after)
        {
            var devicesModel = await RequestClient.SendAsync<DevicesModel>($"/v2/devices?modified_after={after}", HttpMethod.Get, null)
                .ConfigureAwait(false);

            return devicesModel.Models;
        }

        /// <summary>
        ///     Creates a device under the client's account.
        /// </summary>
        /// <param name="deviceParameters">The <see cref="DeviceParameters" /> to set up the new device with.</param>
        /// <returns>
        ///     A <see cref="Task" /> representing the asynchronous post operation. This task will resolve with a
        ///     <see cref="Device" /> entity representing the created device.
        /// </returns>
        public async Task<Device> CreateDeviceAsync(DeviceParameters deviceParameters)
        {
            var deviceModel = await RequestClient.SendAsync<DeviceModel>("/v2/devices", HttpMethod.Post, deviceParameters)
                .ConfigureAwait(false);
            var device = new Device(deviceModel, this);
            return device;
        }

        public async Task<Device> GetDeviceAsync(string identifier)
        {
            var deviceModel = await RequestClient
                .SendAsync<DeviceModel>($"/v2/devices/{identifier}", HttpMethod.Get, null)
                .ConfigureAwait(false);

            return new Device(deviceModel, this);
        }

    }
}