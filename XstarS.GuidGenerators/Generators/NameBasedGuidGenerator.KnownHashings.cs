#if !UUIDREV_DISABLE
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    partial class CustomHashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha256;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha384;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha512;

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
    }
}
#endif
