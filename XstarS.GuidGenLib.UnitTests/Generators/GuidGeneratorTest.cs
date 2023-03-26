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

    [TestMethod]
    public void OfVersion_AllVersionBytes_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Empty, GuidGenerator.OfVersion((byte)0));
        Assert.AreSame(GuidGenerator.Version1, GuidGenerator.OfVersion(1));
        Assert.AreSame(GuidGenerator.Version2, GuidGenerator.OfVersion(2));
        Assert.AreSame(GuidGenerator.Version3, GuidGenerator.OfVersion(3));
        Assert.AreSame(GuidGenerator.Version4, GuidGenerator.OfVersion(4));
        Assert.AreSame(GuidGenerator.Version5, GuidGenerator.OfVersion(5));
        Assert.AreSame(GuidGenerator.Version6, GuidGenerator.OfVersion(6));
        Assert.AreSame(GuidGenerator.Version7, GuidGenerator.OfVersion(7));
        Assert.AreSame(GuidGenerator.Version8, GuidGenerator.OfVersion(8));
        Assert.AreSame(GuidGenerator.MaxValue, GuidGenerator.OfVersion(15));
    }

    [TestMethod]
    public void OfVersion_AllVersionEnums_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Empty,
                       GuidGenerator.OfVersion(GuidVersion.Empty));
        Assert.AreSame(GuidGenerator.Version1,
                       GuidGenerator.OfVersion(GuidVersion.Version1));
        Assert.AreSame(GuidGenerator.Version2,
                       GuidGenerator.OfVersion(GuidVersion.Version2));
        Assert.AreSame(GuidGenerator.Version3,
                       GuidGenerator.OfVersion(GuidVersion.Version3));
        Assert.AreSame(GuidGenerator.Version4,
                       GuidGenerator.OfVersion(GuidVersion.Version4));
        Assert.AreSame(GuidGenerator.Version5,
                       GuidGenerator.OfVersion(GuidVersion.Version5));
        Assert.AreSame(GuidGenerator.Version6,
                       GuidGenerator.OfVersion(GuidVersion.Version6));
        Assert.AreSame(GuidGenerator.Version7,
                       GuidGenerator.OfVersion(GuidVersion.Version7));
        Assert.AreSame(GuidGenerator.Version8,
                       GuidGenerator.OfVersion(GuidVersion.Version8));
        Assert.AreSame(GuidGenerator.MaxValue,
                       GuidGenerator.OfVersion(GuidVersion.MaxValue));
    }

    [TestMethod]
    public void OfVersion_InvalidVersionByte_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfVersion(0xFF));
    }

    [TestMethod]
    public void OfVersion_InvalidVersionEnum_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfVersion((GuidVersion)0xFF));
    }
}
