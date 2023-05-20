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

#if !FEATURE_DISABLE_UUIDREV
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
#endif
}
