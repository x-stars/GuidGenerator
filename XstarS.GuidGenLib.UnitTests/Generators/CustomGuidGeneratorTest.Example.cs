#if !FEATURE_DISABLE_UUIDREV
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class CustomGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8Example_GetExpectedTimestamp()
    {
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var guid = GuidGenerator.Version8.NewGuid();
        var (tsNsHi, tsNsLow, _, _, _, _, _, _, _, _, _) = guid;
        var tsNanoSec = ((long)(uint)tsNsHi << (2 * 8)) | (ushort)tsNsLow;
        var nowTicks = DateTime.UtcNow.Ticks - epoch.Ticks;
        var nowNanoSec = (nowTicks * 100) & ~(-1L << (6 * 8));
        var ticksDiff = Math.Abs((tsNanoSec - nowNanoSec) / 100);
        Assert.IsTrue(ticksDiff < ticksPerSec);
    }

    [TestMethod]
    public void NewGuid_Version8Example_GetDifferentRandomBytes()
    {
        static byte[] GetGuidRandomBytes(Guid guid)
        {
            var (_, _, random01, random2, random3,
                random4, random5, random6, random7,
                random8, random9) = guid;
            var random0 = (byte)((uint)random01 >> (0 * 8));
            var random1 = (byte)((uint)random01 >> (1 * 8));
            return new[]
            {
                random0, random1, random2, random3,
                random4, random5, random6, random7,
                random8, random9,
            };
        }

        var guid0 = GuidGenerator.Version8.NewGuid();
        var randomBytes0 = GetGuidRandomBytes(guid0);
        var guid1 = GuidGenerator.Version8.NewGuid();
        var randomBytes1 = GetGuidRandomBytes(guid1);
        CollectionAssert.AreNotEqual(randomBytes0, randomBytes1);
    }
}
#endif
