using System;
using System.Runtime.CompilerServices;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif
#if NETCOREAPP3_0_OR_GREATER
using System.Numerics;
#endif

namespace XNetEx.Guids.Components;

internal partial class GuidComponents : IGuidCommonComponents
{
    private const string NotSupportedMessage =
        "This instance does not support getting " +
        "or setting the specified Guid component.";

    protected GuidComponents() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsRfc4122Uuid(ref Guid guid)
    {
        var clkSeqHi_Var = guid.ClkSeqHi_Var();
        return (clkSeqHi_Var & 0xC0) == 0x80;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GuidVariant GetVariant(ref Guid guid)
    {
#if NETCOREAPP3_0_OR_GREATER
        var shiftVar = (int)guid.ClkSeqHi_Var() & 0xE0;
        var lzcntVar = ~((uint)shiftVar << (3 * 8));
        var variant = BitOperations.LeadingZeroCount(lzcntVar);
#else
        var variant = -1;
        var shiftVar = (int)guid.ClkSeqHi_Var() & 0xE0;
        while ((sbyte)(shiftVar << ++variant) < 0) { }
#endif
        return (GuidVariant)variant;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVariant(ref Guid guid, GuidVariant variant)
    {
        var shiftVar = -1 << (8 - (int)variant);
        var varMask = (shiftVar >> 1) & 0xE0;
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & ~varMask | shiftVar);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GuidVersion GetVersion(ref Guid guid)
    {
        var shiftVer = (int)guid.TimeHi_Ver() & 0xF000;
        var version = shiftVer >> (3 * 4);
        return (GuidVersion)version;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVersion(ref Guid guid, GuidVersion version)
    {
        var shiftVer = (int)version << (3 * 4);
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000 | shiftVer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearVarAndVer(ref Guid guid)
    {
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & ~0xC0);
        ref var timeHi_Ver = ref guid.TimeHi_Ver();
        timeHi_Ver = (ushort)(timeHi_Ver & ~0xF000);
    }

    public byte[] GetRawData(ref Guid guid, out byte[] bitmask)
    {
        var masked = guid;
        this.ClearVarAndVer(ref masked);
        var rawData = masked.ToUuidByteArray();
        var maskId = Uuid.MaxValue;
        this.ClearVarAndVer(ref maskId);
        bitmask = maskId.ToUuidByteArray();
        return rawData;
    }

    public void SetRawData(ref Guid guid, byte[] rawData)
    {
        var variant = this.GetVariant(ref guid);
        var version = this.GetVersion(ref guid);
        guid = Uuid.FromByteArray(rawData);
        this.SetVariant(ref guid, variant);
        this.SetVersion(ref guid, version);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public void WriteRawData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var masked = guid;
        this.ClearVarAndVer(ref masked);
        var maskedResult = masked.TryWriteUuidBytes(destination);
        Debug.Assert(maskedResult);
        var maskId = Uuid.MaxValue;
        this.ClearVarAndVer(ref maskId);
        var maskIdResult = maskId.TryWriteUuidBytes(bitmask);
        Debug.Assert(maskIdResult);
    }

    public void SetRawData(ref Guid guid, ReadOnlySpan<byte> rawData)
    {
        var variant = this.GetVariant(ref guid);
        var version = this.GetVersion(ref guid);
        guid = Uuid.FromBytes(rawData);
        this.SetVariant(ref guid, variant);
        this.SetVersion(ref guid, version);
    }
#endif

    public virtual long GetTimestamp(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetTimestamp(ref Guid guid, long timestamp)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual short GetClockSequence(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetClockSequence(ref Guid guid, short clockSeq)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual byte[] GetNodeId(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetNodeId(ref Guid guid, byte[] nodeId)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public virtual void WriteNodeId(ref Guid guid, Span<byte> destination)
    {
        var nodeId = this.GetNodeId(ref guid);
        ((ReadOnlySpan<byte>)nodeId).CopyTo(destination);
    }

    public virtual void SetNodeId(ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        this.SetNodeId(ref guid, nodeId.ToArray());
    }
#endif

    public virtual DceSecurityDomain GetDomain(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetDomain(ref Guid guid, DceSecurityDomain domain)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual int GetLocalId(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetLocalId(ref Guid guid, int localId)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual byte[] GetHashData(ref Guid guid, out byte[] bitmask)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetHashData(ref Guid guid, byte[] hashData)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public virtual void WriteHashData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var hashData = this.GetHashData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)hashData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }

    public virtual void SetHashData(ref Guid guid, ReadOnlySpan<byte> hashData)
    {
        this.SetHashData(ref guid, hashData.ToArray());
    }
#endif

    public virtual byte[] GetRandomData(ref Guid guid, out byte[] bitmask)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetRandomData(ref Guid guid, byte[] randomData)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public virtual void WriteRandomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var randomData = this.GetRandomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)randomData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }

    public virtual void SetRandomData(ref Guid guid, ReadOnlySpan<byte> randomData)
    {
        this.SetRandomData(ref guid, randomData.ToArray());
    }
#endif

#if !UUIDREV_DISABLE
    public virtual byte[] GetCustomData(ref Guid guid, out byte[] bitmask)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetCustomData(ref Guid guid, byte[] customData)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public virtual void WriteCustomData(ref Guid guid, Span<byte> destination, Span<byte> bitmask)
    {
        var customData = this.GetCustomData(ref guid, out var bitmaskData);
        ((ReadOnlySpan<byte>)customData).CopyTo(destination);
        ((ReadOnlySpan<byte>)bitmaskData).CopyTo(bitmask);
    }

    public virtual void SetCustomData(ref Guid guid, ReadOnlySpan<byte> customData)
    {
        this.SetCustomData(ref guid, customData.ToArray());
    }
#endif
#endif
}
