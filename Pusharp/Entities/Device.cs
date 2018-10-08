using System;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Pusharp.Utilities;
using Model = Pusharp.Models.DeviceModel;

namespace Pusharp.Entities
{
    public class Device
    {
        private readonly PushBulletClient _client;
        private readonly Model _model;

        public bool IsActive => _model.Active;
        public bool GeneratedNickname => _model.GeneratedNickname;
        public bool IsPushabke => _model.Pushable;
        public bool SmsEnabled => _model.HasSms;
        public bool MmsEnabled => _model.HasMms;

        public int AppVersion => _model.AppVersion;

        public DateTimeOffset Created => _model.Created.ToDateTime();
        public DateTimeOffset Modified => _model.Modified.ToDateTime();

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

        internal Device(Model model, PushBulletClient client)
        {
            _model = model;
            _client = client;
        }

        /// <summary>
        ///     Updates this device's properties, returning a mutated form of this device.
        /// </summary>
        /// <param name="parameterOperator">The <see cref="Action{DeviceParameters}"/> to use when updating this device.</param>
        /// <returns>A new <see cref="Device"/>, representing the updated device.</returns>
        public Task<Device> ModifyAsync(Action<DeviceParameters> parameterOperator)
        {
            var parameters = new DeviceParameters();
            parameterOperator(parameters);
            return _client.UpdateDeviceAsync(Identifier, parameters);
        }
    }
}
