using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class CustomGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8Example_GetExpectedTimestamp()
    {
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        var guid = GuidGenerator.Version8.NewGuid();
        var (tsSec, tsSubsec, _, _, _, _, _, _, _, _, _) = guid;
        var epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var tsSubsecNs = ((long)tsSubsec * 1_000_000_000) >> 16;
        var tsTicks = (tsSubsecNs / 100) + (tsSec * ticksPerSec);
        var nowTicks = DateTime.UtcNow.Ticks - epoch.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        Console.WriteLine(TimeSpan.FromTicks(ticksDiff));
        Assert.IsTrue(ticksDiff < (ticksPerSec * 2));
    }

    [TestMethod]
    public void NewGuid_Version8Example_GetDifferentRandomBytes()
    {
        static byte[] GetGuidRandomBytes(Guid guid)
        {
            var (_, _, random01, random2, random3,
                random4, random5, random6, random7, _, _
                ) = guid;
            var random0 = (byte)((uint)random01 >> (0 * 8));
            var random1 = (byte)((uint)random01 >> (1 * 8));
            return new[]
            {
                random0, random1, random2, random3,
                random4, random5, random6, random7,
            };
        }

        var guid0 = GuidGenerator.Version8.NewGuid();
        var randomBytes0 = GetGuidRandomBytes(guid0);
        var guid1 = GuidGenerator.Version8.NewGuid();
        var randomBytes1 = GetGuidRandomBytes(guid1);
        CollectionAssert.AreNotEqual(randomBytes0, randomBytes1);
    }

    [TestMethod]
    public void NewGuid_Version8Example_GetSameNodeIdByte()
    {
        var guid0 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, _, _, _, _, _, _, nodeId0, _) = guid0;
        var guid1 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, _, _, _, _, _, _, nodeId1, _) = guid1;
        Assert.AreEqual(nodeId0, nodeId1);
    }

    [TestMethod]
    public void NewGuid_Version8Example_GetIncrementSequence()
    {
        var guid0 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, _, _, _, _, _, _, _, sequence0) = guid0;
        var guid1 = GuidGenerator.Version8.NewGuid();
        var (_, _, _, _, _, _, _, _, _, _, sequence1) = guid1;
        Assert.AreEqual((byte)(sequence0 + 1), sequence1);
    }
}
