using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Threading;

internal sealed class AutoRefreshCache<T> : IDisposable
{
    private readonly Func<T> RefreshFunc;

    private readonly int RefreshPeriod;

    private readonly int SleepAfter;

    private readonly Timer RefreshTask;

    private volatile StrongBox<T>? CachedValueBox;

    private volatile int SleepCountdown;

    private volatile bool IsDisposed;

    public AutoRefreshCache(Func<T> refreshFunc, int refreshPeriod, int sleepAfter)
    {
        this.RefreshFunc = refreshFunc;
        this.RefreshPeriod = refreshPeriod;
        this.SleepAfter = sleepAfter;
        this.RefreshTask = new Timer(this.RefreshOrSleep);
        this.CachedValueBox = null;
        this.SleepCountdown = sleepAfter;
        this.IsDisposed = false;
    }

    public T Value => this.GetOrRefreshValue();

    public void Dispose()
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        void DisposeCore()
        {
            if (!this.IsDisposed)
            {
                this.RefreshTask.Dispose();
                this.CachedValueBox = null;
                this.IsDisposed = true;
            }
        }

        if (!this.IsDisposed) { DisposeCore(); }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T GetOrRefreshValue()
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        StrongBox<T> ForcedRefresh()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(AutoRefreshCache<T>));
            }
            if (this.CachedValueBox is null)
            {
                this.CachedValueBox = new StrongBox<T>(this.RefreshFunc.Invoke());
                this.RefreshTask.Change(this.RefreshPeriod, this.RefreshPeriod);
            }
            return this.CachedValueBox;
        }

        var valueBox = this.CachedValueBox ?? ForcedRefresh();
        this.SleepCountdown = this.SleepAfter;
        return valueBox.Value!;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void RefreshOrSleep(object? unused)
    {
        if (this.SleepCountdown > 0)
        {
            this.CachedValueBox = new StrongBox<T>(this.RefreshFunc.Invoke());
            this.SleepCountdown -= 1;
        }
        else
        {
            this.CachedValueBox = null;
            this.RefreshTask.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
