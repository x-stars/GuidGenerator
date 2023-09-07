#if !UUIDREV_DISABLE
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    internal partial class CustomHashing : NameBasedGuidGenerator
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

        public sealed override bool RequiresInput => true;

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

            return new NameBasedGuidGenerator.CustomHashing.Synchronized(hashspaceId, hashing);
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
                throw new InvalidOperationException("The hash algorithm factory returns null.");
        }

        private sealed class Disposable : NameBasedGuidGenerator.CustomHashing, IDisposable
        {
            private volatile int DisposeState;

            internal Disposable(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
                : base(hashspaceId, hashingFactory)
            {
                this.DisposeState = 0;
            }

            protected override void Dispose(bool disposing)
            {
                if (Interlocked.CompareExchange(ref this.DisposeState, 1, 0) == 0)
                {
                    if (disposing)
                    {
                        this.DisposeHashings();
                    }
                    this.DisposeState = 2;
                }
                base.Dispose(disposing);
            }

            protected override void ReturnHashing(HashAlgorithm hashing)
            {
                if (this.DisposeState != 0)
                {
                    hashing.Dispose();
                    return;
                }
                base.ReturnHashing(hashing);
            }
        }

        private sealed class Synchronized : NameBasedGuidGenerator.CustomHashing, IDisposable
        {
            internal Synchronized(Guid hashspaceId, HashAlgorithm hashing)
                : base(hashspaceId, hashing.Identity)
            {
                this.FastHashing = hashing;
            }

            private HashAlgorithm DefaultHashing => this.FastHashing!;

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.DefaultHashing.Dispose();
                }
                base.Dispose(disposing);
            }

            protected override HashAlgorithm GetHashing()
            {
                var hashing = this.DefaultHashing;
                Monitor.Enter(hashing);
                return hashing;
            }

            protected override void ReturnHashing(HashAlgorithm hashing)
            {
                if (hashing != this.DefaultHashing)
                {
                    throw new InvalidOperationException(
                        "An unknown hash algorithm instance is returned.");
                }
                Monitor.Exit(hashing);
            }
        }
    }
}
#endif
