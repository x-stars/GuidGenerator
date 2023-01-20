using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidUrnFormatTest
{
    [TestMethod]
    public void ToUrnString_EmptyGuid_GetUrnFormatString()
    {
        var guid = Guid.Empty;
        var urnFormat = guid.ToUrnString();
        var expected = "urn:uuid:00000000-0000-0000-0000-000000000000";
        Assert.AreEqual(expected, urnFormat);
    }

    [TestMethod]
    public void ToUrnString_RandomGuid_GetUrnFormatString()
    {
        var guid = Guid.NewGuid();
        var urnFormat = guid.ToUrnString();
        var expected = "urn:uuid:" + guid.ToString("D");
        Assert.AreEqual(expected, urnFormat);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryFormatUrn_EmptyGuidWithExactSizeSpan_GetUrnFormatString()
    {
        const int urnFormatLength = 9 + 36;
        var guid = Guid.Empty;
        var urnBuffer = (stackalloc char[urnFormatLength]);
        var result = guid.TryFormatUrn(urnBuffer, out int charsWritten);
        Assert.IsTrue(result);
        Assert.AreEqual(urnFormatLength, charsWritten);
        var expected = "urn:uuid:00000000-0000-0000-0000-000000000000".AsSpan();
        Assert.IsTrue(expected.SequenceEqual(urnBuffer));
    }

    [TestMethod]
    public void TryFormatUrn_EmptyGuidWithSmallerSizeSpan_GetFalseResult()
    {
        const int guidFormatLength = 36;
        var guid = Guid.Empty;
        var urnBuffer = (stackalloc char[guidFormatLength]);
        var result = guid.TryFormatUrn(urnBuffer, out int charsWritten);
        Assert.IsFalse(result);
        Assert.AreEqual(0, charsWritten);
    }
#endif

    [TestMethod]
    public void ParseUrn_RandomGuidUrnFormat_GetOriginalGuid()
    {
        var guid = Guid.NewGuid();
        var urnFormat = "urn:uuid:" + guid.ToString("D");
        var parsed = GuidExtensions.ParseUrn(urnFormat);
        Assert.AreEqual(guid, parsed);
    }

    [TestMethod]
    public void ParseUrn_NullInput_CatchArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidExtensions.ParseUrn(null!));
    }

    [TestMethod]
    public void ParseUrn_InvalidUrnFormat_CatchFormatException()
    {
        Assert.ThrowsException<FormatException>(
            () => GuidExtensions.ParseUrn(Guid.NewGuid().ToString("D")));
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void ParseUrn_RandomGuidUrnFormatSpan_GetOriginalGuid()
    {
        var guid = Guid.NewGuid();
        var urnFormat = ("URN:UUID:" + guid.ToString("D")).AsSpan();
        var parsed = GuidExtensions.ParseUrn(urnFormat);
        Assert.AreEqual(guid, parsed);
    }
#endif

    [TestMethod]
    public void TryParseUrn_RandomGuidUrnFormat_GetOriginalGuid()
    {
        var guid = Guid.NewGuid();
        var urnFormat = "URN:UUID:" + guid.ToString("D");
        var result = GuidExtensions.TryParseUrn(urnFormat, out var parsed);
        Assert.IsTrue(result);
        Assert.AreEqual(guid, parsed);
    }

    [TestMethod]
    public void TryParseUrn_NullInput_GetFalseResult()
    {
        var result = GuidExtensions.TryParseUrn(null!, out _);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryParseUrn_InvalidUrnFormat_GetFalseResult()
    {
        var invalidFormat = Guid.NewGuid().ToString("D");
        var result = GuidExtensions.TryParseUrn(invalidFormat, out _);
        Assert.IsFalse(result);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryParseUrn_RandomGuidUrnFormatSpan_GetOriginalGuid()
    {
        var guid = Guid.NewGuid();
        var urnFormat = ("urn:uuid:" + guid.ToString("D")).AsSpan();
        var result = GuidExtensions.TryParseUrn(urnFormat, out var parsed);
        Assert.IsTrue(result);
        Assert.AreEqual(guid, parsed);
    }

    [TestMethod]
    public void TryParseUrn_EmptyFormatSpan_GetFalseResult()
    {
        var urnFormat = ReadOnlySpan<char>.Empty;
        var result = GuidExtensions.TryParseUrn(urnFormat, out _);
        Assert.IsFalse(result);
    }
#endif
}
