#if !FEATURE_DISABLE_UUIDREV
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version6_GetGuidWithVersion6()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
        var guidP = GuidGenerator.Version6P.NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guidP.GetVersion());
        var guidV = GuidGenerator.CreateVersion6().NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guidV.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version6_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
        var guidP = GuidGenerator.Version6P.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidP.GetVariant());
        var guidV = GuidGenerator.CreateVersion6().NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidV.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version6_GetExpectedTimestamp()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        _ = guid.TryGetTimestamp(out var timestamp);
        var tsTicks = timestamp.Ticks;
        var nowTicks = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticksPerSec);
    }

    [TestMethod]
    public void NewGuid_Version6_GetIncClockSeqWhenTimeBackward()
    {
        var guid0 = GuidGenerator.Version6.NewGuid();
        _ = guid0.TryGetTimestamp(out var timestamp0);
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = GuidGenerator.Version6.NewGuid();
        _ = guid1.TryGetTimestamp(out var timestamp1);
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        if (timestamp1.Ticks < timestamp0.Ticks)
        {
            var expected = (short)((clockSeq0 + 1) & ~0xC000);
            Assert.AreEqual(expected, clockSeq1);
        }
    }

    [TestMethod]
    public void NewGuid_Version6_GetNodeIdWithOddFirstByte()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        var nodeIdByte0 = nodeId![0];
        Assert.AreEqual(0x01, nodeIdByte0 & 0x01);
        var guidV = GuidGenerator.CreateVersion6().NewGuid();
        _ = guidV.TryGetNodeId(out var nodeIdV);
        var nodeIdVByte0 = nodeIdV![0];
        Assert.AreEqual(0x01, nodeIdVByte0 & 0x01);
    }

    [TestMethod]
    public void NewGuid_Version6P_GetNodeIdWithEvenFirstByte()
    {
        var guid = GuidGenerator.Version6P.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        var nodeIdByte0 = nodeId![0];
        Assert.AreEqual(0x00, nodeIdByte0 & 0x01);
    }

    [TestMethod]
    public void NewGuid_Version6_GetGuidsWithSameNodeId()
    {
        var guidGen = GuidGenerator.Version6;
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreEqual(nodeId0, nodeId1);
        var guidGenV = GuidGenerator.CreateVersion6();
        var guidV0 = guidGenV.NewGuid();
        _ = guidV0.TryGetNodeId(out var nodeIdV0);
        var guidV1 = guidGenV.NewGuid();
        _ = guidV1.TryGetNodeId(out var nodeIdV1);
        CollectionAssert.AreEqual(nodeIdV0, nodeIdV1);
    }

    [TestMethod]
    public void NewGuid_CreateVersion6_GetDifferentNodeIds()
    {
        var guidGen0 = GuidGenerator.CreateVersion6();
        var guid0 = guidGen0.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guidGen1 = GuidGenerator.CreateVersion6();
        var guid1 = guidGen1.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreNotEqual(nodeId0, nodeId1);
    }
}
#endif
