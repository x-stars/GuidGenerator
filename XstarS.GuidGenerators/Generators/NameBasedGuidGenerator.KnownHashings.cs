#if !UUIDREV_DISABLE
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
#if NET8_0_OR_GREATER
using XNetEx.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    partial class CustomHashing : NameBasedGuidGenerator
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
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha256, SHA256.Create);
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
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha384, SHA384.Create);
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
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha512, SHA512.Create);
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
                    using (var validatePlatform = SHA3_256.Create()) { }
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D256 ??=
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha3D256, SHA3_256.Create);
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
                    using (var validatePlatform = SHA3_384.Create()) { }
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D384 ??=
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha3D384, SHA3_384.Create);
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
                    using (var validatePlatform = SHA3_512.Create()) { }
                    return NameBasedGuidGenerator.CustomHashing.SingletonSha3D512 ??=
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Sha3D512, SHA3_512.Create);
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
                    using (var validatePlatform = new Shake128()) { }
                    return NameBasedGuidGenerator.CustomHashing.SingletonShake128 ??=
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Shake128, Shake128D.Create256);
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
                    using (var validatePlatform = new Shake256()) { }
                    return NameBasedGuidGenerator.CustomHashing.SingletonShake256 ??=
                        new NameBasedGuidGenerator.CustomHashing(GuidHashspaces.Shake256, Shake256D.Create512);
                }

                return NameBasedGuidGenerator.CustomHashing.SingletonShake256 ?? Initialize();
            }
        }
#endif
    }
}
#endif
