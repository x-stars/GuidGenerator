using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentTest
{
    [TestMethod]
    public void TryGetNodeId_Version1Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var result = guid.TryGetNodeId(out var nodeId);
        Assert.IsTrue(result);
        var expected = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        CollectionAssert.AreEqual(expected, nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version1Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        var nodeId = (stackalloc byte[6]);
        var result = guid.TryWriteNodeId(nodeId);
        Assert.IsTrue(result);
        var expected = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        Assert.IsTrue(expected.SequenceEqual(nodeId));
    }
#endif

    [TestMethod]
    public void TryGetNodeId_Version2Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-b402-00c04fd430c8");
        var result = guid.TryGetNodeId(out var nodeId);
        Assert.IsTrue(result);
        var expected = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        CollectionAssert.AreEqual(expected, nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version2Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-b402-00c04fd430c8");
        var nodeId = (stackalloc byte[6]);
        var result = guid.TryWriteNodeId(nodeId);
        Assert.IsTrue(result);
        var expected = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        Assert.IsTrue(expected.SequenceEqual(nodeId));
    }
#endif

    [TestMethod]
    public void TryGetNodeId_Version6Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("1d19dad6-ba7b-6810-80b4-00c04fd430c8");
        var result = guid.TryGetNodeId(out var nodeId);
        Assert.IsTrue(result);
        var expected = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        CollectionAssert.AreEqual(expected, nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version6Guid_GetExpectedBytes()
    {
        var guid = Guid.Parse("1d19dad6-ba7b-6810-80b4-00c04fd430c8");
        var nodeId = (stackalloc byte[6]);
        var result = guid.TryWriteNodeId(nodeId);
        Assert.IsTrue(result);
        var expected = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        Assert.IsTrue(expected.SequenceEqual(nodeId));
    }
#endif

    [TestMethod]
    public void TryGetNodeId_OtherVersionGuids_GetAllFalseResults()
    {
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetNodeId(out _);
            Assert.IsFalse(result);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_OtherVersionGuids_GetAllFalseResults()
    {
        var nodeId = (stackalloc byte[6]);
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
            var guid = Guid.Parse(guidText);
            var result = guid.TryWriteNodeId(nodeId);
            Assert.IsFalse(result);
        }
    }
#endif
}
