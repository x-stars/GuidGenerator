using System;

namespace XNetEx;

internal static class TimestampFunc
{
    public static DateTime GetUtcDateTime(
        this Func<DateTimeOffset> timeFunc)
    {
        return timeFunc.Invoke().UtcDateTime;
    }
}
