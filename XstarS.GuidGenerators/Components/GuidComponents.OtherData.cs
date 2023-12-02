using System;
using System.Runtime.CompilerServices;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif

namespace XNetEx.Guids.Components;

partial class GuidComponents
{
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
