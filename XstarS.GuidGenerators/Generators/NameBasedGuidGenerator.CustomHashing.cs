#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    internal partial class CustomHashing : NameBasedGuidGenerator
    {
        private readonly Func<HashAlgorithm> HashingFactory;

        private CustomHashing(Guid hashspaceId, Func<HashAlgorithm> hashingFactory)
            : base(hashspaceId)
        {
            this.HashingFactory = hashingFactory;
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

            protected override bool TrackHashing => true;

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

            protected override HashAlgorithm GetHashing()
            {
                if (this.DisposeState != 0)
                {
                    throw new ObjectDisposedException(nameof(NameBasedGuidGenerator));
                }
                return base.GetHashing();
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
            private readonly HashAlgorithm GlobalHashing;

            internal Synchronized(Guid hashspaceId, HashAlgorithm hashing)
                : base(hashspaceId, hashing.Identity)
            {
                this.GlobalHashing = hashing;
                this.LocalHashing.Dispose();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    lock (this.GlobalHashing)
                    {
                        this.GlobalHashing.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            protected override HashAlgorithm GetHashing()
            {
                var hashing = this.GlobalHashing;
                Monitor.Enter(hashing);
                return hashing;
            }

            protected override void ReturnHashing(HashAlgorithm hashing)
            {
                Debug.Assert(hashing == this.GlobalHashing);
                Monitor.Exit(hashing);
            }
        }
    }
}
#endif
