using System;
#if NET7_0_OR_GREATER
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
#endif
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace XNetEx.Guids;

static partial class GuidExtensions
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified 128-bit unsigned integer.
    /// </summary>
    /// <param name="value">A 128-bit unsigned integer
    /// containing the value of the GUID.</param>
    /// <returns>A new <see cref="Guid"/> instance
    /// of the specified 128-bit unsigned integer.</returns>
    [CLSCompliant(false)]
    public static Guid FromUInt128(UInt128 value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            return Unsafe.As<UInt128, Guid>(ref value);
        }

        var guid = default(Guid);
        var upper = (ulong)(value >> (8 * 8));
        guid.TimeLow() = (uint)(upper >> (4 * 8));
        guid.TimeMid() = (ushort)(upper >> (2 * 8));
        guid.TimeHi_Ver() = (ushort)(upper >> (0 * 8));
        var guidLower = MemoryMarshal.CreateSpan(ref guid.ClkSeqHi_Var(), 8);
        BinaryPrimitives.WriteUInt64BigEndian(guidLower, (ulong)value);
        return guid;
    }
#endif

    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified byte array in big-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element byte array
    /// containing the fields of the GUID in big-endian order.</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static unsafe Guid FromUuidByteArray(byte[] bytes)
    {
        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return GuidExtensions.FromUuidBytes((ReadOnlySpan<byte>)bytes);
#else
        if (bytes.Length != 16)
        {
            var throwsException = new Guid(bytes);
        }

        var uuid = default(Guid);
        fixed (byte* pBytes = &bytes[0])
        {
            uuid = *(Guid*)pBytes;
        }
        return uuid.ToBigEndian();
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified read-only byte span in big-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element read-only span
    /// containing the bytes of the fields of the GUID in big-endian order.</param>
    /// <returns>A new <see cref="Guid"/> instance of the specified byte span.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="bytes"/> is not 16 bytes long.</exception>
    public static Guid FromUuidBytes(ReadOnlySpan<byte> bytes)
    {
#if NET8_0_OR_GREATER
        return new Guid(bytes, bigEndian: true);
#else
        if (bytes.Length != 16)
        {
            var throwsException = new Guid(bytes);
        }

        var uuid = MemoryMarshal.Read<Guid>(bytes);
        return uuid.ToBigEndian();
#endif
    }
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Returns a 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>A 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.</returns>
    [CLSCompliant(false)]
    public static UInt128 ToUInt128(this Guid guid)
    {
        if (!BitConverter.IsLittleEndian)
        {
            return Unsafe.As<Guid, UInt128>(ref guid);
        }

        var upper =
            ((ulong)guid.TimeLow() << (4 * 8)) |
            ((ulong)guid.TimeMid() << (2 * 8)) |
            ((ulong)guid.TimeHi_Ver() << (0 * 8));
        var lower = BinaryPrimitives.ReadUInt64BigEndian(
            MemoryMarshal.CreateReadOnlySpan(ref guid.ClkSeqHi_Var(), 8));
        return new UInt128(upper, lower);
    }
#endif

    /// <summary>
    /// Returns a 16-element byte array that contains the fields
    /// of the <see cref="Guid"/> in big-endian order.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>A 16-element byte array that contains the fields
    /// of the <see cref="Guid"/> in big-endian order.</returns>
    public static unsafe byte[] ToUuidByteArray(this Guid guid)
    {
#if NET8_0_OR_GREATER
        return guid.ToByteArray(bigEndian: true);
#else
        var bytes = new byte[16];
        var uuid = guid.ToBigEndian();
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        MemoryMarshal.Write((Span<byte>)bytes, ref uuid);
#else
        fixed (byte* pBytes = &bytes[0])
        {
            *(Guid*)pBytes = uuid;
        }
#endif
        return bytes;
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the fields of the <see cref="Guid"/>
    /// into a span of bytes in big-endian order.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="destination">When this method returns <see langword="true"/>,
    /// contains the fields of the <see cref="Guid"/> in big-endian order.</param>
    /// <returns><see langword="true"/> if the <see cref="Guid"/> is successfully
    /// written to the specified span; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteUuidBytes(this Guid guid, Span<byte> destination)
    {
#if NET8_0_OR_GREATER
        return guid.TryWriteBytes(destination, bigEndian: true, out _);
#else
        var uuid = guid.ToBigEndian();
        return MemoryMarshal.TryWrite(destination, ref uuid);
#endif
    }
#endif
}
