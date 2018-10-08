using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Pusharp.Utilities;
using Model = Pusharp.Models.DeviceModel;

namespace Pusharp.Entities
{
    public class Device
    {
        private readonly PushBulletClient _client;
        private Model _model;

        internal Device(Model model, PushBulletClient client)
        {
            Update(model);
            _client = client;
        }

        public bool IsActive { get; private set; }
        public bool GeneratedNickname { get; private set; }
        public bool IsPushable { get; private set; }
        public bool SmsEnabled { get; private set; }
        public bool MmsEnabled { get; private set; }

        public int AppVersion { get; private set; }

        public DateTimeOffset Created { get; private set; }
        public DateTimeOffset Modified { get; private set; }

        public string Identifier { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string Nickname { get; private set; }
        public string PushToken { get; private set; }
        public string Type { get; private set; }
        public string Kind { get; private set; }
        public string Fingerprint { get; private set; }
        public string KeyFingerprint { get; private set; }
        public string Icon { get; private set; }
        public string RemoteFiles { get; private set; }

        internal void Update(Model model)
        {
            _model = model;

            IsActive = model.Active;
            GeneratedNickname = model.GeneratedNickname;
            IsPushable = model.Pushable;
            SmsEnabled = model.HasSms;
            MmsEnabled = model.HasMms;
            AppVersion = model.AppVersion;
            Created = model.Created.ToDateTime();
            Modified = model.Modified.ToDateTime();
            Identifier = model.Iden;
            Manufacturer = model.Manufacturer;
            Model = model.Model;
            Nickname = model.Nickname;
            PushToken = model.PushToken;
            Type = model.Type;
            Kind = model.Kind;
            Fingerprint = model.Fingerprint;
            KeyFingerprint = model.KeyFingerprint;
            Icon = model.Icon;
            RemoteFiles = model.RemoteFiles;
        }

        /// <summary>
        ///     Updates this device's properties, returning a mutated form of this device.
        /// </summary>
        /// <param name="parameterOperator">The <see cref="Action{DeviceParameters}" /> to use when updating this device.</param>
        public async Task ModifyAsync(Action<DeviceParameters> parameterOperator)
        {
            var parameters = new DeviceParameters();
            parameterOperator(parameters);

            var result = await _client.RequestClient.SendAsync<Model>($"/devices/{Identifier}", HttpMethod.Post, true, 1, parameters).ConfigureAwait(false);
            Update(result);
        }

        /// <summary>
        ///     Deletes this device from the Pushbullet service.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous delete operation.</returns>
        public async Task DeleteAsync()
        {
            await _client.RequestClient.SendAsync($"/devices/{Identifier}", HttpMethod.Delete, true, 1, null).ConfigureAwait(false);
        }
    }
}