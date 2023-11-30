using System;

namespace XNetEx.Guids.Components;

internal abstract class RandomizedGuidComponents : GuidComponents, IRandomizedGuidComponents
{
    protected RandomizedGuidComponents() { }

    public sealed override byte[] GetRandomData(ref Guid guid, out byte[] bitmask)
    {
        return this.GetRawData(ref guid, out bitmask);
    }

    public sealed override void SetRandomData(ref Guid guid, byte[] randomData)
    {
        this.SetRawData(ref guid, randomData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        this.WriteRawData(ref guid, destination, bitmask);
    }

    public sealed override void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData)
    {
        this.SetRawData(ref guid, randomData);
    }
#endif

    internal sealed new class Version4 : RandomizedGuidComponents
    {
        internal static readonly RandomizedGuidComponents.Version4 Instance =
            new RandomizedGuidComponents.Version4();

        private Version4() : base() { }
    }
}
