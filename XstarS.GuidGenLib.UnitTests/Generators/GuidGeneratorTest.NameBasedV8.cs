using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8Sha256_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8Sha256.NewGuid(nsId, name);
        var expected = Guid.Parse("0e38fb05-6337-8c50-a201-6e2ec68fd5ac");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8Sha256StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8Sha256.NewGuid(nsId, name);
        var expected = Guid.Parse("401835fd-a627-870a-873f-ed73f2bc5b2c");
        Assert.AreEqual(expected, guid);
    }
}
