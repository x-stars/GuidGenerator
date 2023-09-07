using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceNodeId_Version1Guid_GetInputBytes()
    {
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var original = Guid.Parse("6ba7b810-9dad-11d1-80b4-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version1Guid_GetInputBytes()
    {
        var nodeId = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        var original = Guid.Parse("6ba7b810-9dad-11d1-80b4-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        var guidNodeId = (stackalloc byte[6]);
        _ = guid.TryWriteNodeId(guidNodeId);
        Assert.IsTrue(nodeId.SequenceEqual(guidNodeId));
    }
#endif

    [TestMethod]
    public void ReplaceNodeId_Version2Guid_GetInputBytes()
    {
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var original = Guid.Parse("6ba7b810-9dad-21d1-b402-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version2Guid_GetInputBytes()
    {
        var nodeId = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        var original = Guid.Parse("6ba7b810-9dad-21d1-b402-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        var guidNodeId = (stackalloc byte[6]);
        _ = guid.TryWriteNodeId(guidNodeId);
        Assert.IsTrue(nodeId.SequenceEqual(guidNodeId));
    }
#endif

#if !UUIDREV_DISABLE
    [TestMethod]
    public void ReplaceNodeId_Version6Guid_GetInputBytes()
    {
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
        var original = Guid.Parse("1d19dad6-ba7b-6810-80b4-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_Version6Guid_GetInputBytes()
    {
        var nodeId = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
        var original = Guid.Parse("1d19dad6-ba7b-6810-80b4-000000000000");
        var guid = original.ReplaceNodeId(nodeId);
        var guidNodeId = (stackalloc byte[6]);
        _ = guid.TryWriteNodeId(guidNodeId);
        Assert.IsTrue(nodeId.SequenceEqual(guidNodeId));
    }
#endif
#endif

    [TestMethod]
    public void ReplaceNodeId_OtherVersionGuids_GetAllFalseResults()
    {
        var nodeId = new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 };
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
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceNodeId(nodeId);
            Assert.AreEqual(original, guid);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteNodeId_OtherVersionGuids_GetAllFalseResults()
    {
        var nodeId = (stackalloc byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
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
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceNodeId(nodeId);
            Assert.AreEqual(original, guid);
        }
    }
#endif
}
