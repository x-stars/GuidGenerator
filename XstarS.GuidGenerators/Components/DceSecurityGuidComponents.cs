﻿using System;

namespace XNetEx.Guids.Components;

internal abstract class DceSecurityGuidComponents
    : TimeNodeBasedGuidComponents, IDceSecurityGuidComponents
{
    protected DceSecurityGuidComponents() { }

    public sealed override short GetClockSequence(ref Guid guid)
    {
        return (short)((uint)(guid.ClkSeqHi_Var() & ~0xC0) << (0 * 8));
    }

    public sealed override void SetClockSequence(ref Guid guid, short clockSeq)
    {
        var clkSeqHi = (byte)((clockSeq >> (0 * 8)) & ~0xC0);
        ref var clkSeqHi_Var = ref guid.ClkSeqHi_Var();
        clkSeqHi_Var = (byte)(clkSeqHi_Var & 0xC0 | clkSeqHi);
    }

    public sealed override DceSecurityDomain GetDomain(ref Guid guid)
    {
        return (DceSecurityDomain)guid.ClkSeqLow();
    }

    public sealed override void SetDomain(ref Guid guid, DceSecurityDomain domain)
    {
        guid.ClkSeqLow() = (byte)domain;
    }

    public sealed override int GetLocalId(ref Guid guid)
    {
        return (int)guid.TimeLow();
    }

    public sealed override void SetLocalId(ref Guid guid, int localId)
    {
        guid.TimeLow() = (uint)localId;
    }

    internal sealed new class Version2 : DceSecurityGuidComponents
    {
        internal static readonly DceSecurityGuidComponents.Version2 Instance =
            new DceSecurityGuidComponents.Version2();

        private Version2() { }

        protected override long GetTimestampCore(ref Guid guid)
        {
            return (long)(
                ((ulong)guid.TimeMid() << (4 * 8)) |
                ((ulong)(guid.TimeHi_Ver() & ~0xF000) << (6 * 8)));
        }

        protected override void SetTimestampCore(ref Guid guid, long timestamp)
        {
            guid.TimeMid() = (ushort)(timestamp >> (4 * 8));
            var timeHi = (ushort)((timestamp >> (6 * 8)) & ~0xF000);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | timeHi);
        }
    }
}