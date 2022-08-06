using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidExtensionsTest
{
    [TestMethod]
    public void Deconstruct_GuidByFields_GetInputFields()
    {
        var guid = new Guid(0x00112233, 0x4455, 0x6677,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF);
        var (a, b, c, d, e, f, g, h, i, j, k) = guid;
        Assert.AreEqual(0x00112233, a);
        Assert.AreEqual(0x4455, b); Assert.AreEqual(0x6677, c);
        Assert.AreEqual(0x88, d); Assert.AreEqual(0x99, e);
        Assert.AreEqual(0xAA, f); Assert.AreEqual(0xBB, g);
        Assert.AreEqual(0xCC, h); Assert.AreEqual(0xDD, i);
        Assert.AreEqual(0xEE, j); Assert.AreEqual(0xFF, k);
    }

    [TestMethod]
    public void GetVersion_Version1Guid_GetGuidVersion1()
    {
        var guid = Guid.Parse("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
        Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
    }

    [TestMethod]
    public void GetVersion_Version4Guid_GetGuidVersion4()
    {
        var guid = Guid.Parse("2502f1d5-c2a9-47d3-b6d8-d7670094ace2");
        Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
    }

    [TestMethod]
    public void GetVariant_EmptyGuid_GetGuidNcsVariant()
    {
        var guid = Guid.Empty;
        Assert.AreEqual(GuidVariant.Ncs, guid.GetVariant());
    }

    [TestMethod]
    public void GetVariant_Version5Guid_GetGuidRfc4122Variant()
    {
        var guid = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void TryGetTimestamp_Version1Guid_GetExpectedDateTime()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

    [TestMethod]
    public void TryGetTimestamp_Version2Guid_GetExpectedDateTime()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-8002-00c04fd430c8");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08BEFFD0E4344000, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

    [TestMethod]
    public void TryGetClockSequence_Version1Guid_GetClockSequence()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var result = guid.TryGetClockSequence(out var clockSeq);
        Assert.IsTrue(result);
        Assert.AreEqual((short)0x00b4, clockSeq);
    }

    [TestMethod]
    public void TryGetClockSequence_Version2Guid_GetClockSequence()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-8002-00c04fd430c8");
        var result = guid.TryGetClockSequence(out var clockSeq);
        Assert.IsTrue(result);
        Assert.AreEqual((short)0x00, clockSeq);
    }

    [TestMethod]
    public void TryGetDomainAndLocalId_Version2Guid_GetInputFields()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-8002-00c04fd430c8");
        var result = guid.TryGetDomainAndLocalId(out var domain, out var localId);
        Assert.IsTrue(result);
        Assert.AreEqual(DceSecurityDomain.Org, domain);
        Assert.AreEqual(0x6ba7b810, localId);
    }

    [TestMethod]
    public void TryGetNodeId_Version1Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-8002-00c04fd430c8");
        var result = guid.TryGetNodeId(out var nodeId);
        Assert.IsTrue(result);
        var expected = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        CollectionAssert.AreEqual(expected, nodeId);
    }

    [TestMethod]
    public void TryGetAllFields_Version4Guid_GetAllFalseResults()
    {
        var guid = Guid.Parse("2502f1d5-c2a9-47d3-b6d8-d7670094ace2");
        Assert.IsFalse(guid.TryGetTimestamp(out _));
        Assert.IsFalse(guid.TryGetClockSequence(out _));
        Assert.IsFalse(guid.TryGetDomainAndLocalId(out _, out _));
        Assert.IsFalse(guid.TryGetNodeId(out _));
    }

    [TestMethod]
    public void FromUuidByteArray_ToByteArray_GetReversedByteOrderFields()
    {
        var uuidBytes = new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var guid = GuidExtensions.FromUuidByteArray(uuidBytes);
        var guidBytes = new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        CollectionAssert.AreEqual(guidBytes, guid.ToByteArray());
    }

    [TestMethod]
    public void ToUuidByteArray_NewByByteArray_GetReversedByteOrderFields()
    {
        var guidBytes = new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var guid = new Guid(guidBytes);
        var uuidBytes = new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        CollectionAssert.AreEqual(uuidBytes, guid.ToUuidByteArray());
    }
}
