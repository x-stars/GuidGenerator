#if !UUIDREV_DISABLE
using System;

namespace XNetEx.Guids.Components;

internal abstract class TimeRandomBasedGuidComponents
    : TimeBasedGuidComponents, ITimeRandomBasedGuidComponents
{
    protected TimeRandomBasedGuidComponents()
        : base(TimestampEpochs.UnixTime)
    {
    }

    public sealed override byte[] GetRandomData(ref Guid guid, out byte[] bitmask)
    {
        var randomData = this.GetRawData(ref guid, out bitmask);
        Array.Clear(randomData, 0, 6);
        Array.Clear(bitmask, 0, 6);
        return randomData;
    }

    public sealed override void SetRandomData(ref Guid guid, byte[] randomData)
    {
        var timestamp = this.GetTimestampCore(ref guid);
        this.SetRawData(ref guid, randomData);
        this.SetTimestampCore(ref guid, timestamp);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override void WriteRandomData(
        ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        this.WriteRawData(ref guid, destination, bitmask);
        destination[0..6].Clear();
        bitmask[0..6].Clear();
    }

    public sealed override void SetRandomData(
        ref Guid guid, ReadOnlySpan<byte> randomData)
    {
        var timestamp = this.GetTimestampCore(ref guid);
        this.SetRawData(ref guid, randomData);
        this.SetTimestampCore(ref guid, timestamp);
    }
#endif

    internal sealed new class Version7 : TimeRandomBasedGuidComponents
    {
        internal static readonly TimeRandomBasedGuidComponents.Version7 Instance = new();

        private Version7() : base() { }

        protected override long GetTimestampCore(ref Guid guid)
        {
            var tsField = (long)(
                ((ulong)guid.TimeLow() << (2 * 8)) |
                ((ulong)guid.TimeMid() << (0 * 8)));
            return tsField * TimeSpan.TicksPerMillisecond;
        }

        protected override void SetTimestampCore(ref Guid guid, long timestamp)
        {
            var tsField = timestamp / TimeSpan.TicksPerMillisecond;
            guid.TimeLow() = (uint)(tsField >> (2 * 8));
            guid.TimeMid() = (ushort)(tsField >> (0 * 8));
        }
    }
}
#endif
