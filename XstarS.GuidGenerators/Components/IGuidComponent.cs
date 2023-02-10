using System;

namespace XNetEx.Guids.Components;

internal interface IGuidVariantComponent
{
    GuidVariant GetVariant(ref Guid guid);

    void SetVariant(ref Guid guid, GuidVariant variant);
}

internal interface IGuidVersionComponent
{
    GuidVersion GetVersion(ref Guid guid);

    void SetVersion(ref Guid guid, GuidVersion version);
}

internal interface IGuidTimestampComponent
{
    long GetTimestamp(ref Guid guid);

    void SetTimestamp(ref Guid guid, long timestamp);
}

internal interface IGuidClockSequenceComponent
{
    short GetClockSequence(ref Guid guid);

    void SetClockSequence(ref Guid guid, short clockSeq);
}

internal interface IGuidNodeIdComponent
{
    byte[] GetNodeId(ref Guid guid);

    void SetNodeId(ref Guid guid, byte[] nodeId);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteNodeId(ref Guid guid, Span<byte> destination)
    {
        var nodeId = this.GetNodeId(ref guid);
        ((ReadOnlySpan<byte>)nodeId).CopyTo(destination);
    }
#else
    void WriteNodeId(ref Guid guid, Span<byte> destination);
#endif

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        this.SetNodeId(ref guid, nodeId.ToArray());
    }
#else
    void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId);
#endif
#endif
}

internal interface IGuidDomainComponent
{
    DceSecurityDomain GetDomain(ref Guid guid);

    void SetDomain(ref Guid guid, DceSecurityDomain domain);
}

internal interface IGuidLocalIdComponent
{
    int GetLocalId(ref Guid guid);

    void SetLocalId(ref Guid guid, int localId);
}
