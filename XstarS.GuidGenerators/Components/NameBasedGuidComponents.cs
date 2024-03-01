using System;

namespace XNetEx.Guids.Components;

internal abstract class NameBasedGuidComponents : GuidComponents, INameBasedGuidComponents
{
    protected NameBasedGuidComponents() { }

    public sealed override byte[] GetHashData(ref Guid guid, out byte[] bitmask)
    {
        return GuidComponents.FixedFormat.GetHashData(ref guid, out bitmask);
    }

    public sealed override void SetHashData(ref Guid guid, byte[] hashData)
    {
        GuidComponents.FixedFormat.SetHashData(ref guid, hashData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        GuidComponents.FixedFormat.WriteHashData(ref guid, destination, bitmask);
    }

    public sealed override void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData)
    {
        GuidComponents.FixedFormat.SetHashData(ref guid, hashData);
    }
#endif

    internal sealed new class Version3 : NameBasedGuidComponents
    {
        internal static readonly NameBasedGuidComponents.Version3 Instance = new();

        private Version3() : base() { }
    }

    internal sealed new class Version5 : NameBasedGuidComponents
    {
        internal static readonly NameBasedGuidComponents.Version5 Instance = new();

        private Version5() : base() { }
    }
}
