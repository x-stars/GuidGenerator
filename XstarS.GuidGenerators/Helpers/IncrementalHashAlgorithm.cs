// Copyright (c) 2023 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

#pragma warning disable
#nullable enable
#define CHECK_DISPOSED

namespace System.Security.Cryptography
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
#if !NETCOREAPP3_0_OR_GREATER
    using System.Runtime.InteropServices;
#endif

    /// <summary>
    /// Provides extension methods for <see cref="HashAlgorithm"/>
    /// for computing a hash value incrementally across several segments.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
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
#if IS_TRIMMABLE || NET5_0_OR_GREATER
        [DynamicDependency(nameof(MethodBridge.Instance), typeof(MethodBridge))]
        [DynamicDependency(nameof(MethodBridge.AppendData) +
            "(System.Byte[],System.Int32,System.Int32)", typeof(MethodBridge))]
#endif
        public static void AppendData(this HashAlgorithm hashing, byte[] buffer)
        {
            IncrementalHashAlgorithm.ThrowIfNull(hashing);
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
#if IS_TRIMMABLE || NET5_0_OR_GREATER
        [DynamicDependency(nameof(MethodBridge.Instance), typeof(MethodBridge))]
        [DynamicDependency(nameof(MethodBridge.AppendData) +
            "(System.Byte[],System.Int32,System.Int32)", typeof(MethodBridge))]
#endif
        public static void AppendData(
            this HashAlgorithm hashing, byte[] buffer, int offset, int count)
        {
            IncrementalHashAlgorithm.ThrowIfNull(hashing);
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
#if IS_TRIMMABLE || NET5_0_OR_GREATER
        [DynamicDependency(nameof(MethodBridge.Instance), typeof(MethodBridge))]
        [DynamicDependency(nameof(MethodBridge.AppendData) +
            "(System.ReadOnlySpan{System.Byte})", typeof(MethodBridge))]
#endif
        public static void AppendData(this HashAlgorithm hashing, ReadOnlySpan<byte> source)
        {
            IncrementalHashAlgorithm.ThrowIfNull(hashing);
            hashing.AsBridge().AppendData(source);
        }
#endif

        /// <summary>
        /// Finalizes the hash computation and retrieves the hash value.
        /// </summary>
        /// <param name="hashing">The <see cref="HashAlgorithm"/>.</param>
        /// <returns>The computed hash value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="hashing"/> object has already been disposed.</exception>
#if IS_TRIMMABLE || NET5_0_OR_GREATER
        [DynamicDependency(nameof(MethodBridge.Instance), typeof(MethodBridge))]
        [DynamicDependency(nameof(MethodBridge.GetFinalHash) + "()", typeof(MethodBridge))]
#endif
        public static byte[] GetFinalHash(this HashAlgorithm hashing)
        {
            IncrementalHashAlgorithm.ThrowIfNull(hashing);
            return hashing.AsBridge().GetFinalHash();
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Finalizes the hash computation and attempts to retrieve the hash value.
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
#if IS_TRIMMABLE || NET5_0_OR_GREATER
        [DynamicDependency(nameof(MethodBridge.Instance), typeof(MethodBridge))]
        [DynamicDependency(nameof(MethodBridge.TryGetFinalHash) +
            "(System.Span{System.Byte},System.Int32@)", typeof(MethodBridge))]
#endif
        public static bool TryGetFinalHash(
            this HashAlgorithm hashing, Span<byte> destination, out int bytesWritten)
        {
            IncrementalHashAlgorithm.ThrowIfNull(hashing);
            return hashing.AsBridge().TryGetFinalHash(destination, out bytesWritten);
        }
#endif

        private static void ThrowIfNull(this HashAlgorithm hashing)
        {
            if (hashing is null)
            {
                throw new ArgumentNullException(nameof(hashing));
            }
        }

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static MethodBridge AsBridge(this HashAlgorithm hashing)
        {
#if NETCOREAPP3_0_OR_GREATER
            return Unsafe.As<MethodBridge>(hashing);
#else
            return new UncheckedCasting() { Source = hashing }.Target!;
#endif
        }

#if !NETCOREAPP3_0_OR_GREATER
        [DebuggerNonUserCode, ExcludeFromCodeCoverage]
        [StructLayout(LayoutKind.Explicit)]
        private struct UncheckedCasting
        {
            [FieldOffset(0)] public HashAlgorithm Source;
            [FieldOffset(0)] public MethodBridge Target;
            [FieldOffset(0)] public StrongBox<bool> Fields;
        }
#endif

        [DebuggerNonUserCode, ExcludeFromCodeCoverage]
        private abstract class MethodBridge : HashAlgorithm
        {
            internal static readonly MethodBridge Instance = new NonPublicMembers();

            public void AppendData(byte[] buffer, int offset, int count)
            {
                this.CheckDisposed();
                this.ValidateInput(buffer, offset, count);
#if NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                this.State = 1;
#endif
                this.HashCore(buffer, offset, count);
            }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            public void AppendData(ReadOnlySpan<byte> source)
            {
                this.CheckDisposed();
                this.State = 1;
                this.HashCore(source);
            }
#endif

            public byte[] GetFinalHash()
            {
                this.CheckDisposed();
                var hash = this.HashFinal();
#if NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                this.HashValue = hash;
                this.State = 0;
#endif
                return (byte[])hash.Clone();
            }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            public bool TryGetFinalHash(Span<byte> destination, out int bytesWritten)
            {
                this.CheckDisposed();
                if (this.TryHashFinal(destination, out bytesWritten))
                {
                    this.HashValue = null;
                    this.State = 0;
                    return true;
                }
                return false;
            }
#endif

            [Conditional("CHECK_DISPOSED")]
            private void CheckDisposed()
            {
#if CHECK_DISPOSED
                if (NonPublicMembers.GetDisposedField(this))
                {
                    throw new ObjectDisposedException(nameof(HashAlgorithm));
                }
#endif
            }

            private void ValidateInput(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                    throw new ArgumentNullException(nameof(buffer));
                if (offset < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(offset), "Non-negative number required.");
                if ((count < 0) || (count > buffer.Length))
                    throw new ArgumentException("Value was invalid.");
                if ((buffer.Length - count) < offset)
                    throw new ArgumentException(
                        "Offset and length were out of bounds for the array " +
                        "or count is greater than the number of elements " +
                        "from index to the end of the source collection.");
            }

            [DebuggerNonUserCode, ExcludeFromCodeCoverage]
            private sealed class NonPublicMembers : MethodBridge
            {
#if CHECK_DISPOSED
                private static readonly int DisposedFieldOffset =
                    NonPublicMembers.GetDisposedFieldOffset();

                private readonly bool EndField;

#if NET45_OR_GREATER || NETCOREAPP || NETSTANDARD
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
                public static unsafe bool GetDisposedField(MethodBridge instance)
                {
                    if (NonPublicMembers.DisposedFieldOffset < 0) { return false; }
#if NETCOREAPP3_0_OR_GREATER
                    var instField0 = Unsafe.As<StrongBox<bool>>(instance);
                    return Unsafe.AddByteOffset(
                        ref instField0.Value, (nint)NonPublicMembers.DisposedFieldOffset);
#else
                    var instField0 = new UncheckedCasting() { Target = instance }.Fields!;
                    fixed (bool* pField0 = &instField0.Value)
                    {
                        return pField0[NonPublicMembers.DisposedFieldOffset];
                    }
#endif
                }

                private static unsafe int GetDisposedFieldOffset()
                {
                    var instance = new NonPublicMembers();
#if NETCOREAPP3_0_OR_GREATER
                    var instField0 = Unsafe.As<StrongBox<bool>>(instance);
#else
                    var instField0 = new UncheckedCasting() { Target = instance }.Fields!;
#endif
                    fixed (bool* pField0 = &instField0.Value, pFieldEnd = &instance.EndField)
                    {
                        var values = stackalloc bool[(int)(pFieldEnd - pField0)];
                        for (bool* pField = pField0; pField < pFieldEnd; pField++)
                        {
                            values[pField - pField0] = *pField;
                        }
                        instance.Dispose();
                        for (bool* pField = pField0; pField < pFieldEnd; pField++)
                        {
                            if (*pField && !values[pField - pField0])
                            {
                                return (int)(pField - pField0);
                            }
                        }
                    }
                    Debug.Fail("Cannot find the disposed flag field.");
                    return -1;
                }
#endif

                public override void Initialize() => throw new NotImplementedException();

                protected override void HashCore(byte[] array, int ibStart, int cbSize) { }

                protected override byte[] HashFinal() => throw new NotImplementedException();
            }
        }
    }
}

#if !(EXCLUDE_FROM_CODE_COVERAGE_ATTRIBUTE || NETCOREAPP3_0_OR_GREATER)
#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    // Excludes the attributed code from code coverage information.
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
