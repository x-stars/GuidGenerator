using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceTimestamp_Version1Guid_GetInputTimestamp()
    {
        var timestamp = new DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc);
        var original = Guid.Parse("00000000-0000-1000-80b4-00c04fd430c8");
        var guid = original.ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
    }

    [TestMethod]
    public void ReplaceTimestamp_Version2Guid_GetInputTimestamp()
    {
        var timestamp = new DateTime(0x08BEFFD0E4344000, DateTimeKind.Utc);
        var original = Guid.Parse("6ba7b810-0000-2000-b402-00c04fd430c8");
        var guid = original.ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
    }

    [TestMethod]
    public void ReplaceTimestamp_Version6Guid_GetInputTimestamp()
    {
        var timestamp = new DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc);
        var original = Guid.Parse("00000000-0000-6000-80b4-00c04fd430c8");
        var guid = original.ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
    }

    [TestMethod]
    public void ReplaceTimestamp_Version1Or6DateTimeMinMaxValue_GetOverflowTimestamp()
    {
        var timestamp0 = DateTime.MinValue.ToUniversalTime();
        var original0 = Guid.Parse("00000000-0000-1000-80b4-00c04fd430c8");
        var guid0 = original0.ReplaceTimestamp(timestamp0);
        _ = guid0.TryGetTimestamp(out var guidTimestamp0);
        Assert.AreNotEqual(timestamp0, guidTimestamp0);
        var timestamp1 = DateTime.MaxValue.ToUniversalTime();
        var original1 = Guid.Parse("00000000-0000-6000-80b4-00c04fd430c8");
        var guid1 = original1.ReplaceTimestamp(timestamp1);
        _ = guid1.TryGetTimestamp(out var guidTimestamp1);
        Assert.AreNotEqual(timestamp1, guidTimestamp1);
    }

    [TestMethod]
    public void ReplaceTimestamp_Version7Guid_GetInputTimestamp()
    {
        var timestamp = new DateTime(0x08D9F638A666EB00, DateTimeKind.Utc);
        var original = Guid.Parse("00000000-0000-7cc3-98c4-dc0c0c07398f");
        var guid = original.ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreEqual(timestamp, guidTimestamp);
    }

    [TestMethod]
    public void ReplaceTimestamp_Version7DateTimeMinValue_GetOverflowTimestamp()
    {
        var timestamp = DateTime.MinValue.ToUniversalTime();
        var original = Guid.Parse("00000000-0000-7cc3-98c4-dc0c0c07398f");
        var guid = original.ReplaceTimestamp(timestamp);
        _ = guid.TryGetTimestamp(out var guidTimestamp);
        Assert.AreNotEqual(timestamp, guidTimestamp);
    }

    [TestMethod]
    public void ReplaceTimestamp_OtherVersionGuids_GetOriginalGuidValues()
    {
        var timestamp = new DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc);
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceTimestamp(timestamp);
            Assert.AreEqual(original, guid);
        }
    }

    [TestMethod]
    public void ReplaceClockSequence_Version1Guid_GetInputClockSequence()
    {
        var clockSeq = (short)0x00b4;
        var original = Guid.Parse("6ba7b810-9dad-11d1-8000-00c04fd430c8");
        var guid = original.ReplaceClockSequence(clockSeq);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
    }

    [TestMethod]
    public void ReplaceClockSequence_Version2Guid_GetInputClockSequence()
    {
        var clockSeq = (short)0x34;
        var original = Guid.Parse("6ba7b810-9dad-21d1-8002-00c04fd430c8");
        var guid = original.ReplaceClockSequence(clockSeq);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
    }

    [TestMethod]
    public void ReplaceClockSequence_Version6Guid_GetInputClockSequence()
    {
        var clockSeq = (short)0x00b4;
        var original = Guid.Parse("1d19dad6-ba7b-6810-8000-00c04fd430c8");
        var guid = original.ReplaceClockSequence(clockSeq);
        _ = guid.TryGetClockSequence(out var guidClockSeq);
        Assert.AreEqual(clockSeq, guidClockSeq);
    }

    [TestMethod]
    public void ReplaceClockSequence_Int16MinMaxValue_GetOverflowClockSequence()
    {
        var clockSeq0 = short.MinValue;
        var original0 = Guid.Parse("1d19dad6-ba7b-6810-8000-00c04fd430c8");
        var guid0 = original0.ReplaceClockSequence(clockSeq0);
        _ = guid0.TryGetClockSequence(out var guidClockSeq0);
        Assert.AreNotEqual(clockSeq0, guidClockSeq0);
        var clockSeq1 = short.MaxValue;
        var original1 = Guid.Parse("1d19dad6-ba7b-6810-8000-00c04fd430c8");
        var guid1 = original1.ReplaceClockSequence(clockSeq1);
        _ = guid1.TryGetClockSequence(out var guidClockSeq1);
        Assert.AreNotEqual(clockSeq1, guidClockSeq1);
    }

    [TestMethod]
    public void ReplaceClockSequence_OtherVersionGuids_GetOriginalGuidValues()
    {
        var clockSeq = (short)0x00b4;
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceClockSequence(clockSeq);
            Assert.AreEqual(original, guid);
        }
    }
}
