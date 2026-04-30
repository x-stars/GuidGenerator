using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Threading;

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using AsyncScope = ValueTask<SemaphoreSlimLock.Scope>;
#else
using AsyncScope = Task<SemaphoreSlimLock.Scope>;
#endif

/// <summary>
/// Provides extension methods for <see cref="SemaphoreSlim"/> used as locks.
/// </summary>
/// <remarks>For internal usage only. Do not expose it as public APIs.</remarks>
internal static class SemaphoreSlimLock
{
    /// <param name="semaphore">The <see cref="SemaphoreSlim"/> lock.</param>
    extension(SemaphoreSlim semaphore)
    {
        /// <summary>
        /// Creates a new <see cref="SemaphoreSlim"/> instance for locking
        /// which <see cref="SemaphoreSlim.CurrentCount"/> is 1.
        /// </summary>
        /// <returns>A new <see cref="SemaphoreSlim"/> instance for locking
        /// which <see cref="SemaphoreSlim.CurrentCount"/> is 1.</returns>
        public static SemaphoreSlim CreateLock()
        {
            return new(initialCount: 1, maxCount: 1);
        }

        /// <summary>
        /// Enters the <see cref="SemaphoreSlim"/> lock,
        /// waiting if necessary until the lock can be entered.
        /// </summary>
        /// <returns>A <see cref="Scope"/> that can be disposed
        /// to exit the <see cref="SemaphoreSlim"/> lock.</returns>
        /// <exception cref="ArgumentNullException">
        /// The current instance is <see langword="null"/>.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The current instance has already been disposed.</exception>
        public Scope EnterScope()
        {
            ArgumentNullException.ThrowIfNull(semaphore);
            Debug.Assert(semaphore.CurrentCount <= 1);
            semaphore.Wait();
            return new Scope(semaphore);
        }

        /// <summary>
        /// Enters the <see cref="SemaphoreSlim"/> lock,
        /// asynchronously waiting if necessary until the lock can be entered.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> to observe.</param>
        /// <returns>A task that will complete
        /// when the <see cref="SemaphoreSlim"/> lock has been entered,
        /// containing a <see cref="Scope"/> that can be disposed
        /// to exit the <see cref="SemaphoreSlim"/> lock.</returns>
        /// <exception cref="ArgumentNullException">
        /// The current instance is <see langword="null"/>.</exception>
        /// <exception cref="ObjectDisposedException">
        /// The current instance has already been disposed.</exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was canceled.</exception>
        public async AsyncScope EnterScopeAsync(
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(semaphore);
            Debug.Assert(semaphore.CurrentCount <= 1);
            await semaphore.WaitAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new Scope(semaphore);
        }
    }

    /// <summary>
    /// Represents a <see cref="SemaphoreSlim"/> lock that has been entered.
    /// </summary>
    public readonly struct Scope : IDisposable
    {
        /// <summary>
        /// The <see cref="SemaphoreSlim"/> lock that has been entered.
        /// </summary>
        private readonly SemaphoreSlim Semaphore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> structure
        /// of the specified <see cref="SemaphoreSlim"/> lock that has been entered.
        /// </summary>
        /// <param name="semaphore">
        /// A <see cref="SemaphoreSlim"/> lock that has been entered.</param>
        internal Scope(SemaphoreSlim semaphore)
        {
            Debug.Assert(semaphore.CurrentCount == 0);
            this.Semaphore = semaphore;
        }

        /// <summary>
        /// Exits the <see cref="SemaphoreSlim"/> lock.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The input
        /// <see cref="SemaphoreSlim"/> lock has already been disposed.</exception>
        public void Dispose()
        {
            Debug.Assert(this.Semaphore.CurrentCount == 0);
            this.Semaphore.Release();
        }
    }
}
