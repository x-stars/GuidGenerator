#if !UUIDREV_DISABLE
using System;

namespace XNetEx.Guids.Components;

internal abstract class CustomizedGuidComponents : GuidComponents, ICustomizedGuidComponents
{
    protected CustomizedGuidComponents() { }

    public sealed override byte[] GetCustomData(ref Guid guid, out byte[] bitmask)
    {
        return GuidComponents.FixedFormat.GetCustomData(ref guid, out bitmask);
    }

    public sealed override void SetCustomData(ref Guid guid, byte[] customData)
    {
        GuidComponents.FixedFormat.SetCustomData(ref guid, customData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        GuidComponents.FixedFormat.WriteCustomData(ref guid, destination, bitmask);
    }

    public sealed override void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData)
    {
        GuidComponents.FixedFormat.SetCustomData(ref guid, customData);
    }
#endif

    internal sealed new class Version8 : CustomizedGuidComponents
    {
        internal static readonly CustomizedGuidComponents.Version8 Instance =
            new CustomizedGuidComponents.Version8();

        private Version8() : base() { }
    }
}
#endif
