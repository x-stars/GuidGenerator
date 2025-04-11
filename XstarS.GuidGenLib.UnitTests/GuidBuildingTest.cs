using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidBuildingTest
{
    [TestMethod]
    public void Version1Guid_BuildFromEmptyGuid_GetInputComponents()
    {
        var timestamp = new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc);
        var clockSeq = (short)0x00b4;
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var guid = Uuid.EmptyOf(GuidVersion.Version1)
            .ReplaceTimestamp(timestamp)
            .ReplaceClockSequence(clockSeq)
            .ReplaceNodeId(nodeId);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

    [TestMethod]
    public void Version2Guid_BuildFromEmptyGuid_GetInputComponents()
    {
        var timestamp = new DateTime(0x08BEFFD0E4344000, DateTimeKind.Utc);
        var clockSeq = (short)0x34;
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var domain = DceSecurityDomain.Org;
        var localId = 0x6ba7b810;
        var guid = Uuid.EmptyOf(GuidVersion.Version2)
            .ReplaceTimestamp(timestamp)
            .ReplaceClockSequence(clockSeq)
            .ReplaceNodeId(nodeId)
            .ReplaceDomainAndLocalId(domain, localId);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
        _ = guid.TryGetDomainAndLocalId(out var guidDomain, out var guidLocalId);
        Assert.AreEqual(domain, guidDomain);
        Assert.AreEqual(localId, guidLocalId);
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void Version6Guid_BuildFromEmptyGuid_GetInputComponents()
    {
        var timestamp = new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc);
        var clockSeq = (short)0x00b4;
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var guid = Uuid.EmptyOf(GuidVersion.Version6)
            .ReplaceTimestamp(timestamp)
            .ReplaceClockSequence(clockSeq)
            .ReplaceNodeId(nodeId);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

    [TestMethod]
    public void Version7Guid_BuildFromNewGuid_GetInputComponents()
    {
        var timestamp = new DateTime(0x08D9F638A666EB00, DateTimeKind.Utc);
        var guid = Guid.NewGuid()
            .ReplaceVersion(GuidVersion.Version7)
            .ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
    }
#endif
}
