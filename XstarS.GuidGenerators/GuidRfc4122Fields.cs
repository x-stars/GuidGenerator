using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators;

internal static unsafe class GuidRfc4122Fields
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref uint TimeLow(this ref Guid guid) =>
        ref *(uint*)guid.RefFieldAt(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeMid(this ref Guid guid) =>
        ref *(ushort*)guid.RefFieldAt(4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeHi_Ver(this ref Guid guid) =>
        ref *(ushort*)guid.RefFieldAt(6);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqHi_Var(this ref Guid guid) =>
        ref *(byte*)guid.RefFieldAt(8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqLow(this ref Guid guid) =>
        ref *(byte*)guid.RefFieldAt(9);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static byte* NodeId(this ref Guid guid) =>
        (byte*)guid.RefFieldAt(10);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetNodeId(this ref Guid guid, byte[] nodeId)
    {
        var size = Math.Min(nodeId.Length, 6);
        fixed (byte* pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(pNodeId, guid.NodeId(), size, size);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void* RefFieldAt(this ref Guid guid, int offset)
    {
        fixed (Guid* pGuid = &guid)
        {
            return (byte*)pGuid + offset;
        }
    }
}
