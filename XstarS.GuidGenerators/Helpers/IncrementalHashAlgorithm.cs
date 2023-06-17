using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#if !NETCOREAPP3_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace System.Security.Cryptography;

/// <summary>
/// Provides extension methods for <see cref="HashAlgorithm"/>
/// for computing a hash value incrementally across several segments.
/// </summary>
internal static class IncrementalHashAlgorithm
{
    /// <summary>
    /// Appends the specified data into the hash algorithm for computing the hash.
    /// </summary>
    /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
    /// <param name="buffer">The input to compute the hash for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="hashing"/>
    /// or <paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> object has already been disposed.</exception>
    [DynamicDependency(nameof(MethodBridge.AppendData), typeof(MethodBridge))]
    public static void AppendData(this HashAlgorithm hashing, byte[] buffer)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(buffer, 0, buffer?.Length ?? 0);
    }

    /// <summary>
    /// Appends the specified data into the hash algorithm for computing the hash.
    /// </summary>
    /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
    /// <param name="buffer">The input to compute the hash for.</param>
    /// <param name="offset">The offset into the byte array from which to begin using data.</param>
    /// <param name="count">The number of bytes in the byte array to use as data.</param>
    /// <exception cref="ArgumentNullException"><paramref name="hashing"/>
    /// or <paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count"/> or <paramref name="offset"/> is negative;
    /// or <paramref name="count"/> is larger than the length of <paramref name="buffer"/>.</exception>
    /// <exception cref="ArgumentException">The sum of <paramref name="offset"/>
    /// and <paramref name="count"/> is larger than the length of <paramref name="buffer"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> object has already been disposed.</exception>
    [DynamicDependency(nameof(MethodBridge.AppendData), typeof(MethodBridge))]

    public static void AppendData(
        this HashAlgorithm hashing, byte[] buffer, int offset, int count)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(buffer, offset, count);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Appends the specified data into the hash algorithm for computing the hash.
    /// </summary>
    /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
    /// <param name="source">The input to compute the hash for.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> object has already been disposed.</exception>
    [DynamicDependency(nameof(MethodBridge.AppendData), typeof(MethodBridge))]
    public static void AppendData(this HashAlgorithm hashing, ReadOnlySpan<byte> source)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(source);
    }
#endif

    /// <summary>
    /// Finalize the hash computation and retrieves the hash value.
    /// </summary>
    /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
    /// <returns>The computed hash value.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> object has already been disposed.</exception>
    [DynamicDependency(nameof(MethodBridge.GetFinalHash), typeof(MethodBridge))]
    public static byte[] GetFinalHash(this HashAlgorithm hashing)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        return hashing.AsBridge().GetFinalHash();
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Finalize the hash computation and attempts to retrieve the hash value.
    /// </summary>
    /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
    /// <param name="destination">The buffer to receive the hash value.</param>
    /// <param name="bytesWritten">When this method returns,
    /// the total number of bytes written into <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="destination"/> is long enough
    /// to receive the hash value; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> object has already been disposed.</exception>
    [DynamicDependency(nameof(MethodBridge.TryGetFinalHash), typeof(MethodBridge))]
    public static bool TryGetFinalHash(
        this HashAlgorithm hashing, Span<byte> destination, out int bytesWritten)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        return hashing.AsBridge().TryGetFinalHash(destination, out bytesWritten);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MethodBridge AsBridge(this HashAlgorithm hashing)
    {
#if NETCOREAPP3_0_OR_GREATER
        return Unsafe.As<MethodBridge>(hashing);
#else
        return new UncheckedCasting() { Source = hashing }.Target!;
#endif
    }

#if !NETCOREAPP3_0_OR_GREATER
    [StructLayout(LayoutKind.Explicit)]
    private struct UncheckedCasting
    {
        [FieldOffset(0)] public HashAlgorithm Source;
        [FieldOffset(0)] public MethodBridge Target;
    }
#endif

    private abstract class MethodBridge : HashAlgorithm
    {
        public void AppendData(byte[] buffer, int offset, int count)
        {
            this.ValidateInput(buffer, offset, count);
            this.State = 1;
            this.HashCore(buffer, offset, count);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public void AppendData(ReadOnlySpan<byte> source)
        {
            this.State = 1;
            this.HashCore(source);
        }
#endif

        public byte[] GetFinalHash()
        {
            var hash = this.HashFinal();
            this.HashValue = hash;
            this.State = 0;
            return (byte[])hash.Clone();
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public bool TryGetFinalHash(Span<byte> destination, out int bytesWritten)
        {
            var result = this.TryHashFinal(destination, out bytesWritten);
            this.HashValue = null;
            this.State = 0;
            return result;
        }
#endif

        private void ValidateInput(byte[] buffer, int offset, int count)
        {
            Validator.Instance.TransformBlock(buffer, offset, count, null, 0);
        }

        private sealed class Validator : MethodBridge
        {
            internal static readonly Validator Instance = new Validator();

            public override void Initialize() => throw new NotImplementedException();

            protected override void HashCore(byte[] array, int ibStart, int cbSize) { }

            protected override byte[] HashFinal() => throw new NotImplementedException();
        }
    }
}
