using System;

namespace XNetEx;

internal static class TimestampFunc
{
    public static DateTime UtcDateTime(
        this Func<DateTimeOffset> timeFunc)
    {
        return timeFunc.Invoke().UtcDateTime;
    }
}
