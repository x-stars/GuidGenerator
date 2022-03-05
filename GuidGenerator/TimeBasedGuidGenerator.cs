using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace XstarS.GuidGenerators
{
    internal sealed class TimeBasedGuidGenerator : GuidGenerator
    {
        private static byte[] LocalMacAddressBytes =
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
            var currentInterface = default(NetworkInterface);
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.OperationalStatus == OperationalStatus.Up)
                {
                    currentInterface = @interface;
                    break;
                }
            }
            if (currentInterface is null) { return PhysicalAddress.None; }
            return currentInterface.GetPhysicalAddress();
        }

        public override Guid NewGuid()
        {
            var clockSeq = Interlocked.Increment(ref this.ClockSequence);
            var macAddress = TimeBasedGuidGenerator.LocalMacAddressBytes;
            throw new NotImplementedException();
        }
    }
}
