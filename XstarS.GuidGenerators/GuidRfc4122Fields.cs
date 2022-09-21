using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids;

internal static class GuidRfc4122Fields
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref uint TimeLow(this ref Guid guid) =>
        ref guid.RefFieldAt<uint>(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeMid(this ref Guid guid) =>
        ref guid.RefFieldAt<ushort>(4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeHi_Ver(this ref Guid guid) =>
        ref guid.RefFieldAt<ushort>(6);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqHi_Var(this ref Guid guid) =>
        ref guid.RefFieldAt<byte>(8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqLow(this ref Guid guid) =>
        ref guid.RefFieldAt<byte>(9);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte NodeId(this ref Guid guid, int index) =>
        ref guid.RefFieldAt<byte>(10 + index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe byte* NodeId(this ref Guid guid)
    {
        fixed (Guid* pGuid = &guid)
        {
            return (byte*)pGuid + 10;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetNodeId(this ref Guid guid, byte[] nodeId)
    {
        var size = Math.Min(nodeId.Length, 6);
        fixed (byte* pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(pNodeId, guid.NodeId(), size, size);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ref T RefFieldAt<T>(this ref Guid guid, int offset)
        where T : unmanaged
    {
        fixed (Guid* pGuid = &guid)
        {
            return ref *(T*)((byte*)pGuid + offset);
        }
    }
}
