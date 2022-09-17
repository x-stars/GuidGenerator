using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class AutoRefreshCache<T>
{
    private readonly Func<T> RefreshFunc;

    private readonly int RefreshPeriod;

    private readonly int SleepAfter;

    private readonly Timer RefreshTask;

    private volatile int SleepCountdown;

    private volatile StrongBox<T>? CachedValueBox;

    public AutoRefreshCache(Func<T> refreshFunc, int refreshPeriod, int sleepAfter)
    {
        Debug.Assert(sleepAfter >= 0);
        this.RefreshFunc = refreshFunc;
        this.RefreshPeriod = refreshPeriod;
        this.SleepAfter = sleepAfter;
        this.RefreshTask = new Timer(this.RefreshOrSleep);
        this.SleepCountdown = sleepAfter;
        this.CachedValueBox = null;
    }

    public T Value => this.GetOrRefreshValue();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T GetOrRefreshValue()
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        StrongBox<T> ForcedRefresh()
        {
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
