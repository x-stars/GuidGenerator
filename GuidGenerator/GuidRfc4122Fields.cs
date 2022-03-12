using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal static unsafe class GuidRfc4122Fields
    {
        internal static ref uint TimeLow(this ref Guid guid) =>
            ref *(uint*)guid.RefFieldAt(0);

        internal static ref ushort TimeMid(this ref Guid guid) =>
            ref *(ushort*)guid.RefFieldAt(4);

        internal static ref ushort TimeHi_Ver(this ref Guid guid) =>
            ref *(ushort*)guid.RefFieldAt(6);

        internal static ref byte ClkSeqHi_Var(this ref Guid guid) =>
            ref *(byte*)guid.RefFieldAt(8);

        internal static ref byte ClkSeqLow(this ref Guid guid) =>
            ref *(byte*)guid.RefFieldAt(9);

        internal static byte* NodeID(this ref Guid guid) =>
            (byte*)guid.RefFieldAt(10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* RefFieldAt(this ref Guid guid, int offset)
        {
            fixed (Guid* pGuid = &guid) { return (byte*)pGuid + offset; }
        }
    }
}
