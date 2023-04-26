using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_EmptyVersion_GetEmptyGuid()
    {
        var guid = GuidGenerator.Empty.NewGuid();
        Assert.AreEqual(Guid.Empty, guid);
    }

    [TestMethod]
    public void NewGuid_Version4_GetGuidWithVersion4()
    {
        var guid = GuidGenerator.Version4.NewGuid();
        Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version4_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version4.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version4_GetDifferentGuidValues()
    {
        var guid0 = GuidGenerator.Version4.NewGuid();
        var guid1 = GuidGenerator.Version4.NewGuid();
        Assert.AreNotEqual(guid0, guid1);
    }

    [TestMethod]
    public void NewGuid_Version7_GetGuidWithVersion7()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        Assert.AreEqual(GuidVersion.Version7, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version7_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version7_GetExpectedTimestamp()
    {
        var guid = GuidGenerator.Version7.NewGuid();
        var hasTs = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(hasTs);
        var tsTicks = timestamp.Ticks;
        var nowTicks = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        var ticksPerSec = TimeSpan.FromSeconds(1).Ticks;
        Assert.IsTrue(ticksDiff < ticksPerSec);
    }

    [TestMethod]
    public void NewGuid_Version7_GetDifferentLower8Bytes()
    {
        static byte[] GetGuidLower8Bytes(Guid guid)
        {
            var (_, _, _,
                lower0, lower1, lower2, lower3,
                lower4, lower5, lower6, lower7
                ) = guid;
            return new[]
            {
                lower0, lower1, lower2, lower3,
                lower4, lower5, lower6, lower7,
            };
        }

        var guid0 = GuidGenerator.Version7.NewGuid();
        var guid0Lower = GetGuidLower8Bytes(guid0);
        var guid1 = GuidGenerator.Version7.NewGuid();
        var guid1Lower = GetGuidLower8Bytes(guid1);
        CollectionAssert.AreNotEqual(guid0Lower, guid1Lower);
    }

    [TestMethod]
    public void NewGuid_Version8_GetGuidWithVersion8()
    {
        var guid = GuidGenerator.Version8.NewGuid();
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version8_GetGuidWithRfc4122Variant()
    {
        var guid = GuidGenerator.Version8.NewGuid();
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version8_GetDifferentGuidValues()
    {
        var guid0 = GuidGenerator.Version8.NewGuid();
        var guid1 = GuidGenerator.Version8.NewGuid();
        Assert.AreNotEqual(guid0, guid1);
    }

    [TestMethod]
    public void NewGuid_VersionMaxValue_GetGuidMaxValue()
    {
        var guid = GuidGenerator.MaxValue.NewGuid();
        Assert.AreEqual(Uuid.MaxValue, guid);
    }
}
