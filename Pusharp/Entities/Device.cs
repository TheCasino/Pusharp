using Pusharp.RequestParameters;
using Pusharp.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Model = Pusharp.Models.DeviceModel;

namespace Pusharp.Entities
{
    public class Device
    {
        private readonly PushBulletClient _client;
        private Model _model;

        internal Device(Model model, PushBulletClient client)
        {
            _model = model;
            _client = client;
        }

        public bool IsActive => _model.Active;
        public bool GeneratedNickname => _model.GeneratedNickname;
        public bool IsPushabke => _model.Pushable;
        public bool SmsEnabled => _model.HasSms;
        public bool MmsEnabled => _model.HasMms;

        public int AppVersion => _model.AppVersion;

        public DateTimeOffset Created => DateTimeHelpers.ToDateTime(_model.Created);
        public DateTimeOffset Modified => DateTimeHelpers.ToDateTime(_model.Modified);

        public string Identifier => _model.Iden;
        public string Manufacturer => _model.Manufacturer;
        public string Model => _model.Model;
        public string Nickname => _model.Nickname;
        public string PushToken => _model.PushToken;
        public string Type => _model.Type;
        public string Kind => _model.Kind;
        public string Fingerprint => _model.Fingerprint;
        public string KeyFingerprint => _model.KeyFingerprint;
        public string Icon => _model.Icon;
        public string RemoteFiles => _model.RemoteFiles;

        /// <summary>
        ///     Updates this device's properties, returning a mutated form of this device.
        /// </summary>
        /// <param name="parameterOperator">The <see cref="Action{DeviceParameters}" /> to use when updating this device.</param>
        public async Task ModifyAsync(Action<DeviceParameters> parameterOperator)
        {
            var parameters = new DeviceParameters();
            parameterOperator(parameters);

            var result = await _client.RequestClient.SendAsync<Model>($"/devices/{Identifier}", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            _model = result;
        }

        /// <summary>
        ///     Deletes this device from the Pushbullet service.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous delete operation.</returns>
        public async Task DeleteAsync()
        {
            if(!IsActive)
                throw new Exception("Device must be active");

            await _client.RequestClient.SendAsync($"/devices/{Identifier}", HttpMethod.Delete, true, 1, null).ConfigureAwait(false);
        }
    }
}