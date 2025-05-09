﻿#if !UUIDREV_DISABLE
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class TimeBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version6_GetGuidWithVersion6()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
        var guidP = GuidGenerator.Version6P.NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guidP.GetVersion());
        var guidR = GuidGenerator.Version6R.NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guidR.GetVersion());
        var guidV = GuidGenerator.CreateVersion6R().NewGuid();
        Assert.AreEqual(GuidVersion.Version6, guidV.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version6_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version6.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
        var guidP = GuidGenerator.Version6P.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidP.GetVariant());
        var guidR = GuidGenerator.Version6R.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidR.GetVariant());
        var guidV = GuidGenerator.CreateVersion6R().NewGuid();
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
        Assert.IsTrue(ticksDiff < TimeSpan.TicksPerSecond);
    }

    [TestMethod]
    public void NewGuid_Version6R_GetIncClockSeqWhenTimeBackward()
    {
        var guid0 = GuidGenerator.Version6R.NewGuid();
        _ = guid0.TryGetTimestamp(out var timestamp0);
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = GuidGenerator.Version6R.NewGuid();
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
        var guidR = GuidGenerator.Version6R.NewGuid();
        _ = guidR.TryGetNodeId(out var nodeIdR);
        var nodeIdRByte0 = nodeIdR![0];
        Assert.AreEqual(0x01, nodeIdRByte0 & 0x01);
        var guidV = GuidGenerator.CreateVersion6R().NewGuid();
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
    public void NewGuid_Version6_GetDifferentClockSequencesAndNodeIds()
    {
        var guidGen = GuidGenerator.Version6;
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        _ = guid1.TryGetNodeId(out var nodeId1);
        Assert.AreNotEqual(clockSeq0, clockSeq1);
        CollectionAssert.AreNotEqual(nodeId0, nodeId1);
    }

    [TestMethod]
    public void NewGuid_Version6PR_GetGuidsWithSameNodeId()
    {
        var guidGenP = GuidGenerator.Version6P;
        var guidP0 = guidGenP.NewGuid();
        _ = guidP0.TryGetNodeId(out var nodeIdP0);
        var guidP1 = guidGenP.NewGuid();
        _ = guidP1.TryGetNodeId(out var nodeIdP1);
        CollectionAssert.AreEqual(nodeIdP0, nodeIdP1);
        var guidGenR = GuidGenerator.Version6R;
        var guidR0 = guidGenR.NewGuid();
        _ = guidR0.TryGetNodeId(out var nodeIdR0);
        var guidR1 = guidGenR.NewGuid();
        _ = guidR1.TryGetNodeId(out var nodeIdR1);
        CollectionAssert.AreEqual(nodeIdR0, nodeIdR1);
        var guidGenV = GuidGenerator.CreateVersion6R();
        var guidV0 = guidGenV.NewGuid();
        _ = guidV0.TryGetNodeId(out var nodeIdV0);
        var guidV1 = guidGenV.NewGuid();
        _ = guidV1.TryGetNodeId(out var nodeIdV1);
        CollectionAssert.AreEqual(nodeIdV0, nodeIdV1);
    }

    [TestMethod]
    public void NewGuid_CreateVersion6R_GetDifferentNodeIds()
    {
        var guidGen0 = GuidGenerator.CreateVersion6R();
        var guid0 = guidGen0.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guidGen1 = GuidGenerator.CreateVersion6R();
        var guid1 = guidGen1.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreNotEqual(nodeId0, nodeId1);
    }

    [TestMethod]
    public void NewGuid_CreatePooledV6R_GetAllVersion6Guids()
    {
        using var guidGen = GuidGenerator.CreatePooled(GuidGenerator.CreateVersion6R);
        Parallel.For(0, 1000, part =>
        {
            var guid = guidGen.NewGuid();
            Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
        });
    }
}
#endif
