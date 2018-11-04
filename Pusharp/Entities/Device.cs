using Pusharp.RequestParameters;
using Pusharp.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Model = Pusharp.Models.DeviceModel;

namespace Pusharp.Entities
{
    public sealed class Device
    {
        private Model _model;
        private readonly PushBulletClient _client;

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
        public string Model => _model.Model; //TODO enum
        public string Nickname => _model.Nickname;
        public string PushToken => _model.PushToken;
        public string Type => _model.Type; //TODO enum
        public string Kind => _model.Kind; //TODO enum
        public string Fingerprint => _model.Fingerprint;
        public string KeyFingerprint => _model.KeyFingerprint;
        public string Icon => _model.Icon;
        public string RemoteFiles => _model.RemoteFiles;

        internal double InternalModified => _model.Modified;

        /// <summary>
        ///     Updates this device's properties, returning a mutated form of this device.
        /// </summary>
        /// <param name="parameterOperator">The <see cref="Action{DeviceParameters}" /> to use when updating this device.</param>
        public async Task ModifyAsync(Action<DeviceParameters> parameterOperator)
        {
            var parameters = new DeviceParameters();
            parameterOperator(parameters);

            await _client.RequestClient.SendAsync<Model>($"/v2/devices/{Identifier}", HttpMethod.Post, parameters)
                .ConfigureAwait(false);
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

            await _client.RequestClient.SendAsync<Model>($"/v2/devices/{Identifier}", HttpMethod.Delete, null)
                .ConfigureAwait(false);
        }

        public Task<Push> SendNoteAsync(Action<NotePushParameters> parameterOperator)
        {
            var parameters = new NotePushParameters();
            parameterOperator(parameters);

            return _client.PushNoteAsync(parameters, PushTarget.Device, Identifier);
        }

        public Task<Push> SendFileAsync(Action<FilePushParameters> parameterOperator)
        {
            var parameters = new FilePushParameters();
            parameterOperator(parameters);

            return _client.PushFileAsync(parameters, PushTarget.Device, Identifier);
        }

        public Task<Push> SendLinkAsync(Action<LinkPushParameters> parameterOperator)
        {
            var parameters = new LinkPushParameters();
            parameterOperator(parameters);

            return _client.PushLinkAsync(parameters, PushTarget.Device, Identifier);
        }
    }
}