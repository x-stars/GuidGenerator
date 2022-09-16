using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class RefreshingCache<T>
{
    private readonly Func<T> RefreshFunc;

    private readonly int RefreshPeriod;

    private readonly int BeforeSleep;

    private readonly Timer RefreshTask;

    private volatile int SleepCountdown;

    private volatile StrongBox<T>? CachedValueBox;

    public RefreshingCache(Func<T> refreshFunc, int refreshPeriod, int beforeSleep)
    {
        Debug.Assert(beforeSleep >= 0);
        this.RefreshFunc = refreshFunc;
        this.RefreshPeriod = refreshPeriod;
        this.BeforeSleep = beforeSleep;
        this.RefreshTask = new Timer(this.RefreshOrSleep);
        this.SleepCountdown = beforeSleep;
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
        this.SleepCountdown = this.BeforeSleep;
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
