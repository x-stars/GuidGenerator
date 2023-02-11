using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_EmptyVersion_GetEmptyGuid()
    {
        var guid = GuidGenerator.Empty.NewGuid();
        Assert.AreEqual(Guid.Empty, guid);
    }

    [TestMethod]
    public void NewGuid_EmptyVersion_GetGuidWithEmptyVersion()
    {
        var guid = GuidGenerator.Empty.NewGuid();
        Assert.AreEqual(GuidVersion.Empty, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_EmptyVersion_GetGuidWithNcsVariant()
    {
        var guid = GuidGenerator.Empty.NewGuid();
        Assert.AreEqual(GuidVariant.Ncs, guid.GetVariant());
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
    public void OfVersion_InvalidVersionEnum_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfVersion((GuidVersion)0xFF));
    }
}
