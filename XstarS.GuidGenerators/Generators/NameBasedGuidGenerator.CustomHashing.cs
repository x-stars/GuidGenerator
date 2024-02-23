#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using XNetEx.Runtime.CompilerServices;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
    internal partial class CustomHashing : NameBasedGuidGenerator
    {
        private readonly Func<HashAlgorithm> HashingFactory;

        private CustomHashing(Func<HashAlgorithm> hashingFactory)
        {
            this.HashingFactory = hashingFactory ??
                throw new ArgumentNullException(nameof(hashingFactory));
        }

        public sealed override GuidVersion Version => GuidVersion.Version8;

        public sealed override bool RequiresInput => true;

        internal static NameBasedGuidGenerator.CustomHashing CreateInstance(HashAlgorithm hashing)
        {
            return new NameBasedGuidGenerator.CustomHashing.Synchronized(hashing);
        }

        internal static NameBasedGuidGenerator.CustomHashing CreateInstance(Func<HashAlgorithm> hashingFactory)
        {
            return new NameBasedGuidGenerator.CustomHashing.Disposable(hashingFactory);
        }

        protected sealed override HashAlgorithm CreateHashing()
        {
            return this.HashingFactory.Invoke() ??
                throw new InvalidOperationException("The hash algorithm factory returns null.");
        }

        private sealed class Disposable : NameBasedGuidGenerator.CustomHashing, IDisposable
        {
            private volatile int DisposeState;

            internal Disposable(Func<HashAlgorithm> hashingFactory)
                : base(hashingFactory)
            {
                this.DisposeState = LatchStates.Initial;
            }

            protected override bool TrackHashing => true;

            protected override void Dispose(bool disposing)
            {
                if (Interlocked.CompareExchange(
                    ref this.DisposeState, LatchStates.Entered,
                    LatchStates.Initial) == LatchStates.Initial)
                {
                    try
                    {
                        if (disposing)
                        {
                            this.DisposeHashings();
                        }
                        this.DisposeState = LatchStates.Exited;
                    }
                    catch (Exception)
                    {
                        this.DisposeState = LatchStates.Failed;
                        throw;
                    }
                }
                base.Dispose(disposing);
            }

            protected override HashAlgorithm GetHashing()
            {
                if (this.DisposeState != LatchStates.Initial)
                {
                    throw new ObjectDisposedException(nameof(NameBasedGuidGenerator));
                }
                return base.GetHashing();
            }

            protected override void ReturnHashing(HashAlgorithm hashing)
            {
                if (this.DisposeState != LatchStates.Initial)
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

            private volatile bool IsDisposed;

            internal Synchronized(HashAlgorithm hashing)
                : base(hashing.Identity)
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

                this.GlobalHashing = hashing;
                this.LocalHashing.Dispose();
                this.IsDisposed = false;
            }

            protected override void Dispose(bool disposing)
            {
                if (!this.IsDisposed)
                {
                    if (disposing)
                    {
                        lock (this.GlobalHashing)
                        {
                            this.GlobalHashing.Dispose();
                        }
                    }
                    this.IsDisposed = true;
                }
                base.Dispose(disposing);
            }

            protected override HashAlgorithm GetHashing()
            {
                if (this.IsDisposed)
                {
                    throw new ObjectDisposedException(nameof(NameBasedGuidGenerator));
                }
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
