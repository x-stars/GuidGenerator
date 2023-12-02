using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceRandomData_Version4Guid_GetInputBytes()
    {
        var randomData = new byte[]
        {
            0x25, 0x02, 0xf1, 0xd5, 0xc2, 0xa9, 0x07, 0xd3,
            0x36, 0xd8, 0xd7, 0x67, 0x00, 0x94, 0xac, 0xe2,
        };
        var original = Guid.Parse("00000000-0000-4000-8000-000000000000");
        var guid = original.ReplaceRandomData(randomData);
        _ = guid.TryGetRandomData(out var guidRandomData, out _);
        CollectionAssert.AreEqual(randomData, guidRandomData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_Version4Guid_GetInputBytes()
    {
        var randomData = (stackalloc byte[]
        {
            0x25, 0x02, 0xf1, 0xd5, 0xc2, 0xa9, 0x07, 0xd3,
            0x36, 0xd8, 0xd7, 0x67, 0x00, 0x94, 0xac, 0xe2,
        });
        var original = Guid.Parse("00000000-0000-4000-8000-000000000000");
        var guid = original.ReplaceRandomData(randomData);
        var guidRandomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        _ = guid.TryWriteRandomData(guidRandomData, bitmask);
        Assert.IsTrue(randomData.SequenceEqual(guidRandomData));
    }
#endif

#if !UUIDREV_DISABLE
    [TestMethod]
    public void ReplaceRandomData_Version7Guid_GetInputBytes()
    {
        var randomData = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0xc3,
            0x18, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f,
        };
        var original = Guid.Parse("017f22e2-79b0-7000-8000-000000000000");
        var guid = original.ReplaceRandomData(randomData);
        _ = guid.TryGetRandomData(out var guidRandomData, out _);
        CollectionAssert.AreEqual(randomData, guidRandomData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_Version7Guid_GetInputBytes()
    {
        var randomData = (stackalloc byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0xc3,
            0x18, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f,
        });
        var original = Guid.Parse("017f22e2-79b0-7000-8000-000000000000");
        var guid = original.ReplaceRandomData(randomData);
        var guidRandomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        _ = guid.TryWriteRandomData(guidRandomData, bitmask);
        Assert.IsTrue(randomData.SequenceEqual(guidRandomData));
    }
#endif
#endif

    [TestMethod]
    public void ReplaceRandomData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var randomData = new byte[16];
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceRandomData(randomData);
            Assert.AreEqual(original, guid);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var randomData = (stackalloc byte[16]);
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceRandomData(randomData);
            Assert.AreEqual(original, guid);
        }
    }
#endif
}
