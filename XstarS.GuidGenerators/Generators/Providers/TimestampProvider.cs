using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class TimestampProvider
{
    private static volatile TimestampProvider? Singleton;

    private TimestampProvider() { }

    internal static TimestampProvider Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static TimestampProvider Initialize()
            {
                return TimestampProvider.Singleton ??=
                    TimestampProvider.Create();
            }

            return TimestampProvider.Singleton ?? Initialize();
        }
    }

    private static TimestampProvider Create()
    {
        return TimestampProvider.IsDateTimeHiRes() ?
            new TimestampProvider.DirectTime() :
            Stopwatch.IsHighResolution ?
            new TimestampProvider.PerfCounter() :
            new TimestampProvider.IncTimestamp();
    }

    private static bool IsDateTimeHiRes()
    {
        var spinner = new SpinWait();
        _ = DateTime.UtcNow;
        _ = DateTime.UtcNow;
        spinner.SpinOnce();
        var time0 = DateTime.UtcNow;
        spinner.SpinOnce();
        var time1 = DateTime.UtcNow;
        spinner.SpinOnce();
        var time2 = DateTime.UtcNow;
        spinner.SpinOnce();
        var time3 = DateTime.UtcNow;
        if (!Stopwatch.IsHighResolution) { return false; }
        var period = Stopwatch.Frequency / 10000000.0;
        var diff10 = (time1.Ticks - time0.Ticks) / period;
        var diff21 = (time2.Ticks - time1.Ticks) / period;
        var diff32 = (time3.Ticks - time2.Ticks) / period;
        var matches = ((diff10 is > 0 and < 10) ? 1 : 0) +
                      ((diff21 is > 0 and < 10) ? 1 : 0) +
                      ((diff32 is > 0 and < 10) ? 1 : 0);
        return matches >= 2;
    }

    public abstract long GetCurrentTimestamp();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected long GetGuidTimestamp(long ticks)
    {
        return ticks - GuidExtensions.TimeBasedEpoch.Ticks;
    }

    private sealed class DirectTime : TimestampProvider
    {
        internal DirectTime() { }

        public override long GetCurrentTimestamp()
        {
            return this.GetGuidTimestamp(DateTime.UtcNow.Ticks);
        }
    }

    private sealed class PerfCounter : TimestampProvider
    {
        private long Volatile_StartTimestamp;

        private readonly Stopwatch HiResTimer;

        private readonly Timer UpdateTimeTask;

        internal PerfCounter()
        {
            const int updateMs = 1 * 1000;
            var nowTime = DateTime.UtcNow;
            var hiResTimer = Stopwatch.StartNew();
            var timestamp = this.GetGuidTimestamp(nowTime.Ticks);
            this.StartTimestamp = timestamp;
            this.HiResTimer = hiResTimer;
            this.UpdateTimeTask = new Timer(this.UpdateSystemTime);
            this.UpdateTimeTask.Change(updateMs, updateMs);
        }

        private long StartTimestamp
        {
            get => Volatile.Read(ref this.Volatile_StartTimestamp);
            set => Volatile.Write(ref this.Volatile_StartTimestamp, value);
        }

        public override long GetCurrentTimestamp()
        {
            return this.StartTimestamp + this.HiResTimer.ElapsedTicks;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void UpdateSystemTime(object? unused)
        {
            const long secTicks = 10 * 1000 * 1000;
            var sysTicks = DateTime.UtcNow.Ticks;
            var sysTs = this.GetGuidTimestamp(sysTicks);
            var nowTs = this.GetCurrentTimestamp();
            if (Math.Abs(nowTs - sysTs) >= secTicks)
            {
                var nowTicks = DateTime.UtcNow.Ticks;
                var timerTicks = this.HiResTimer.ElapsedTicks;
                var startTicks = nowTicks - timerTicks;
                var startTs = this.GetGuidTimestamp(startTicks);
                this.StartTimestamp = startTs;
            }
        }
    }

    private sealed class IncTimestamp : TimestampProvider
    {
        private long LastTimeTicks;

        private int TicksOffset;

        internal IncTimestamp() { }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override long GetCurrentTimestamp()
        {
            var lastTicks = this.LastTimeTicks;
            var nowTicks = DateTime.UtcNow.Ticks;
            this.TicksOffset = (nowTicks == lastTicks) ?
                (this.TicksOffset + 1) : 0;
            this.LastTimeTicks = nowTicks;
            var incTicks = nowTicks + this.TicksOffset;
            return this.GetGuidTimestamp(incTicks);
        }
    }
}
