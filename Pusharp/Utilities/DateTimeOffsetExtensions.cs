using System;

namespace Pusharp.Utilities
{
    internal static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset ToDateTime(this double time)
        {
            return ToDateTime((long) time);
        }

        public static DateTimeOffset ToDateTime(this long time)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(time);
        }
    }
}