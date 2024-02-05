#if !UUIDREV_DISABLE
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
#if NET8_0_OR_GREATER
using System;
using XNetEx.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    partial class CustomHashing
    {
        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha256;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha384;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha512;

#if NET8_0_OR_GREATER
        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha3D256;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha3D384;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha3D512;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonShake128;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonShake256;
#endif

        internal static NameBasedGuidGenerator.CustomHashing InstanceSha256
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.CustomHashing Initialize()
                {
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha256 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA256.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha256 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha384 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA384.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha384 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha512 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA512.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha512 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D256 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA3_256.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha3D256 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D384 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA3_384.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha3D384 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D512 ??=
                        new NameBasedGuidGenerator.CustomHashing(SHA3_512.Create);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonSha3D512 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonShake128 ??=
                        new NameBasedGuidGenerator.CustomHashing(Shake128D.Create256);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonShake128 ?? Initialize();
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
                    return NameBasedGuidGenerator.CustomHashing.SingletonShake256 ??=
                        new NameBasedGuidGenerator.CustomHashing(Shake256D.Create512);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonShake256 ?? Initialize();
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
