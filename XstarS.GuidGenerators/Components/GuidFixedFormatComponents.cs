using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Components;

internal sealed class GuidFixedFormatComponents : GuidComponents,
    IGuidNodeIdComponent, IGuidDomainComponent, IGuidLocalIdComponent
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
}
