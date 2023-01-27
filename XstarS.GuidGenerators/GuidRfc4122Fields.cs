using System;
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
    internal static unsafe void SetNodeId(this ref Guid guid, byte[] nodeId)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guid.SetNodeId((ReadOnlySpan<byte>)nodeId);
#else
        const int size = 6;
        fixed (byte* pNodeId = &nodeId[0], pGuidNodeId = &guid.NodeId(0))
        {
            Buffer.MemoryCopy(pNodeId, pGuidNodeId, size, size);
        }
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetNodeId(this ref Guid guid, ReadOnlySpan<byte> nodeId)
    {
        const int size = 6;
        nodeId[..size].CopyTo(guid.NodeId());
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ref T FieldAt<T>(this ref Guid guid, int offset)
        where T : unmanaged
    {
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
    internal static Guid ToBigEndian(this Guid guid)
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
            ref var timeLow = ref uuid.TimeLow();
            var timeLow02 = timeLow & 0x00FF00FF;
            var timeLow13 = timeLow & 0xFF00FF00;
            timeLow = ((timeLow02 >> 8) | (timeLow02 << 24)) +
                      ((timeLow13 << 8) | (timeLow13 >> 24));
            ref var timeMid = ref uuid.TimeMid();
            timeMid = (ushort)((timeMid >> 8) + (timeMid << 8));
            ref var timeHi_Ver = ref uuid.TimeHi_Ver();
            timeHi_Ver = (ushort)((timeHi_Ver >> 8) + (timeHi_Ver << 8));
#endif
        }
        return uuid;
    }
}
