#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8NSha256_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name);
        var expected = Guid.Parse("0e38fb05-6337-8c50-a201-6e2ec68fd5ac");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha256StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name);
        var expected = Guid.Parse("401835fd-a627-870a-873f-ed73f2bc5b2c");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha256UnicodeStringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var encoding = Encoding.Unicode;
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name, encoding);
        var expected = Guid.Parse("909371f1-5575-8bf9-82e4-0a96ce6c1f90");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha384_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha384.NewGuid(nsId, name);
        var expected = Guid.Parse("62f1016f-87be-8f20-92fc-2c7a85fffd51");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha384StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha384.NewGuid(nsId, name);
        var expected = Guid.Parse("cbf4bd9c-024c-849c-bb0d-8bffc5be3afe");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha512_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha512.NewGuid(nsId, name);
        var expected = Guid.Parse("16bf9d22-5233-8783-95c7-c06484e01ae2");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha512StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha512.NewGuid(nsId, name);
        var expected = Guid.Parse("da65f7c3-21ae-82e0-a654-97eea9af6ed6");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256_GetExpectedGuid()
    {
        var hashId = GuidHashspaces.Sha256;
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashId, hashing);
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = guidGen.NewGuid(nsId, name);
        var expected = Guid.Parse("0e38fb05-6337-8c50-a201-6e2ec68fd5ac");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256StringName_GetExpectedGuid()
    {
        var hashId = GuidHashspaces.Sha256;
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashId, hashing);
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = guidGen.NewGuid(nsId, name);
        var expected = Guid.Parse("401835fd-a627-870a-873f-ed73f2bc5b2c");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256UnicodeStringName_GetExpectedGuid()
    {
        var hashId = GuidHashspaces.Sha256;
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashId, hashing);
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var encoding = Encoding.Unicode;
        var guid = guidGen.NewGuid(nsId, name, encoding);
        var expected = Guid.Parse("909371f1-5575-8bf9-82e4-0a96ce6c1f90");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomByValue_ConcurrentGetExpectedGuid()
    {
        if (Environment.ProcessorCount <= 1) { return; }
        var hashId = GuidHashspaces.Sha256;
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashId, hashing);
        Parallel.For(0, 1000, index =>
        {
            var nsId = GuidNamespaces.Dns;
            var name = Array.Empty<byte>();
            var guid = guidGen.NewGuid(nsId, name);
            var expected = Guid.Parse("0e38fb05-6337-8c50-a201-6e2ec68fd5ac");
            Assert.AreEqual(expected, guid);
        });
    }

    [TestMethod]
    public void NewGuid_Version8NCustomByDelegate_ConcurrentGetExpectedGuid()
    {
        var hashId = GuidHashspaces.Sha256;
        using var guidGen = GuidGenerator.CreateVersion8N(hashId, SHA256.Create);
        Parallel.For(0, 1000, index =>
        {
            var nsId = GuidNamespaces.Dns;
            var name = Array.Empty<byte>();
            var guid = guidGen.NewGuid(nsId, name);
            var expected = Guid.Parse("0e38fb05-6337-8c50-a201-6e2ec68fd5ac");
            Assert.AreEqual(expected, guid);
        });
    }
}
#endif
