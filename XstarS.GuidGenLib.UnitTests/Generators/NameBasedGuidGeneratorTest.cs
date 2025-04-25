using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public partial class NameBasedGuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version3_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("4ae71336-e44b-39bf-b9d2-752e234818a5");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version3SpanName_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("4ae71336-e44b-39bf-b9d2-752e234818a5");
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
            () => GuidGenerator.Version3.NewGuid(Guid.Empty, (byte[])null!));
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version3.NewGuid(Guid.Empty, (string)null!));
    }

    [TestMethod]
    public void NewGuid_Version5_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = Array.Empty<byte>();
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("e129f27c-5103-5c5c-844b-cdf0a15e160d");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version5SpanName_GetExpectedGuid()
    {
        var nsId = Guid.Empty;
        var name = ReadOnlySpan<byte>.Empty;
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("e129f27c-5103-5c5c-844b-cdf0a15e160d");
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
            () => GuidGenerator.Version5.NewGuid(Guid.Empty, (byte[])null!));
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.Version5.NewGuid(Guid.Empty, (string)null!));
    }

    [TestMethod]
    public void NewGuid_NonNameBasedVersions_CatchNotSupportedException()
    {
        foreach (var version in new[]
        {
            GuidVersion.Empty,
            GuidVersion.Version1,
            GuidVersion.Version2,
            GuidVersion.Version4,
#if !UUIDREV_DISABLE
            GuidVersion.Version6,
            GuidVersion.Version7,
            GuidVersion.Version8,
            GuidVersion.MaxValue,
#endif
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
