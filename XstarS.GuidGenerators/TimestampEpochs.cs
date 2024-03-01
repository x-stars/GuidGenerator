using System;

namespace XNetEx.Guids;

internal static class TimestampEpochs
{
    internal static readonly DateTime Gregorian =
        // 1582-10-15T00:00:00Z
        new(499163040000000000L, DateTimeKind.Utc);

    internal static readonly DateTime UnixTime =
        // 1970-01-01T00:00:00Z
        new(621355968000000000L, DateTimeKind.Utc);

    internal static readonly DateTime Epoch2020 =
        // 2020-01-01T00:00:00Z
        new(637134336000000000L, DateTimeKind.Utc);
}
