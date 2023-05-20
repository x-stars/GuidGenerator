using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !FEATURE_DISABLE_UUIDREV
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace XNetEx.Guids.Generators;

[TestClass]
public partial class TimeBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version1_GetGuidWithVersion1()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
        var guidR = GuidGenerator.Version1R.NewGuid();
        Assert.AreEqual(GuidVersion.Version1, guidR.GetVersion());
        var guidVR = GuidGenerator.CreateVersion1R().NewGuid();
        Assert.AreEqual(GuidVersion.Version1, guidVR.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version1_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
        var guidR = GuidGenerator.Version1R.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidR.GetVariant());
        var guidVR = GuidGenerator.CreateVersion1R().NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guidVR.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version1_GetExpectedTimestamp()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        _ = guid.TryGetTimestamp(out var timestamp);
        var tsTicks = timestamp.Ticks;
        var nowTicks = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticksPerSec);
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
    public void NewGuid_Version1_GetNodeIdWithEventFirstByte()
    {
        var guid = GuidGenerator.Version1.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        var nodeIdByte0 = nodeId![0];
        Assert.AreEqual(0x00, nodeIdByte0 & 0x01);
    }

    [TestMethod]
    public void NewGuid_Version1R_GetNodeIdWithOddFirstByte()
    {
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        var nodeIdByte0 = nodeId![0];
        Assert.AreEqual(0x01, nodeIdByte0 & 0x01);
        var guidV = GuidGenerator.CreateVersion1R().NewGuid();
        _ = guidV.TryGetNodeId(out var nodeIdV);
        var nodeIdVByte0 = nodeIdV![0];
        Assert.AreEqual(0x01, nodeIdVByte0 & 0x01);
    }

    [TestMethod]
    public void NewGuid_Version1R_GetGuidsWithSameNodeId()
    {
        var guidGen = GuidGenerator.Version1R;
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreEqual(nodeId0, nodeId1);
        var guidGenV = GuidGenerator.CreateVersion1R();
        var guidV0 = guidGenV.NewGuid();
        _ = guidV0.TryGetNodeId(out var nodeIdV0);
        var guidV1 = guidGenV.NewGuid();
        _ = guidV1.TryGetNodeId(out var nodeIdV1);
        CollectionAssert.AreEqual(nodeIdV0, nodeIdV1);
    }

    [TestMethod]
    public void NewGuid_CreateVersion1R_GetDifferentNodeIds()
    {
        var guidGen0 = GuidGenerator.CreateVersion1R();
        var guid0 = guidGen0.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guidGen1 = GuidGenerator.CreateVersion1R();
        var guid1 = guidGen1.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreNotEqual(nodeId0, nodeId1);
    }

#if !FEATURE_DISABLE_UUIDREV
    [TestMethod]
    public void NewGuid_CreatePooledV1R_GetAllNonEmptyGuids()
    {
        using var guidGen = GuidGenerator.CreatePooled(GuidGenerator.CreateVersion1R);
        var countBox = new StrongBox<int>(0);
        Parallel.For(0, 1000, part =>
        {
            for (int index = 0; index < 1000; index++)
            {
                var guid = guidGen.NewGuid();
                if (guid != Guid.Empty)
                {
                    Interlocked.Increment(ref countBox.Value);
                }
            }
        });
        Assert.AreEqual(1000 * 1000, countBox.Value);
    }
#endif
}
