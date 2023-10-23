#if !UUIDREV_DISABLE
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class CustomGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8Example_GetExpectedTimestamp()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var guid = GuidGenerator.Version8.NewGuid();
        var (ts10NsHi, ts10NsMid, ts10NsLow, _) = guid;
        var ts10NanoSec = (long)(
            ((ulong)(uint)ts10NsHi << (4 * 8 - 4)) |
            ((ulong)(ushort)ts10NsMid << (2 * 8 - 4)) |
            ((ulong)(ushort)ts10NsLow & ~0xF000UL));
        var nowTicks = DateTime.UtcNow.Ticks - epoch.Ticks;
        var now10NanoSec = (nowTicks * 10) & ~(-1L << (8 * 8 - 4));
        var ticksDiff = Math.Abs((ts10NanoSec - now10NanoSec) / 10);
        Assert.IsTrue(ticksDiff < TimeSpan.TicksPerSecond);
    }

    [TestMethod]
    public void NewGuid_Version8Example_GetDifferentRandomBytes()
    {
        var guid0 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, randomBytes0) = guid0;
        var guid1 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, randomBytes1) = guid1;
        CollectionAssert.AreNotEqual(randomBytes0, randomBytes1);
    }
}
#endif
