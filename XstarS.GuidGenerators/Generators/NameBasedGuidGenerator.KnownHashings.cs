#if !UUIDREV_DISABLE
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
#if NET8_0_OR_GREATER
using System;
using XNetEx.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    partial class CustomHashing
    {
        // Don't use `Lazy<T>` for lazy initialization for performance issues
        // in .NET Framework (fixed in .NET Core).

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha256
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA256.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha384
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA384.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha512
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA512.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

#if NET8_0_OR_GREATER
        internal static NameBasedGuidGenerator.CustomHashing InstanceSha3D256
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    ThrowIfNotSupported(SHA3_256.IsSupported, HashAlgorithmNames.SHA3_256);
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA3_256.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha3D384
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    ThrowIfNotSupported(SHA3_384.IsSupported, HashAlgorithmNames.SHA3_384);
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA3_384.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha3D512
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    ThrowIfNotSupported(SHA3_512.IsSupported, HashAlgorithmNames.SHA3_512);
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(SHA3_512.Create));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceShake128
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    ThrowIfNotSupported(Shake128.IsSupported, HashAlgorithmNames.SHAKE128);
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(Shake128D.Create256));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        internal static NameBasedGuidGenerator.CustomHashing InstanceShake256
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    ThrowIfNotSupported(Shake256.IsSupported, HashAlgorithmNames.SHAKE256);
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.CustomHashing(Shake256D.Create512));
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        private static void ThrowIfNotSupported(bool isSupported, string hashingName)
        {
            if (!isSupported)
            {
                throw new PlatformNotSupportedException(
                    $"The {hashingName} hash algorithm is not supported on this platform.");
            }
        }
#endif
    }
}
#endif
