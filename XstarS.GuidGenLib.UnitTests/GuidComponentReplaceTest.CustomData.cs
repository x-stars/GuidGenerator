#if !UUIDREV_DISABLE
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceCustomData_Version8Guid_GetInputBytes()
    {
        var customData = new byte[]
        {
            0x05, 0xdb, 0x6c, 0x94, 0xbb, 0xa6, 0x07, 0x02,
            0x08, 0xaa, 0x54, 0x8f, 0x4d, 0x6c, 0xd7, 0x00,
        };
        var original = Guid.Parse("00000000-0000-8000-8000-000000000000");
        var guid = original.ReplaceCustomData(customData);
        _ = guid.TryGetCustomData(out var guidCustomData, out _);
        CollectionAssert.AreEqual(customData, guidCustomData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteCustomData_Version8Guid_GetInputBytes()
    {
        var customData = (stackalloc byte[]
        {
            0x05, 0xdb, 0x6c, 0x94, 0xbb, 0xa6, 0x07, 0x02,
            0x08, 0xaa, 0x54, 0x8f, 0x4d, 0x6c, 0xd7, 0x00,
        });
        var original = Guid.Parse("00000000-0000-8000-8000-000000000000");
        var guid = original.ReplaceCustomData(customData);
        var guidCustomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        _ = guid.TryWriteCustomData(guidCustomData, bitmask);
        Assert.IsTrue(customData.SequenceEqual(guidCustomData));
    }
#endif

    [TestMethod]
    public void ReplaceCustomData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var customData = new byte[16];
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceCustomData(customData);
            Assert.AreEqual(original, guid);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteCustomData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var customData = (stackalloc byte[16]);
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceCustomData(customData);
            Assert.AreEqual(original, guid);
        }
    }
#endif
}
#endif
