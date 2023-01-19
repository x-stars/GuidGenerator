using System;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace XNetEx.Guids;

static partial class GuidExtensions
{
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
        var uuid = new Guid(bytes);
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        uuid = MemoryMarshal.Read<Guid>((ReadOnlySpan<byte>)bytes);
#else
        fixed (byte* pBytes = &bytes[0])
        {
            uuid = *(Guid*)pBytes;
        }
#endif
        return uuid.ToBigEndian();
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
        var uuid = new Guid(bytes);
        uuid = MemoryMarshal.Read<Guid>(bytes);
        return uuid.ToBigEndian();
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
        if (destination.Length < 16) { return false; }
        var uuid = guid.ToBigEndian();
        MemoryMarshal.Write(destination, ref uuid);
        return true;
    }
#endif
}
