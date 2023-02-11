using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version3_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("c87ee674-4ddc-3efe-a74e-dfe25da5d7b3");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version3SpanName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("c87ee674-4ddc-3efe-a74e-dfe25da5d7b3");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewGuid_Version3StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version3CharSpanName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator".AsSpan();
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewGuid_Version3NullName_CatchArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version3.NewGuid(Guid.Empty, default(byte[])!));
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version3.NewGuid(Guid.Empty, default(string)!));
    }

    [TestMethod]
    public void NewGuid_Version5_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("4ebd0208-8328-5d69-8c44-ec50939c0967");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version5SpanName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Dns;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("4ebd0208-8328-5d69-8c44-ec50939c0967");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewGuid_Version5StringName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version5CharSpanName_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator".AsSpan();
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(expected, guid);
    }
#endif

    [TestMethod]
    public void NewGuid_Version5NullName_CatchArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version5.NewGuid(Guid.Empty, default(byte[])!));
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version5.NewGuid(Guid.Empty, default(string)!));
    }

    [TestMethod]
    public void NewGuid_NonNameBasedVersions_CatchNotSupportedException()
    {
        foreach (var version in new[]
        {
            GuidVersion.Empty,
            GuidVersion.MaxValue,
            GuidVersion.Version1,
            GuidVersion.Version2,
            GuidVersion.Version4,
            GuidVersion.Version6,
            GuidVersion.Version7,
            GuidVersion.Version8,
        })
        {
            Assert.ThrowsException<NotSupportedException>(
                () => GuidGenerator.OfVersion(version)
                    .NewGuid(Guid.Empty, Array.Empty<byte>()));
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            Assert.ThrowsException<NotSupportedException>(
                () => GuidGenerator.OfVersion(version)
                    .NewGuid(Guid.Empty, ReadOnlySpan<byte>.Empty));
#endif
        }
    }
}
