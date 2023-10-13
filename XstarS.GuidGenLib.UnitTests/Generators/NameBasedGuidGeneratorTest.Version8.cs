#if !UUIDREV_DISABLE
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8NSha256_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name);
        var expected = Guid.Parse("374708ff-f771-8dd5-979e-c875d56cd228");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha256StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name);
        var expected = Guid.Parse("5c146b14-3c52-8afd-938a-375d0df1fbf6");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha256UnicodeStringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var encoding = Encoding.Unicode;
        var guid = GuidGenerator.Version8NSha256.NewGuid(nsId, name, encoding);
        var expected = Guid.Parse("55e88991-1eda-876a-8ff8-65f5a9fbd5db");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha384_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha384.NewGuid(nsId, name);
        var expected = Guid.Parse("ae40659d-a119-8cde-88df-474b5e36416a");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha384StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha384.NewGuid(nsId, name);
        var expected = Guid.Parse("3df00ae4-42a7-8066-88ad-1f925b8b8e54");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha512_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha512.NewGuid(nsId, name);
        var expected = Guid.Parse("0b6cbac8-38df-87f4-bea1-bd0df00ec282");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha512StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha512.NewGuid(nsId, name);
        var expected = Guid.Parse("94ee4ddb-9f36-8018-9ccf-86a4441691e0");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256_GetExpectedGuid()
    {
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashing);
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = guidGen.NewGuid(nsId, name);
        var expected = Guid.Parse("4ebc3bf9-4458-8d83-baae-f9d9dc2ad979");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256StringName_GetExpectedGuid()
    {
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashing);
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = guidGen.NewGuid(nsId, name);
        var expected = Guid.Parse("5c146b14-3c52-8afd-938a-375d0df1fbf6");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomSha256UnicodeStringName_GetExpectedGuid()
    {
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashing);
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var encoding = Encoding.Unicode;
        var guid = guidGen.NewGuid(nsId, name, encoding);
        var expected = Guid.Parse("55e88991-1eda-876a-8ff8-65f5a9fbd5db");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NCustomByValue_ConcurrentGetExpectedGuid()
    {
        if (Environment.ProcessorCount <= 1) { return; }
        using var hashing = SHA256.Create();
        using var guidGen = GuidGenerator.CreateVersion8N(hashing);
        Parallel.For(0, 1000, index =>
        {
            var nsId = Guid.Empty;
            var name = Array.Empty<byte>();
            var guid = guidGen.NewGuid(nsId, name);
            var expected = Guid.Parse("374708ff-f771-8dd5-979e-c875d56cd228");
            Assert.AreEqual(expected, guid);
        });
    }

    [TestMethod]
    public void NewGuid_Version8NCustomByDelegate_ConcurrentGetExpectedGuid()
    {
        using var guidGen = GuidGenerator.CreateVersion8N(SHA256.Create);
        Parallel.For(0, 1000, index =>
        {
            var nsId = Guid.Empty;
            var name = Array.Empty<byte>();
            var guid = guidGen.NewGuid(nsId, name);
            var expected = Guid.Parse("374708ff-f771-8dd5-979e-c875d56cd228");
            Assert.AreEqual(expected, guid);
        });
    }
}
#endif
