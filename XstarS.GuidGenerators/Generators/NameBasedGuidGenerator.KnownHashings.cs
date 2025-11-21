using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
using System;
using XNetEx.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    internal sealed class MD5Hashing : NameBasedGuidGenerator
    {
        private MD5Hashing() { }

        internal static NameBasedGuidGenerator.MD5Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.MD5Hashing Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.MD5Hashing());
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version3;

        protected override HashAlgorithm CreateHashing() => MD5.Create();
    }

    internal sealed class SHA1Hashing : NameBasedGuidGenerator
    {
        private SHA1Hashing() { }

        internal static NameBasedGuidGenerator.SHA1Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.SHA1Hashing Initialize()
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new NameBasedGuidGenerator.SHA1Hashing());
                }

                return Volatile.Read(ref field) ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version5;

        protected override HashAlgorithm CreateHashing() => SHA1.Create();
    }

#if !UUIDREV_DISABLE
    partial class CustomHashing
    {
        // Don't use `Lazy<T>` for lazy initialization for performance issues
        // in .NET Framework (fixed in .NET Core).

        // Don't use nested class field for lazy initialization, because inlined static field accessing
        // will cause unconditional initialization in .NET Framework (fixed in .NET Core).

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
#endif
}
