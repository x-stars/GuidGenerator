using System;

namespace XNetEx.Guids.Components;

internal interface IGuidVariantComponent
{
    GuidVariant GetVariant(ref Guid guid);

    void SetVariant(ref Guid guid, GuidVariant variant);

    void SetVariantChecked(ref Guid guid, GuidVariant variant);
}

internal interface IGuidVersionComponent
{
    GuidVersion GetVersion(ref Guid guid);

    void SetVersion(ref Guid guid, GuidVersion version);

    void SetVersionChecked(ref Guid guid, GuidVersion version);
}

internal interface IGuidRawDataComponent
{
    byte[] GetRawData(ref Guid guid, out byte[] bitmask);

    void SetRawData(ref Guid guid, byte[] rawData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteRawData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var rawData = this.GetRawData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)rawData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    void WriteRawData(ref Guid guid, Span<byte> destination, Span<byte> bitmask);
#endif

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void SetRawData(ref Guid guid, ReadOnlySpan<byte> rawData)
    {
        this.SetRawData(ref guid, rawData.ToArray());
    }
#else
    void SetRawData(ref Guid guid, ReadOnlySpan<byte> rawData);
#endif
#endif
}

internal interface IGuidTimestampComponent
{
    long GetTimestamp(ref Guid guid);

    void SetTimestamp(ref Guid guid, long timestamp);

    void SetTimestampChecked(ref Guid guid, long timestamp);
}

internal interface IGuidClockSequenceComponent
{
    short GetClockSequence(ref Guid guid);

    void SetClockSequence(ref Guid guid, short clockSeq);

    void SetClockSequenceChecked(ref Guid guid, short clockSeq);
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

internal interface IGuidHashDataComponent
{
    byte[] GetHashData(ref Guid guid, out byte[] bitmask);

    void SetHashData(ref Guid guid, byte[] hashData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var hashData = this.GetHashData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)hashData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask);
#endif

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData)
    {
        this.SetHashData(ref guid, hashData.ToArray());
    }
#else
    void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData);
#endif
#endif
}

internal interface IGuidRandomDataComponent
{
    byte[] GetRandomData(ref Guid guid, out byte[] bitmask);

    void SetRandomData(ref Guid guid, byte[] randomData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var randomData = this.GetRandomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)randomData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask);
#endif

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData)
    {
        this.SetRandomData(ref guid, randomData.ToArray());
    }
#else
    void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData);
#endif
#endif
}

#if !UUIDREV_DISABLE
internal interface IGuidCustomDataComponent
{
    byte[] GetCustomData(ref Guid guid, out byte[] bitmask);

    void SetCustomData(ref Guid guid, byte[] customData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var customData = this.GetCustomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)customData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask);
#endif

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData)
    {
        this.SetCustomData(ref guid, customData.ToArray());
    }
#else
    void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData);
#endif
#endif
}
#endif
