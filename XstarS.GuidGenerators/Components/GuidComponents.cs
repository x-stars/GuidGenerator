using System;
using System.Runtime.CompilerServices;
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
    internal bool IsRfc4122Uuid(ref Guid guid)
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
    public string? TrySetVariant(ref Guid guid, GuidVariant variant)
    {
        if (variant > GuidVariant.Reserved)
        {
            return "Variant for Guid must be between 0 and 3.";
        }

        this.SetVariant(ref guid, variant);
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVariantChecked(ref Guid guid, GuidVariant variant)
    {
        if (this.TrySetVariant(ref guid, variant) is not null)
        {
            GuidComponents.ThrowArgumentOutOfRange(nameof(variant));
        }
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
    public string? TrySetVersion(ref Guid guid, GuidVersion version)
    {
        if (version > (GuidVersion)/*MaxValue*/15)
        {
            return "Version for Guid must be between 0 and 15.";
        }

        this.SetVersion(ref guid, version);
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVersionChecked(ref Guid guid, GuidVersion version)
    {
        if (this.TrySetVersion(ref guid, version) is not null)
        {
            GuidComponents.ThrowArgumentOutOfRange(nameof(version));
        }
    }

    public virtual long GetTimestamp(ref Guid guid)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetTimestamp(ref Guid guid, long timestamp)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual string? TrySetTimestamp(ref Guid guid, long timestamp)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetTimestampChecked(ref Guid guid, long timestamp)
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

    public virtual string? TrySetClockSequence(ref Guid guid, short clockSeq)
    {
        throw new NotSupportedException(GuidComponents.NotSupportedMessage);
    }

    public virtual void SetClockSequenceChecked(ref Guid guid, short clockSeq)
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowArgumentOutOfRange(string? paramName)
    {
        throw new ArgumentOutOfRangeException(paramName);
    }
}
