using System;

namespace Pusharp.Utilities
{
    internal static class DateTimeHelpers
    {
        public static DateTimeOffset ToDateTime(double time)
        {
            return ToDateTime((long) time);
        }

        public static DateTimeOffset ToDateTime(long time)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(time);
        }
    }
}