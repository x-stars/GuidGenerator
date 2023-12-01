using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentTest
{
    [TestMethod]
    public void TryGetRandomData_Version4Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("2502f1d5-c2a9-47d3-b6d8-d7670094ace2");
        var result = guid.TryGetRandomData(out var randomData, out var bitmask);
        Assert.IsTrue(result);
        var expectedData = new byte[]
        {
            0x25, 0x02, 0xf1, 0xd5, 0xc2, 0xa9, 0x07, 0xd3,
            0x36, 0xd8, 0xd7, 0x67, 0x00, 0x94, 0xac, 0xe2,
        };
        CollectionAssert.AreEqual(expectedData, randomData);
        var expectedMask = new byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        };
        CollectionAssert.AreEqual(expectedMask, bitmask);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_Version4Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("2502f1d5-c2a9-47d3-b6d8-d7670094ace2");
        var randomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        var result = guid.TryWriteRandomData(randomData, bitmask);
        Assert.IsTrue(result);
        var expectedData = (stackalloc byte[]
        {
            0x25, 0x02, 0xf1, 0xd5, 0xc2, 0xa9, 0x07, 0xd3,
            0x36, 0xd8, 0xd7, 0x67, 0x00, 0x94, 0xac, 0xe2,
        });
        Assert.IsTrue(expectedData.SequenceEqual(randomData));
        var expectedMask = (stackalloc byte[]
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        });
        Assert.IsTrue(expectedMask.SequenceEqual(bitmask));
    }
#endif

#if !UUIDREV_DISABLE
    [TestMethod]
    public void TryGetRandomData_Version7Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("017f22e2-79b0-7cc3-98c4-dc0c0c07398f");
        var result = guid.TryGetRandomData(out var randomData, out var bitmask);
        Assert.IsTrue(result);
        var expectedData = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0xc3,
            0x18, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f,
        };
        CollectionAssert.AreEqual(expectedData, randomData);
        var expectedMask = new byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        };
        CollectionAssert.AreEqual(expectedMask, bitmask);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_Version7Guid_GetExpectedBytesAndBitmask()
    {
        var guid = Guid.Parse("017f22e2-79b0-7cc3-98c4-dc0c0c07398f");
        var randomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
        var result = guid.TryWriteRandomData(randomData, bitmask);
        Assert.IsTrue(result);
        var expectedData = (stackalloc byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0xc3,
            0x18, 0xc4, 0xdc, 0x0c, 0x0c, 0x07, 0x39, 0x8f,
        });
        Assert.IsTrue(expectedData.SequenceEqual(randomData));
        var expectedMask = (stackalloc byte[]
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0f, 0xff,
            0x3f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        });
        Assert.IsTrue(expectedMask.SequenceEqual(bitmask));
    }
#endif
#endif

    [TestMethod]
    public void TryGetRandomData_OtherVersionGuids_GetAllFalseResults()
    {
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetRandomData(out _, out _);
            Assert.IsFalse(result);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteRandomData_OtherVersionGuids_GetAllFalseResults()
    {
        var randomData = (stackalloc byte[16]);
        var bitmask = (stackalloc byte[16]);
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryWriteRandomData(randomData, bitmask);
            Assert.IsFalse(result);
        }
    }
#endif
}
