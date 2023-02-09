using System;

namespace XNetEx.Guids;

internal static class TimestampEpochs
{
    internal static readonly DateTime Gregorian =
        // new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
        new DateTime(499163040000000000L, DateTimeKind.Utc);

    internal static readonly DateTime UnixTime =
        // new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        new DateTime(621355968000000000L, DateTimeKind.Utc);

    internal static readonly DateTime Epoch2020 =
        // new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        new DateTime(637134336000000000L, DateTimeKind.Utc);
}
