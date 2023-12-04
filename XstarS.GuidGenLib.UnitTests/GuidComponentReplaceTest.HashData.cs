using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceHashData_Version3Guid_GetInputBytes()
    {
        var hashData = new byte[]
        {
            0xa9, 0xec, 0x44, 0x20, 0x72, 0x52, 0x0c, 0x11,
            0x2b, 0x70, 0x51, 0x2e, 0x10, 0x27, 0x35, 0x37,
        };
        var original = Guid.Parse("00000000-0000-3000-8000-000000000000");
        var guid = original.ReplaceHashData(hashData);
        _ = guid.TryGetHashData(out var guidHashData, out _);
        CollectionAssert.AreEqual(hashData, guidHashData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteHashData_Version3Guid_GetInputBytes()
    {
        var hashData = (stackalloc byte[]
        {
            0xa9, 0xec, 0x44, 0x20, 0x72, 0x52, 0x0c, 0x11,
            0x2b, 0x70, 0x51, 0x2e, 0x10, 0x27, 0x35, 0x37,
        });
        var original = Guid.Parse("00000000-0000-3000-8000-000000000000");
        var guid = original.ReplaceHashData(hashData);
        var guidHashData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        _ = guid.TryWriteHashData(guidHashData, bitmask);
        Assert.IsTrue(hashData.SequenceEqual(guidHashData));
    }
#endif

    [TestMethod]
    public void ReplaceHashData_Version5Guid_GetInputBytes()
    {
        var hashData = new byte[]
        {
            0x76, 0x8a, 0x7b, 0x1b, 0xae, 0x51, 0x0c, 0x0a,
            0x3c, 0x9d, 0xa8, 0x5a, 0x34, 0x3f, 0x2c, 0x24,
        };
        var original = Guid.Parse("00000000-0000-5000-8000-000000000000");
        var guid = original.ReplaceHashData(hashData);
        _ = guid.TryGetHashData(out var guidHashData, out _);
        CollectionAssert.AreEqual(hashData, guidHashData);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteHashData_Version5Guid_GetInputBytes()
    {
        var hashData = (stackalloc byte[]
        {
            0x76, 0x8a, 0x7b, 0x1b, 0xae, 0x51, 0x0c, 0x0a,
            0x3c, 0x9d, 0xa8, 0x5a, 0x34, 0x3f, 0x2c, 0x24,
        });
        var original = Guid.Parse("00000000-0000-5000-8000-000000000000");
        var guid = original.ReplaceHashData(hashData);
        var guidHashData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        _ = guid.TryWriteHashData(guidHashData, bitmask);
        Assert.IsTrue(hashData.SequenceEqual(guidHashData));
    }
#endif

    [TestMethod]
    public void ReplaceHashData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var hashData = new byte[16]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceHashData(hashData);
            Assert.AreEqual(original, guid);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteHashData_OtherVersionGuids_GetOriginalGuidValues()
    {
        var hashData = (stackalloc byte[16]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        });
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceHashData(hashData);
            Assert.AreEqual(original, guid);
        }
    }
#endif

    [TestMethod]
    public void ReplaceHashData_OtherVariantGuids_GetOriginalGuidValues()
    {
        var hashData = new byte[16]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        foreach (var guidText in new[]
        {
            "a9ec4420-7252-3c11-2b70-512e10273537",
            "a9ec4420-7252-3c11-cb70-512e10273537",
            "a9ec4420-7252-3c11-eb70-512e10273537",
            "768a7b1b-ae51-5c0a-3c9d-a85a343f2c24",
            "768a7b1b-ae51-5c0a-dc9d-a85a343f2c24",
            "768a7b1b-ae51-5c0a-fc9d-a85a343f2c24",
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceHashData(hashData);
            Assert.AreEqual(original, guid);
        }
    }
}
