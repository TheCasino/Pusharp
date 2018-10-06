using System;
using System.Collections.Generic;
using System.Linq;
using Pusharp.Models;
using Pusharp.Results;

namespace Pusharp
{
    internal static class ExtensionMethods
    {
        public static DateTimeOffset ToDateTime(this double time)
            => ToDateTime((long)time);

        public static DateTimeOffset ToDateTime(this long time)
            => DateTimeOffset.FromUnixTimeMilliseconds(time);

        public static IEnumerable<Device> ToDevices(this IEnumerable<DeviceModel> models)
            => models.Select(x => new Device(x));
    }
}
