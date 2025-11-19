using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidCreationTest
{
    [TestMethod]
    public void NewVersion1_NoInput_GetVersion1Guid()
    {
        var guid = Guid.NewVersion1();
        Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion1R_NoInput_GetVersion1GuidWithMulticastBit()
    {
        var guid = Guid.NewVersion1R();
        Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
        Assert.IsTrue(guid.TryGetNodeId(out var nodeId));
        Assert.AreEqual(0x01, nodeId[0] & 0x01);
    }

    [TestMethod]
    public void NewVersion2_DomainAndLocalId_GetVersion2GuidWithInputValues()
    {
        var domain = DceSecurityDomain.Org;
        var localId = 0x12345678;
        var guid = Guid.NewVersion2(domain, localId);
        Assert.AreEqual(GuidVersion.Version2, guid.GetVersion());
        Assert.IsTrue(guid.TryGetDomainAndLocalId(
            out var guidDomain, out var guidLocalId));
        Assert.AreEqual(domain, guidDomain);
        Assert.AreEqual(localId, guidLocalId);
    }

    [TestMethod]
    public void NewVersion3_ByteArrayName_GetVersion3GuidInExpectedHash()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = Guid.NewVersion3(nsId, name);
        Assert.AreEqual(GuidVersion.Version3, guid.GetVersion());
        var expected = Guid.Parse("4ae71336-e44b-39bf-b9d2-752e234818a5");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion3_ByteSpanName_GetVersion3GuidInExpectedHash()
    {
        var nsId = Guid.Empty;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = Guid.NewVersion3(nsId, name);
        Assert.AreEqual(GuidVersion.Version3, guid.GetVersion());
        var expected = Guid.Parse("4ae71336-e44b-39bf-b9d2-752e234818a5");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewVersion3_StringName_GetVersion3GuidInExpectedHash()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = Guid.NewVersion3(nsId, name);
        Assert.AreEqual(GuidVersion.Version3, guid.GetVersion());
        var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion3_CharSpanName_GetVersion3GuidInExpectedHash()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator".AsSpan();
        var guid = Guid.NewVersion3(nsId, name);
        Assert.AreEqual(GuidVersion.Version3, guid.GetVersion());
        var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewVersion4_NoInput_GetVersion4Guid()
    {
        var guid = Guid.NewVersion4();
        Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion5_ByteArrayName_GetVersion5GuidInExpectedHash()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = Guid.NewVersion5(nsId, name);
        Assert.AreEqual(GuidVersion.Version5, guid.GetVersion());
        var expected = Guid.Parse("e129f27c-5103-5c5c-844b-cdf0a15e160d");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion5_ByteSpanName_GetVersion5GuidInExpectedHash()
    {
        var nsId = Guid.Empty;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = Guid.NewVersion5(nsId, name);
        Assert.AreEqual(GuidVersion.Version5, guid.GetVersion());
        var expected = Guid.Parse("e129f27c-5103-5c5c-844b-cdf0a15e160d");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewVersion5_StringName_GetVersion5GuidInExpectedHash()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = Guid.NewVersion5(nsId, name);
        Assert.AreEqual(GuidVersion.Version5, guid.GetVersion());
        var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion5_CharSpanName_GetVersion5GuidInExpectedHash()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator".AsSpan();
        var guid = Guid.NewVersion5(nsId, name);
        Assert.AreEqual(GuidVersion.Version5, guid.GetVersion());
        var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(expected, guid);
    }
#endif

#if !UUIDREV_DISABLE
    [TestMethod]
    public void NewVersion6_NoInput_GetVersion6Guid()
    {
        var guid = Guid.NewVersion6();
        Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion6P_NoInput_GetVersion6Guid()
    {
        var guid = Guid.NewVersion6P();
        Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion6R_NoInput_GetVersion6GuidWithMulticastBit()
    {
        var guid = Guid.NewVersion6R();
        Assert.AreEqual(GuidVersion.Version6, guid.GetVersion());
        Assert.IsTrue(guid.TryGetNodeId(out var nodeId));
        Assert.AreEqual(0x01, nodeId[0] & 0x01);
    }

    [TestMethod]
    public void NewVersion7_NoInput_GetVersion7Guid()
    {
        var guid = Guid.NewVersion7();
        Assert.AreEqual(GuidVersion.Version7, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion8_NoInput_GetVersion8Guid()
    {
        var guid = Guid.NewVersion8();
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
    }

    [TestMethod]
    public void NewVersion8N_Sha256ByteArrayName_GetVersion8GuidInExpectedHash()
    {
        using var hashing = SHA256.Create();
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = Guid.NewVersion8N(hashing, nsId, name);
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
        var expected = Guid.Parse("374708ff-f771-8dd5-979e-c875d56cd228");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion8N_Sha256ByteSpanName_GetVersion8GuidInExpectedHash()
    {
        using var hashing = SHA256.Create();
        var nsId = Guid.Empty;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = Guid.NewVersion8N(hashing, nsId, name);
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
        var expected = Guid.Parse("374708ff-f771-8dd5-979e-c875d56cd228");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewVersion8N_Sha256StringName_GetVersion8GuidInExpectedHash()
    {
        using var hashing = SHA256.Create();
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com";
        var guid = Guid.NewVersion8N(hashing, nsId, name);
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
        var expected = Guid.Parse("5c146b14-3c52-8afd-938a-375d0df1fbf6");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewVersion8N_Sha256CharSpanName_GetVersion8GuidInExpectedHash()
    {
        using var hashing = SHA256.Create();
        var nsId = GuidNamespaces.Dns;
        var name = "www.example.com".AsSpan();
        var guid = Guid.NewVersion8N(hashing, nsId, name);
        Assert.AreEqual(GuidVersion.Version8, guid.GetVersion());
        var expected = Guid.Parse("5c146b14-3c52-8afd-938a-375d0df1fbf6");
        Assert.AreEqual(expected, guid);
    }
#endif
#endif
}
