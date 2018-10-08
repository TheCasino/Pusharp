using System;

namespace Pusharp.Utilities
{
    internal static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset ToDateTime(this double time)
            => ToDateTime((long)time);

        public static DateTimeOffset ToDateTime(this long time)
            => DateTimeOffset.FromUnixTimeMilliseconds(time);
    }
}