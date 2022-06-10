using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XstarS.GuidGenerators
{
    internal abstract class TimestampProvider
    {
        internal static class Singleton
        {
            internal static readonly TimestampProvider Value =
                Stopwatch.IsHighResolution ?
                new TimestampProvider.PerfCounter() :
                new TimestampProvider.IncTimestamp();
        }

        private TimestampProvider() { }

        internal static TimestampProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => TimestampProvider.Singleton.Value;
        }

        public abstract long GetCurrentTimestamp();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long GetGuidTimestamp(long ticks)
        {
            return ticks - GuidExtensions.BaseTimestamp.Ticks;
        }

        private sealed class PerfCounter : TimestampProvider
        {
            private long StartTimestamp;

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

            public override long GetCurrentTimestamp()
            {
                return this.StartTimestamp + this.HiResTimer.ElapsedTicks;
            }

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

            private volatile int TicksOffset;

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
}
