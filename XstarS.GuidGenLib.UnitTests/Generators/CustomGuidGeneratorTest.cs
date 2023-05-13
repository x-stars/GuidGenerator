#if !FEATURE_DISABLE_UUIDREV
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public partial class CustomGuidGeneratorTest
{
    [TestMethod]
    public void GetCurrentTimestamp_CustomEpochDateTime_GetExpectedTimestamp()
    {
        var epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var guidGen = new TestCustomGuidGenerator(epochDateTime: epoch);
        var timestamp = guidGen.GetCurrentTimestamp();
        var nowTicks = DateTime.UtcNow.Ticks - epoch.Ticks;
        var ticksDiff = Math.Abs(nowTicks - timestamp);
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticksPerSec);
    }

    [TestMethod]
    public void GetNodeIdByte_NonVolatileRandom_GetVersion6NodeIdBytes()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.NonVolatileRandom);
        _ = GuidGenerator.Version6.NewGuid().TryGetNodeId(out var expected);
        var nodeIdBytes = new[]
        {
            guidGen.GetNodeIdByte(0), guidGen.GetNodeIdByte(1),
            guidGen.GetNodeIdByte(2), guidGen.GetNodeIdByte(3),
            guidGen.GetNodeIdByte(4), guidGen.GetNodeIdByte(5),
        };
        CollectionAssert.AreEqual(expected, nodeIdBytes);
    }

    [TestMethod]
    public void GetNodeIdByte_IndexOutOfRange_CatchArgumentOutOfRangeException()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => guidGen.GetNodeIdByte(-1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => guidGen.GetNodeIdByte(6));
    }

    [TestMethod]
    public void GetNodeIdBytes_PhysicalAddress_GetVersion1NodeIdBytes()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.PhysicalAddress);
        _ = GuidGenerator.Version1.NewGuid().TryGetNodeId(out var expected);
        var nodeIdBuffer = new byte[6];
        guidGen.GetNodeIdBytes(nodeIdBuffer);
        CollectionAssert.AreEqual(expected, nodeIdBuffer);
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        guidGen.GetNodeIdBytes((Span<byte>)nodeIdBuffer);
        CollectionAssert.AreEqual(expected, nodeIdBuffer);
#endif
    }

    [TestMethod]
    public void GetNodeIdBytes_VolatileAddress_GetDifferentNodeIdBytes()
    {
        var guidGen0 = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        var nodeIdBuffer0 = new byte[6];
        guidGen0.GetNodeIdBytes(nodeIdBuffer0);
        var guidGen1 = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        var nodeIdBuffer1 = new byte[6];
        guidGen1.GetNodeIdBytes(nodeIdBuffer1);
        CollectionAssert.AreNotEqual(nodeIdBuffer0, nodeIdBuffer1);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void GetNodeIdByteSpan_VolatileAddress_GetDifferentNodeIdBytes()
    {
        var guidGen0 = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        var nodeIdBuffer0 = (stackalloc byte[6]);
        guidGen0.GetNodeIdBytes(nodeIdBuffer0);
        var guidGen1 = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        var nodeIdBuffer1 = (stackalloc byte[6]);
        guidGen1.GetNodeIdBytes(nodeIdBuffer1);
        Assert.IsFalse(nodeIdBuffer0.SequenceEqual(nodeIdBuffer1));
    }
#endif

    [TestMethod]
    public void GetNodeIdBytes_NodeIdSourceNone_CatchNotSupportedException()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.None);
        var nodeIdBuffer = new byte[6];
        Assert.ThrowsException<NotSupportedException>(
            () => guidGen.GetNodeIdBytes(nodeIdBuffer));
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Assert.ThrowsException<NotSupportedException>(
            () => guidGen.GetNodeIdBytes((Span<byte>)nodeIdBuffer));
#endif
    }

    [TestMethod]
    public void GetNodeIdBytes_NullNodeIdBuffer_CatchArgumentNullException()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        Assert.ThrowsException<ArgumentNullException>(
            () => guidGen.GetNodeIdBytes(default(byte[])!));
    }

    [TestMethod]
    public void GetNodeIdBytes_TooSmallNodeIdBuffer_CatchArgumentException()
    {
        var guidGen = new TestCustomGuidGenerator(
            nodeIdSource: NodeIdSource.VolatileRandom);
        var nodeIdBuffer = new byte[4];
        Assert.ThrowsException<ArgumentException>(
            () => guidGen.GetNodeIdBytes(nodeIdBuffer));
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        Assert.ThrowsException<ArgumentException>(
            () => guidGen.GetNodeIdBytes((Span<byte>)nodeIdBuffer));
#endif
    }

    [TestMethod]
    public void GetRandomInt32_GenerateTwoValues_GetDifferentValues()
    {
        var guidGen = new TestCustomGuidGenerator();
        var random0 = guidGen.GetRandomInt32();
        var random1 = guidGen.GetRandomInt32();
        Assert.AreNotEqual(random0, random1);
    }

    [TestMethod]
    public void GetRandomInt64_GenerateTwoValues_GetDifferentValues()
    {
        var guidGen = new TestCustomGuidGenerator();
        var random0 = guidGen.GetRandomInt64();
        var random1 = guidGen.GetRandomInt64();
        Assert.AreNotEqual(random0, random1);
    }
}
#endif
