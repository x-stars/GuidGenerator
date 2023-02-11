using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version3_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = GuidGenerator.Version3.NewGuid(nsId, name);
        var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version3SpanName_GetExpectedGuid()
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
            () => GuidGenerator.Version3.NewGuid(Guid.Empty, default(string)!));
    }

    [TestMethod]
    public void NewGuid_Version5_GetExpectedGuid()
    {
        var nsId = GuidNamespaces.Url;
        var name = "https://github.com/x-stars/GuidGenerator";
        var guid = GuidGenerator.Version5.NewGuid(nsId, name);
        var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(expected, guid);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void NewGuid_Version5SpanName_GetExpectedGuid()
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
            () => GuidGenerator.Version5.NewGuid(Guid.Empty, default(string)!));
    }
}
