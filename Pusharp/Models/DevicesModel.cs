using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class DevicesModel
    {
        [ModelProperty("devices")] public DeviceModel[] Models { get; set; }
    }
}