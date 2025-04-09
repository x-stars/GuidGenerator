using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public class CustomStateGuidGeneratorBuilderTest
{
    [TestMethod]
    public void Version1_UseDefaultProviders_GetPhysicalNodeId()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version1.ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        _ = GuidGenerator.Version1.NewGuid().TryGetNodeId(out var expected);
        CollectionAssert.AreEqual(expected, nodeId);
    }

    [TestMethod]
    public void Version1_UseNonVolatileNodeIdSource_GetRandomNodeId()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version1
            .UseNodeIdSource(NodeIdSource.NonVolatileRandom)
            .ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        _ = GuidGenerator.Version1R.NewGuid().TryGetNodeId(out var expected);
        CollectionAssert.AreEqual(expected, nodeId);
    }

    [TestMethod]
    public void Version1_UseNoneNodeIdSource_CatchInvalidOperationException()
    {
        Assert.ThrowsException<InvalidOperationException>(
            () => CustomStateGuidGeneratorBuilder.Version1
                .UseNodeIdSource(NodeIdSource.None)
                .ToGuidGenerator());
    }

    [TestMethod]
    public void Version1_UseBackwardTimestampProvider_GetIncClockSeq()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version1
            .UseTimestampProvider(() => new DateTime(
                DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks, DateTimeKind.Utc))
            .ToGuidGenerator();
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        var expected = (short)((clockSeq0 + 1) & ~0xC000);
        Assert.AreEqual(expected, clockSeq1);
    }

    [TestMethod]
    public void Version1_UseUserDefinedNodeId_GetInputNodeId()
    {
        var nodeId = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
        var guidGen = CustomStateGuidGeneratorBuilder.Version1
            .UseNodeId(nodeId)
            .ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

    [TestMethod]
    public void Version1_UseCustomNodeIdProvider_GetExpectedNodeId()
    {
        var nodeId = new byte[6];
        var randGen = new Random();
        var guidGen = CustomStateGuidGeneratorBuilder.Version1
            .UseNodeIdProvider(() => { randGen.NextBytes(nodeId); return nodeId; })
            .ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var guidNodeId);
        CollectionAssert.AreEqual(nodeId, guidNodeId);
    }

    [TestMethod]
    public void Version1_UseRandomNodeIdProvider_GetDifferentClockSequence()
    {
        var nodeId = new byte[6];
        var randGen = new Random();
        var guidGen = CustomStateGuidGeneratorBuilder.Version1
            .UseNodeIdProvider(() => { randGen.NextBytes(nodeId); return nodeId; })
            .ToGuidGenerator();
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        Assert.AreNotEqual(clockSeq0, clockSeq1);
    }

    [TestMethod]
    public void Version1_UseInvalidProviders_CatchArgumentExceptions()
    {
        var builder = CustomStateGuidGeneratorBuilder.Version1;
        Assert.ThrowsException<ArgumentNullException>(
            () => builder.UseTimestampProvider((Func<DateTime>)null!));
        Assert.ThrowsException<ArgumentNullException>(
            () => builder.UseTimestampProvider((Func<DateTimeOffset>)null!));
#if NET8_0_OR_GREATER
        Assert.ThrowsException<ArgumentNullException>(
            () => builder.UseTimeProvider((TimeProvider)null!));
#endif
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => builder.UseNodeIdSource((NodeIdSource)(-1)));
        Assert.ThrowsException<ArgumentNullException>(
            () => builder.UseNodeIdProvider(null!));
        Assert.ThrowsException<ArgumentNullException>(
            () => builder.UseNodeId(null!));
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void Version6_UseDefaultProviders_GetDifferentNodeIds()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version6.ToGuidGenerator();
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetNodeId(out var nodeId0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetNodeId(out var nodeId1);
        CollectionAssert.AreNotEqual(nodeId0, nodeId1);
    }

    [TestMethod]
    public void Version6_UsePhysicalNodeIdSource_GetPhysicalNodeId()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version6
            .UseNodeIdSource(NodeIdSource.PhysicalAddress)
            .ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        _ = GuidGenerator.Version6P.NewGuid().TryGetNodeId(out var expected);
        CollectionAssert.AreEqual(expected, nodeId);
    }

    [TestMethod]
    public void Version6_UseNonVolatileNodeIdSource_GetRandomNodeId()
    {
        var guidGen = CustomStateGuidGeneratorBuilder.Version6
            .UseNodeIdSource(NodeIdSource.NonVolatileRandom)
            .ToGuidGenerator();
        var guid = guidGen.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        _ = GuidGenerator.Version6R.NewGuid().TryGetNodeId(out var expected);
        CollectionAssert.AreEqual(expected, nodeId);
    }

    [TestMethod]
    public void Version6_UseBackwardTimestampProvider_GetIncClockSeq()
    {
        var guidGenV6R = CustomStateGuidGeneratorBuilder.Version6
            .UseNodeIdSource(NodeIdSource.NonVolatileRandom)
            .UseTimestampProvider(() => new DateTimeOffset(
                DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks, TimeSpan.Zero))
            .ToGuidGenerator();
        var guid0 = guidGenV6R.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = guidGenV6R.NewGuid();
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        var expected = (short)((clockSeq0 + 1) & ~0xC000);
        Assert.AreEqual(expected, clockSeq1);
    }

    [TestMethod]
    public void Version7_UseCustomTimestampProvider_GetExpectedTimestamp()
    {
        var timestampMs = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        var guidGen = CustomStateGuidGeneratorBuilder.Version7
#if NET8_0_OR_GREATER
            .UseTimeProvider(new CustomTimeProvider(() => new DateTimeOffset(
                timestampMs++ * TimeSpan.TicksPerMillisecond, TimeSpan.Zero)))
#else
            .UseTimestampProvider(() => new DateTimeOffset(
                timestampMs++ * TimeSpan.TicksPerMillisecond, TimeSpan.Zero))
#endif
            .ToGuidGenerator();
        var guid0 = guidGen.NewGuid();
        _ = guid0.TryGetTimestamp(out var timestamp0);
        var guid1 = guidGen.NewGuid();
        _ = guid1.TryGetTimestamp(out var timestamp1);
        var expected = timestamp0 + TimeSpan.FromMilliseconds(1);
        Assert.AreEqual(expected, timestamp1);
    }
#endif

    [TestMethod]
    public void Create_ValidGuidVersion_GetExpectedInstance()
    {
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version1,
                        CustomStateGuidGeneratorBuilder.Create(GuidVersion.Version1));
#if !UUIDREV_DISABLE
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version6,
                        CustomStateGuidGeneratorBuilder.Create(GuidVersion.Version6));
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version7,
                        CustomStateGuidGeneratorBuilder.Create(GuidVersion.Version7));
#endif
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version1,
                        GuidGenerator.CreateCustomStateBuilder(GuidVersion.Version1));
#if !UUIDREV_DISABLE
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version6,
                        GuidGenerator.CreateCustomStateBuilder(GuidVersion.Version6));
        Assert.AreEqual(CustomStateGuidGeneratorBuilder.Version7,
                        GuidGenerator.CreateCustomStateBuilder(GuidVersion.Version7));
#endif
    }

    [TestMethod]
    public void Create_InvalidGuidVersion_CatchOutOfRangeException()
    {
        foreach (var version in new[]
        {
#if !UUIDREV_DISABLE
            0, 2, 3, 4, 5,
#else
            0, 2, 3, 4, 5, 6, 7,
#endif
            8, 9, 10, 11, 12, 13, 14, 15,
        })
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CustomStateGuidGeneratorBuilder.Create((GuidVersion)version));
        }
    }
}
