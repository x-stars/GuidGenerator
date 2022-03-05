using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace XstarS.GuidGenerators
{
    internal sealed class TimeBasedGuidGenerator : GuidGenerator
    {
        private static readonly DateTime BaseTimestamp =
            new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

        private static readonly byte[] LocalMacAddressBytes =
            TimeBasedGuidGenerator.GetLocalMacAdddress().GetAddressBytes();

        private volatile int ClockSequence;

        private TimeBasedGuidGenerator()
        {
            this.ClockSequence = new Random().Next();
        }

        internal static TimeBasedGuidGenerator Instance { get; } =
            new TimeBasedGuidGenerator();

        public override GuidVersion Version => GuidVersion.Version1;

        private static PhysicalAddress GetLocalMacAdddress()
        {
            var target = default(NetworkInterface);
            var ifaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in ifaces)
            {
                if (iface.OperationalStatus == OperationalStatus.Up)
                {
                    target = iface;
                    break;
                }
            }
            if (target is null) { return PhysicalAddress.None; }
            return target.GetPhysicalAddress();
        }

        public override Guid NewGuid()
        {
            var guidBytes = new byte[16];
            var timestamp = this.GetCurrentTimestamp();
            this.FillTimestampFields(guidBytes, timestamp);
            var version = (int)this.Version << 4;
            guidBytes[7] = (byte)(guidBytes[7] & ~0xF0 | version);
            var clockSeq = Interlocked.Increment(ref this.ClockSequence);
            this.FillClockSequenceField(guidBytes, clockSeq);
            guidBytes[8] = (byte)(guidBytes[8] & ~0xC0 | 0x80);
            var macAddress = TimeBasedGuidGenerator.LocalMacAddressBytes;
            Array.Copy(macAddress, 0, guidBytes, 10, macAddress.Length);
            return new Guid(guidBytes);
        }

        private long GetCurrentTimestamp()
        {
            var begin = TimeBasedGuidGenerator.BaseTimestamp;
            return DateTime.UtcNow.Ticks - begin.Ticks;
        }

        private unsafe void FillTimestampFields(byte[] guidBytes, long timestamp)
        {
            if (BitConverter.IsLittleEndian)
            {
                fixed (byte* pGuidBytes = &guidBytes[0])
                {
                    *(long*)pGuidBytes = timestamp;
                }
            }
            else
            {
                foreach (var index in 0..8)
                {
                    var shifted = timestamp >> (index * 8);
                    guidBytes[index] = (byte)shifted;
                }
            }
        }

        private unsafe void FillClockSequenceField(byte[] guidBytes, int clockSeq)
        {
            if (BitConverter.IsLittleEndian)
            {
                fixed (byte* pGuidBytes = &guidBytes[8])
                {
                    *(int*)pGuidBytes = clockSeq;
                }
            }
            else
            {
                guidBytes[8] = (byte)(clockSeq >> (0 * 8));
                guidBytes[9] = (byte)(clockSeq >> (1 * 8));
            }
        }
    }
}
