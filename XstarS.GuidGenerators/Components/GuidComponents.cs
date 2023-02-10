using System;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_0_OR_GREATER
using System.Numerics;
#endif

namespace XNetEx.Guids.Components;

internal partial class GuidComponents : IGuidCommonComponents
{
    protected GuidComponents() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GuidVariant GetVariant(ref Guid guid)
    {
#if NETCOREAPP3_0_OR_GREATER
        var shiftVar = (uint)guid.ClkSeqHi_Var() & 0xE0;
        var lzcntVar = ~(shiftVar << (3 * 8));
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

    public virtual long GetTimestamp(ref Guid guid)
    {
        throw new NotSupportedException();
    }

    public virtual void SetTimestamp(ref Guid guid, long timestamp)
    {
        throw new NotSupportedException();
    }

    public virtual short GetClockSequence(ref Guid guid)
    {
        throw new NotSupportedException();
    }

    public virtual void SetClockSequence(ref Guid guid, short clockSeq)
    {
        throw new NotSupportedException();
    }

    public virtual byte[] GetNodeId(ref Guid guid)
    {
        throw new NotSupportedException();
    }

    public virtual void SetNodeId(ref Guid guid, byte[] nodeId)
    {
        throw new NotSupportedException();
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
        throw new NotSupportedException();
    }

    public virtual void SetDomain(ref Guid guid, DceSecurityDomain domain)
    {
        throw new NotSupportedException();
    }

    public virtual int GetLocalId(ref Guid guid)
    {
        throw new NotSupportedException();
    }

    public virtual void SetLocalId(ref Guid guid, int localId)
    {
        throw new NotSupportedException();
    }
}
