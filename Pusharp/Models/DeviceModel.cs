using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class DeviceModel
    {
        [ModelProperty("active")] public bool Active { get; set; }

        [ModelProperty("app_version")] public int AppVersion { get; set; }

        [ModelProperty("created")] public double Created { get; set; }

        [ModelProperty("iden")] public string Iden { get; set; }

        [ModelProperty("manufacturer")] public string Manufacturer { get; set; }

        [ModelProperty("model")] public string Model { get; set; }

        [ModelProperty("modified")] public double Modified { get; set; }

        [ModelProperty("nickname")] public string Nickname { get; set; }

        [ModelProperty("push_token")] public string PushToken { get; set; }

        [ModelProperty("type")] public string Type { get; set; }

        [ModelProperty("kind")] public string Kind { get; set; }

        [ModelProperty("generated_nickname")] public bool GeneratedNickname { get; set; }

        [ModelProperty("fingerprint")] public string Fingerprint { get; set; }

        [ModelProperty("key_fingerprint")] public string KeyFingerprint { get; set; }

        [ModelProperty("pushable")] public bool Pushable { get; set; }

        [ModelProperty("has_sms")] public bool HasSms { get; set; }

        [ModelProperty("has_mms")] public bool HasMms { get; set; }

        [ModelProperty("icon")] public string Icon { get; set; }

        [ModelProperty("remote_files")] public string RemoteFiles { get; set; }
    }
}