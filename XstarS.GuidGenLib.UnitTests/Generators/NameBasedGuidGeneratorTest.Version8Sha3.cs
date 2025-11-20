#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version8NSha3D256_GetExpectedGuid()
    {
        if (!SHA3_256.IsSupported) { return; }
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha3D256.NewGuid(nsId, name);
        var expected = Guid.Parse("61664696-888a-8102-b8ff-672620c85217");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha3D256StringName_GetExpectedGuid()
    {
        if (!SHA3_256.IsSupported) { return; }
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha3D256.NewGuid(nsId, name);
        var expected = Guid.Parse("fc506eca-a1f4-8315-87c8-c71449dfd324");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha3D384_GetExpectedGuid()
    {
        if (!SHA3_384.IsSupported) { return; }
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha3D384.NewGuid(nsId, name);
        var expected = Guid.Parse("a78e349c-372b-8ed0-abcb-0d141600cc2d");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha3D384StringName_GetExpectedGuid()
    {
        if (!SHA3_384.IsSupported) { return; }
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha3D384.NewGuid(nsId, name);
        var expected = Guid.Parse("ab4f9412-4e4c-87a5-be26-dc72f9adf6ed");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha3D512_GetExpectedGuid()
    {
        if (!SHA3_512.IsSupported) { return; }
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NSha3D512.NewGuid(nsId, name);
        var expected = Guid.Parse("f0140e31-4ee3-8d44-b239-3680e7a72a81");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NSha3D512StringName_GetExpectedGuid()
    {
        if (!SHA3_512.IsSupported) { return; }
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NSha3D512.NewGuid(nsId, name);
        var expected = Guid.Parse("83120d41-2935-8110-964c-6bd77c735fbc");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NShake128_GetExpectedGuid()
    {
        if (!Shake128.IsSupported) { return; }
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NShake128.NewGuid(nsId, name);
        var expected = Guid.Parse("8f8e4f61-2e61-8fb9-978c-3ea707e37768");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NShake128StringName_GetExpectedGuid()
    {
        if (!Shake128.IsSupported) { return; }
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NShake128.NewGuid(nsId, name);
        var expected = Guid.Parse("54e7e64f-0dba-8913-8b69-cd3dbf220c7a");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NShake256_GetExpectedGuid()
    {
        if (!Shake256.IsSupported) { return; }
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version8NShake256.NewGuid(nsId, name);
        var expected = Guid.Parse("d570b23c-455f-8f43-84bf-34aa6f2b7628");
        Assert.AreEqual(expected, guid);
    }

    [TestMethod]
    public void NewGuid_Version8NShake256StringName_GetExpectedGuid()
    {
        if (!Shake256.IsSupported) { return; }
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = GuidGenerator.Version8NShake256.NewGuid(nsId, name);
        var expected = Guid.Parse("10e1aa77-4f8c-8a0f-be35-354cc01a76ac");
        Assert.AreEqual(expected, guid);
    }
}
#endif
