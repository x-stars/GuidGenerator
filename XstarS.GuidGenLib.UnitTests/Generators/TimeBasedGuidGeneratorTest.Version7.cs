#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class TimeBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version7_GetGuidWithVersion7()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        Assert.AreEqual(GuidVersion.Version7, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version7_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version7_GetExpectedTimestamp()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        var hasTs = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(hasTs);
        var tsTicks = timestamp.Ticks;
        var nowTicks = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticksPerSec);
    }

    [TestMethod]
    public void NewGuid_Version7_GetDifferentLower8Bytes()
    {
        static byte[] GetGuidLower8Bytes(Guid guid)
        {
            var (_, _, _,
                lower0, lower1, lower2, lower3,
                lower4, lower5, lower6, lower7
                ) = guid;
            return new[]
            {
                lower0, lower1, lower2, lower3,
                lower4, lower5, lower6, lower7,
            };
        }

        var guid0 = GuidGenerator.Version7.NewGuid();
        var guid0Lower = GetGuidLower8Bytes(guid0);
        var guid1 = GuidGenerator.Version7.NewGuid();
        var guid1Lower = GetGuidLower8Bytes(guid1);
        CollectionAssert.AreNotEqual(guid0Lower, guid1Lower);
    }

    [TestMethod]
    public void NewGuid_Version7_GetMonotonicGuids()
    {
        var lastGuid = Guid.Empty;
        var guidGen = GuidGenerator.Version7;
        for (int index = 0; index < 1000; index++)
        {
            var guid = guidGen.NewGuid();
            Assert.IsTrue(guid.CompareTo(lastGuid) > 0);
            lastGuid = guid;
        }
    }

    [TestMethod]
    public void NewGuid_Version7M_ConcurrentGetMonotonicGuids()
    {
        var lastGuid = Guid.Empty;
        var guidGen = GuidGenerator.Version7M;
        Parallel.For(0, 1000, index =>
        {
            lock (guidGen)
            {
                var guid = guidGen.NewGuid();
                Assert.IsTrue(guid.CompareTo(lastGuid) > 0);
                lastGuid = guid;
            }
        });
    }
}
#endif
