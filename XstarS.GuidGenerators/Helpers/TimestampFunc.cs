using System;

namespace XNetEx;

internal static class TimestampFunc
{
    internal static DateTime UtcDateTime(this Func<DateTimeOffset> timeFunc)
    {
        return timeFunc.Invoke().UtcDateTime;
    }
}
