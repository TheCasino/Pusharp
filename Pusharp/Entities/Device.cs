using Pusharp.RequestParameters;
using Pusharp.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Voltaic;
using Voltaic.Serialization.Json;
using Model = Pusharp.Models.DeviceModel;

namespace Pusharp.Entities
{
    public class Device
    {
        private Model _model;
        private readonly JsonSerializer _serializer;
        private readonly PushBulletClient _client;

        internal Device(Model model, JsonSerializer serializer, PushBulletClient client)
        {
            _model = model;
            _serializer = serializer;
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
        public string Model => _model.Model; //TODO enum
        public string Nickname => _model.Nickname;
        public string PushToken => _model.PushToken;
        public string Type => _model.Type; //TODO enum
        public string Kind => _model.Kind; //TODO enum
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

            var result = await _client.RequestClient.SendAsync<Model>($"/devices/{Identifier}", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            _model = result;
        }

        /// <summary>
        ///     Deletes this device from the Pushbullet service.
        /// </summary>
        /// <returns>A <see cref="Task" /> representing the asynchronous delete operation.</returns>
        public async Task DeleteAsync()
        {
            if (!IsActive)
            {
                await _client.InternalLogAsync(new LogMessage(LogLevel.Error, "Only active devices can be deleted"))
                    .ConfigureAwait(false);
                    
                return;
            }

            var model = await _client.RequestClient.SendAsync<Model>($"/devices/{Identifier}", HttpMethod.Delete, null)
                .ConfigureAwait(false);

            _model = model;
        }

        public async Task<Push> SendPushAsync(DevicePushParameters parameters)
        {
            //kinda hacky... Can't think of a better way to do it though
            //TODO doesn't work think of a better way to do this
            var serialized = parameters.BuildContent(_serializer);
            var pushParameters = _serializer.ReadUtf8<PushParameters>(new Utf8String(serialized));
            pushParameters.DeviceIdentifier = Identifier;

            var push = await _client.SendPushAsync(pushParameters).ConfigureAwait(false);

            return push;
        }
    }
}