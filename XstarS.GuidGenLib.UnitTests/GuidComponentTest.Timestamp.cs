using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentTest
{
    [TestMethod]
    public void TryGetTimestamp_Version1Guid_GetExpectedTimestamp()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

    [TestMethod]
    public void TryGetTimestamp_Version2Guid_GetExpectedTimestamp()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-b402-00c04fd430c8");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08BEFFD0E4344000, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void TryGetTimestamp_Version6Guid_GetExpectedTimestamp()
    {
        var guid = Guid.Parse("1d19dad6-ba7b-6810-80b4-00c04fd430c8");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

    [TestMethod]
    public void TryGetTimestamp_Version7Guid_GetExpectedTimestamp()
    {
        var guid = Guid.Parse("017f22e2-79b0-7cc3-98c4-dc0c0c07398f");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        var expected = new DateTime(0x08D9F638A666EB00, DateTimeKind.Utc);
        Assert.AreEqual(expected, timestamp);
    }

    [TestMethod]
    public void TryGetTimestamp_MaxVersion7Guid_GetOverMaxValueDateTime()
    {
        var guid = Guid.Parse("ffffffff-ffff-7fff-bfff-ffffffffffff");
        var result = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(result);
        Assert.IsTrue(timestamp > DateTime.MaxValue);
    }
#endif

    [TestMethod]
    public void TryGetTimestamp_OtherVersionGuids_GetAllFalseResults()
    {
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetTimestamp(out _);
            Assert.IsFalse(result);
        }
    }

    [TestMethod]
    public void TryGetClockSequence_Version1Guid_GetExpectedClockSequence()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var result = guid.TryGetClockSequence(out var clockSeq);
        Assert.IsTrue(result);
        Assert.AreEqual((short)0x00b4, clockSeq);
    }

    [TestMethod]
    public void TryGetClockSequence_Version2Guid_GetExpectedClockSequence()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-b402-00c04fd430c8");
        var result = guid.TryGetClockSequence(out var clockSeq);
        Assert.IsTrue(result);
        Assert.AreEqual((short)0x34, clockSeq);
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void TryGetClockSequence_Version6Guid_GetExpectedClockSequence()
    {
        var guid = Guid.Parse("1d19dad6-ba7b-6810-80b4-00c04fd430c8");
        var result = guid.TryGetClockSequence(out var clockSeq);
        Assert.IsTrue(result);
        Assert.AreEqual((short)0x00b4, clockSeq);
    }
#endif

    [TestMethod]
    public void TryGetClockSequence_OtherVersionGuids_GetAllFalseResults()
    {
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetClockSequence(out _);
            Assert.IsFalse(result);
        }
    }
}
