using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version1_GetExpectedTimestamp()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        var hasTs = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(hasTs);
        var ticksTs = timestamp.Ticks;
        var ticksNow = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(ticksNow - ticksTs);
        var ticks1s = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticks1s);
    }

    [TestMethod]
    public void NewGuid_Version1_GetGuidWithVersion1()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version1_GetIncClockSeqWhenTimeBackward()
    {
        var guid0 = GuidGenerator.Version1.NewGuid();
        _ = guid0.TryGetTimestamp(out var timestamp0);
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = GuidGenerator.Version1.NewGuid();
        _ = guid1.TryGetTimestamp(out var timestamp1);
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        if (timestamp1.Ticks < timestamp0.Ticks)
        {
            var expected = (short)((clockSeq0 + 1) & ~0xC000);
            Assert.AreEqual(expected, clockSeq1);
        }
    }

    [TestMethod]
    public void NewGuid_Version1_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version1R_GetNodeIdWithOddFirstByte()
    {
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        var nodeIdByte0 = nodeId![0];
        Assert.AreEqual(0x01, nodeIdByte0 & 0x01);
    }
}
