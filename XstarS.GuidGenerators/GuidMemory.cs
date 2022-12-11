#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Runtime.CompilerServices;
#if !NETCOREAPP3_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace XNetEx.Guids;

internal static class GuidMemory
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Guid Read(ReadOnlySpan<byte> bytes) =>
#if NETCOREAPP3_0_OR_GREATER
        Unsafe.ReadUnaligned<Guid>(ref Unsafe.AsRef(in bytes[0]));
#else
        MemoryMarshal.Read<Guid>(bytes);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void Write(Span<byte> bytes, Guid guid) =>
#if NETCOREAPP3_0_OR_GREATER
        Unsafe.WriteUnaligned<Guid>(ref bytes[0], guid);
#else
        MemoryMarshal.Write<Guid>(bytes, ref guid);
#endif
}
#endif
