#if !UUIDREV_DISABLE
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentTest
{
    [TestMethod]
    public void TryGetCustomData_Version8Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("05db6c94-bba6-8702-88aa-548f4d6cd700");
        var result = guid.TryGetCustomData(out var customData, out var bitmask);
        Assert.IsTrue(result);
        var expectedData = new byte[]
        {
            0x05, 0xdb, 0x6c, 0x94, 0xbb, 0xa6, 0x07, 0x02,
            0x08, 0xaa, 0x54, 0x8f, 0x4d, 0x6c, 0xd7, 0x00,
        };
        CollectionAssert.AreEqual(expectedData, customData);
        var expectedMask = new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        };
        CollectionAssert.AreEqual(expectedMask, bitmask);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteCustomData_Version8Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("05db6c94-bba6-8702-88aa-548f4d6cd700");
        var customData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        var result = guid.TryWriteCustomData(customData, bitmask);
        Assert.IsTrue(result);
        var expectedData = (stackalloc byte[]
        {
            0x05, 0xdb, 0x6c, 0x94, 0xbb, 0xa6, 0x07, 0x02,
            0x08, 0xaa, 0x54, 0x8f, 0x4d, 0x6c, 0xd7, 0x00,
        });
        Assert.IsTrue(expectedData.SequenceEqual(customData));
        var expectedMask = (stackalloc byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        });
        Assert.IsTrue(expectedMask.SequenceEqual(bitmask));
    }
#endif

    [TestMethod]
    public void TryGetCustomData_OtherVersionGuids_GetAllFalseResults()
    {
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetCustomData(out _, out _);
            Assert.IsFalse(result);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteCustomData_OtherVersionGuids_GetAllFalseResults()
    {
        var customData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryWriteCustomData(customData, bitmask);
            Assert.IsFalse(result);
        }
    }
#endif
}
#endif
