using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Components;

internal sealed class GuidFixedFormatComponents : GuidComponents,
    IGuidNodeIdComponent, IGuidDomainComponent, IGuidLocalIdComponent,
    IGuidHashDataComponent, IGuidCustomDataComponent
{
    internal static readonly GuidFixedFormatComponents Instance =
        new GuidFixedFormatComponents();

    private GuidFixedFormatComponents() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override byte[] GetNodeId(ref Guid guid)
    {
        return guid.NodeIdToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void SetNodeId(ref Guid guid, byte[] nodeId)
    {
        guid.SetNodeId(nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void WriteNodeId(ref Guid guid, Span<byte> destination)
    {
        guid.NodeId().CopyTo(destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        guid.SetNodeId(nodeId);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override DceSecurityDomain GetDomain(ref Guid guid)
    {
        return (DceSecurityDomain)guid.ClkSeqLow();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void SetDomain(ref Guid guid, DceSecurityDomain domain)
    {
        guid.ClkSeqLow() = (byte)domain;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetLocalId(ref Guid guid)
    {
        return (int)guid.TimeLow();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void SetLocalId(ref Guid guid, int localId)
    {
        guid.TimeLow() = (uint)localId;
    }

    public override byte[] GetHashData(ref Guid guid, out byte[] bitmask)
    {
        return this.GetRawData(ref guid, out bitmask);
    }

    public override void SetHashData(ref Guid guid, byte[] hashData)
    {
        this.SetRawData(ref guid, hashData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public override void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        this.WriteRawData(ref guid, destination, bitmask);
    }

    public override void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData)
    {
        this.SetRawData(ref guid, hashData);
    }
#endif

#if !UUIDREV_DISABLE
    public override byte[] GetCustomData(ref Guid guid, out byte[] bitmask)
    {
        return this.GetRawData(ref guid, out bitmask);
    }

    public override void SetCustomData(ref Guid guid, byte[] customData)
    {
        this.SetRawData(ref guid, customData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public override void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        this.WriteRawData(ref guid, destination, bitmask);
    }

    public override void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData)
    {
        this.SetRawData(ref guid, customData);
    }
#endif
#endif
}
