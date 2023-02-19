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
        spinner.SpinOnce();
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
        const double ticksPerSec = 1_000_000_000 / 100;
        var period = ticksPerSec / Stopwatch.Frequency;
        var diff10 = (time1.Ticks - time0.Ticks) / period;
        var diff21 = (time2.Ticks - time1.Ticks) / period;
        var diff32 = (time3.Ticks - time2.Ticks) / period;
        var matches = ((diff10 is > 0 and < 10) ? 1 : 0) +
                      ((diff21 is > 0 and < 10) ? 1 : 0) +
                      ((diff32 is > 0 and < 10) ? 1 : 0);
        return matches >= 2;
    }

    public abstract long CurrentTimestamp { get; }

    private sealed class DirectTime : TimestampProvider
    {
        internal DirectTime() { }

        public override long CurrentTimestamp => DateTime.UtcNow.Ticks;
    }

    private sealed class PerfCounter : TimestampProvider
    {
        private long Volatile_StartTimestamp;

        private readonly Stopwatch HiResTimer;

        private readonly Timer UpdateTimeTask;

        internal PerfCounter()
        {
            const int updateMs = 1 * 1000;
            var startTime = DateTime.UtcNow;
            var hiResTimer = Stopwatch.StartNew();
            this.StartTimestamp = startTime.Ticks;
            this.HiResTimer = hiResTimer;
            this.UpdateTimeTask = new Timer(this.UpdateSystemTime);
            this.UpdateTimeTask.Change(updateMs, updateMs);
        }

        private long StartTimestamp
        {
            get => Volatile.Read(ref this.Volatile_StartTimestamp);
            set => Volatile.Write(ref this.Volatile_StartTimestamp, value);
        }

        public override long CurrentTimestamp =>
            this.StartTimestamp + this.HiResTimer.Elapsed.Ticks;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void UpdateSystemTime(object? unused)
        {
            const long ticksPerSec = 1_000_000_000 / 100;
            var sysTs = DateTime.UtcNow.Ticks;
            var nowTs = this.CurrentTimestamp;
            if (Math.Abs(nowTs - sysTs) >= ticksPerSec)
            {
                var nowTicks = DateTime.UtcNow.Ticks;
                var timerTicks = this.HiResTimer.Elapsed.Ticks;
                var startTs = nowTicks - timerTicks;
                this.StartTimestamp = startTs;
            }
        }
    }

    private sealed class IncTimestamp : TimestampProvider
    {
        private long LastTimeTicks;

        private int TicksOffset;

        internal IncTimestamp() { }

        public override long CurrentTimestamp => this.GetOrIncTimestamp();

        [MethodImpl(MethodImplOptions.Synchronized)]
        private long GetOrIncTimestamp()
        {
            var lastTicks = this.LastTimeTicks;
            var nowTicks = DateTime.UtcNow.Ticks;
            this.TicksOffset = (nowTicks == lastTicks) ?
                (this.TicksOffset + 1) : 0;
            this.LastTimeTicks = nowTicks;
            return nowTicks + this.TicksOffset;
        }
    }
}
