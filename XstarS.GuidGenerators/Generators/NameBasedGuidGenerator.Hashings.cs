using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    internal sealed class MD5Hashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.MD5Hashing? Singleton;

        private MD5Hashing() { }

        internal static NameBasedGuidGenerator.MD5Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.MD5Hashing Initialize()
                {
                    return NameBasedGuidGenerator.MD5Hashing.Singleton ??=
                        new NameBasedGuidGenerator.MD5Hashing();
                }

                return NameBasedGuidGenerator.MD5Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version3;

        protected override HashAlgorithm CreateHashing() => MD5.Create();
    }

    internal sealed class SHA1Hashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.SHA1Hashing? Singleton;

        private SHA1Hashing() { }

        internal static NameBasedGuidGenerator.SHA1Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.SHA1Hashing Initialize()
                {
                    return NameBasedGuidGenerator.SHA1Hashing.Singleton ??=
                        new NameBasedGuidGenerator.SHA1Hashing();
                }

                return NameBasedGuidGenerator.SHA1Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version5;

        protected override HashAlgorithm CreateHashing() => SHA1.Create();
    }

    internal class CustomHashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha256;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha384;

        private static volatile NameBasedGuidGenerator.CustomHashing? SingletonSha512;

        private readonly Func<HashAlgorithm> HashingFactory;

        private CustomHashing(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
            : base(hashspaceId)
        {
            this.HashingFactory = hashingFactory;
        }

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

        public sealed override GuidVersion Version => GuidVersion.Version8;

        internal static NameBasedGuidGenerator.CustomHashing CreateInstance(
            Guid hashspaceId, HashAlgorithm hashing)
        {
            if (hashing is null)
            {
                throw new ArgumentNullException(nameof(hashing));
            }
            if (hashing.HashSize < 16 * 8)
            {
                throw new ArgumentException(
                    "The algorithm's hash size is less than 128 bits.",
                    nameof(hashing));
            }

            return new NameBasedGuidGenerator.CustomHashing.Disposable(hashspaceId, hashing.Identity);
        }

        internal static NameBasedGuidGenerator.CustomHashing CreateInstance(
            Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
        {
            if (hashingFactory is null)
            {
                throw new ArgumentNullException(nameof(hashingFactory));
            }

            return new NameBasedGuidGenerator.CustomHashing.Disposable(hashspaceId, hashingFactory);
        }

        protected sealed override HashAlgorithm CreateHashing()
        {
            return this.HashingFactory.Invoke() ??
                throw new InvalidOperationException(
                    "The hashing algorithm factory returns null.");
        }

        private sealed class Disposable : NameBasedGuidGenerator.CustomHashing, IDisposable
        {
            private volatile bool IsDisposed;

            internal Disposable(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
                : base(hashspaceId, hashingFactory)
            {
                this.IsDisposed = false;
            }

            public void Dispose()
            {
                if (this.IsDisposed) { return; }
                lock (this.Hashings)
                {
                    if (this.IsDisposed) { return; }
                    var hashings = this.Hashings;
                    hashings.CompleteAdding();
                    while (hashings.TryTake(out var hashing))
                    {
                        hashing.Dispose();
                    }
                    hashings.Dispose();
                    this.IsDisposed = true;
                }
            }
        }
    }
}
