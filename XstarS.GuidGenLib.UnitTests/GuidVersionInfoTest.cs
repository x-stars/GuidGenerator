using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidVersionInfoTest
{
    [TestMethod]
    public void GuidVersionInfo_EmptyVersion_GetExpectedFlags()
    {
        var version = GuidVersion.Empty;
        Assert.IsFalse(version.IsTimeBased());
        Assert.IsFalse(version.IsNameBased());
        Assert.IsFalse(version.IsRandomized());
        Assert.IsFalse(version.ContainsNodeId());
        Assert.IsFalse(version.ContainsLocalId());
    }

    [TestMethod]
    public void GuidVersionInfo_GuidVersion1_GetExpectedFlags()
    {
        var version = GuidVersion.Version1;
        Assert.IsTrue(version.IsTimeBased());
        Assert.IsFalse(version.IsNameBased());
        Assert.IsFalse(version.IsRandomized());
        Assert.IsTrue(version.ContainsNodeId());
        Assert.IsFalse(version.ContainsLocalId());
    }

    [TestMethod]
    public void GuidVersionInfo_GuidVersion2_GetExpectedFlags()
    {
        var version = GuidVersion.Version2;
        Assert.IsTrue(version.IsTimeBased());
        Assert.IsFalse(version.IsNameBased());
        Assert.IsFalse(version.IsRandomized());
        Assert.IsTrue(version.ContainsNodeId());
        Assert.IsTrue(version.ContainsLocalId());
    }

    [TestMethod]
    public void GuidVersionInfo_GuidVersion3_GetExpectedFlags()
    {
        var version = GuidVersion.Version3;
        Assert.IsFalse(version.IsTimeBased());
        Assert.IsTrue(version.IsNameBased());
        Assert.IsFalse(version.IsRandomized());
        Assert.IsFalse(version.ContainsNodeId());
        Assert.IsFalse(version.ContainsLocalId());
    }

    [TestMethod]
    public void GuidVersionInfo_GuidVersion4_GetExpectedFlags()
    {
        var version = GuidVersion.Version4;
        Assert.IsFalse(version.IsTimeBased());
        Assert.IsFalse(version.IsNameBased());
        Assert.IsTrue(version.IsRandomized());
        Assert.IsFalse(version.ContainsNodeId());
        Assert.IsFalse(version.ContainsLocalId());
    }

    [TestMethod]
    public void GuidVersionInfo_GuidVersion5_GetExpectedFlags()
    {
        var version = GuidVersion.Version5;
        Assert.IsFalse(version.IsTimeBased());
        Assert.IsTrue(version.IsNameBased());
        Assert.IsFalse(version.IsRandomized());
        Assert.IsFalse(version.ContainsNodeId());
        Assert.IsFalse(version.ContainsLocalId());
    }
}
