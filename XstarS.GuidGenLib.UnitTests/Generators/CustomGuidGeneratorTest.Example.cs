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
            var random = new byte[10];
            var (_, _, random01, lower8) = guid;
            random[0] = (byte)((uint)random01 >> (0 * 8));
            random[1] = (byte)((uint)random01 >> (1 * 8));
            Buffer.BlockCopy(lower8, 0, random, 2, 8);
            return random;
        }

        var guid0 = GuidGenerator.Version8.NewGuid();
        var randomBytes0 = GetGuidRandomBytes(guid0);
        var guid1 = GuidGenerator.Version8.NewGuid();
        var randomBytes1 = GetGuidRandomBytes(guid1);
        CollectionAssert.AreNotEqual(randomBytes0, randomBytes1);
    }
}
#endif
