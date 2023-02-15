using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Buffers.Binary;
using System.Runtime.InteropServices;
#endif

namespace XNetEx.Guids;

internal static class GuidRfc4122Fields
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref uint TimeLow(this ref Guid guid) =>
        ref guid.FieldAt<uint>(0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeMid(this ref Guid guid) =>
        ref guid.FieldAt<ushort>(4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref ushort TimeHi_Ver(this ref Guid guid) =>
        ref guid.FieldAt<ushort>(6);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqHi_Var(this ref Guid guid) =>
        ref guid.FieldAt<byte>(8);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte ClkSeqLow(this ref Guid guid) =>
        ref guid.FieldAt<byte>(9);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte NodeId(this ref Guid guid, int index) =>
        ref guid.FieldAt<byte>(10 + index);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Span<byte> NodeId(this ref Guid guid) =>
        MemoryMarshal.CreateSpan(ref guid.NodeId(0), 6);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe byte[] NodeIdToArray(this ref Guid guid)
    {
        const int nodeIdSize = 6;
        var nodeId = new byte[nodeIdSize];
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guid.NodeId().CopyTo((Span<byte>)nodeId);
#else
        fixed (byte* pGuidNodeId = &guid.NodeId(0), pNodeId = &nodeId[0])
        {
            Buffer.MemoryCopy(pGuidNodeId, pNodeId, nodeIdSize, nodeIdSize);
        }
#endif
        return nodeId;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetNodeId(this ref Guid guid, byte[] nodeId)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guid.SetNodeId((ReadOnlySpan<byte>)nodeId);
#else
        const int nodeIdSize = 6;
        Debug.Assert(nodeId.Length >= nodeIdSize);
        fixed (byte* pNodeId = &nodeId[0], pGuidNodeId = &guid.NodeId(0))
        {
            Buffer.MemoryCopy(pNodeId, pGuidNodeId, nodeIdSize, nodeIdSize);
        }
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetNodeId(this ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        const int nodeIdSize = 6;
        Debug.Assert(nodeId.Length >= nodeIdSize);
        nodeId[..nodeIdSize].CopyTo(guid.NodeId());
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ref T FieldAt<T>(this ref Guid guid, int offset)
        where T : unmanaged
    {
        Debug.Assert((offset >= 0) && (offset + sizeof(T) <= 16));
#if NETCOREAPP3_0_OR_GREATER
        return ref Unsafe.As<Guid, T>(
            ref Unsafe.AddByteOffset(ref guid, (nint)offset));
#else
        fixed (Guid* pGuid = &guid)
        {
            return ref *(T*)((byte*)pGuid + offset);
        }
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Guid ToBigEndian(this Guid guid)
    {
        var uuid = guid;
        if (BitConverter.IsLittleEndian)
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            ref var timeLow = ref uuid.TimeLow();
            timeLow = BinaryPrimitives.ReverseEndianness(timeLow);
            ref var timeMid = ref uuid.TimeMid();
            timeMid = BinaryPrimitives.ReverseEndianness(timeMid);
            ref var timeHi_Ver = ref uuid.TimeHi_Ver();
            timeHi_Ver = BinaryPrimitives.ReverseEndianness(timeHi_Ver);
#else
            var pGuid = (byte*)&guid;
            var pUuid = (byte*)&uuid;
            pUuid[0] = pGuid[3]; pUuid[1] = pGuid[2];
            pUuid[2] = pGuid[1]; pUuid[3] = pGuid[0];
            pUuid[4] = pGuid[5]; pUuid[5] = pGuid[4];
            pUuid[6] = pGuid[7]; pUuid[7] = pGuid[6];
#endif
        }
        return uuid;
    }
}
