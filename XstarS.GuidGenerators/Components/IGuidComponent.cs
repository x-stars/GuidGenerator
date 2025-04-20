using System;

namespace XNetEx.Guids.Components;

internal interface IGuidVariantComponent
{
    GuidVariant GetVariant(ref Guid guid);

    void SetVariant(ref Guid guid, GuidVariant variant);

    string? TrySetVariant(ref Guid guid, GuidVariant variant);

    void SetVariantChecked(ref Guid guid, GuidVariant variant);
}

internal interface IGuidVersionComponent
{
    GuidVersion GetVersion(ref Guid guid);

    void SetVersion(ref Guid guid, GuidVersion version);

    string? TrySetVersion(ref Guid guid, GuidVersion version);

    void SetVersionChecked(ref Guid guid, GuidVersion version);
}

internal interface IGuidRawDataComponent
{
    byte[] GetRawData(ref Guid guid, out byte[] bitmask);

    void SetRawData(ref Guid guid, byte[] rawData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteRawData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        var rawData = this.GetRawData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)rawData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    ;
#endif

    void SetRawData(ref Guid guid, ReadOnlySpan<byte> rawData)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        this.SetRawData(ref guid, rawData.ToArray());
    }
#else
    ;
#endif
#endif
}

internal interface IGuidTimestampComponent
{
    long GetTimestamp(ref Guid guid);

    void SetTimestamp(ref Guid guid, long timestamp);

    string? TrySetTimestamp(ref Guid guid, long timestamp);

    void SetTimestampChecked(ref Guid guid, long timestamp);
}

internal interface IGuidClockSequenceComponent
{
    short GetClockSequence(ref Guid guid);

    void SetClockSequence(ref Guid guid, short clockSeq);

    string? TrySetClockSequence(ref Guid guid, short clockSeq);

    void SetClockSequenceChecked(ref Guid guid, short clockSeq);
}

internal interface IGuidNodeIdComponent
{
    byte[] GetNodeId(ref Guid guid);

    void SetNodeId(ref Guid guid, byte[] nodeId);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteNodeId(ref Guid guid, Span<byte> destination)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        var nodeId = this.GetNodeId(ref guid);
        ((ReadOnlySpan<byte>)nodeId).CopyTo(destination);
    }
#else
    ;
#endif

    void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        this.SetNodeId(ref guid, nodeId.ToArray());
    }
#else
    ;
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
    void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        var hashData = this.GetHashData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)hashData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    ;
#endif

    void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        this.SetHashData(ref guid, hashData.ToArray());
    }
#else
    ;
#endif
#endif
}

internal interface IGuidRandomDataComponent
{
    byte[] GetRandomData(ref Guid guid, out byte[] bitmask);

    void SetRandomData(ref Guid guid, byte[] randomData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        var randomData = this.GetRandomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)randomData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    ;
#endif

    void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        this.SetRandomData(ref guid, randomData.ToArray());
    }
#else
    ;
#endif
#endif
}

#if !UUIDREV_DISABLE
internal interface IGuidCustomDataComponent
{
    byte[] GetCustomData(ref Guid guid, out byte[] bitmask);

    void SetCustomData(ref Guid guid, byte[] customData);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        var customData = this.GetCustomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)customData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }
#else
    ;
#endif

    void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData)
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    {
        this.SetCustomData(ref guid, customData.ToArray());
    }
#else
    ;
#endif
#endif
}
#endif
