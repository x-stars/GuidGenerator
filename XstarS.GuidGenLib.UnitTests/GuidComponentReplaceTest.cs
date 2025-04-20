using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public partial class GuidComponentReplaceTest
{
    [TestMethod]
    public void ReplaceVersion_EmptyGuid_GetGuidWithInputVersion()
    {
        var versions = Enum.GetValues(typeof(GuidVersion));
        foreach (var version in (GuidVersion[])versions)
        {
            var guid = Guid.Empty.ReplaceVersion(version);
            Assert.AreEqual(version, guid.GetVersion());
        }
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void ReplaceVersion_GuidMaxValue_GetGuidWithInputVersion()
    {
        var versions = Enum.GetValues(typeof(GuidVersion));
        foreach (var version in (GuidVersion[])versions)
        {
            var guid = Uuid.MaxValue.ReplaceVersion(version);
            Assert.AreEqual(version, guid.GetVersion());
        }
    }
#endif

    [TestMethod]
    public void ReplaceVersion_InvalidVersion_CatchArgumentOutOfRangeException()
    {
        foreach (var version in 16..byte.MaxValue)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Guid.Empty.ReplaceVersion((GuidVersion)version));
        }
    }

    [TestMethod]
    public void ReplaceVersion_GuidNewGuid_GetGuidWithInputVersionNumber()
    {
        var versions = Array.ConvertAll(
            (GuidVersion[])Enum.GetValues(typeof(GuidVersion)),
            version => (byte)version);
        foreach (var version in versions)
        {
            var guid = Guid.NewGuid().ReplaceVersion(version);
            Assert.AreEqual(version, (byte)guid.GetVersion());
        }
    }

    [TestMethod]
    public void ReplaceVariant_EmptyGuid_GetGuidWithInputVariant()
    {
        var variants = Enum.GetValues(typeof(GuidVariant));
        foreach (var variant in (GuidVariant[])variants)
        {
            var guid = Guid.Empty.ReplaceVariant(variant);
            Assert.AreEqual(variant, guid.GetVariant());
        }
    }

#if !UUIDREV_DISABLE
    [TestMethod]
    public void ReplaceVariant_GuidMaxValue_GetGuidWithInputVariant()
    {
        var variants = Enum.GetValues(typeof(GuidVariant));
        foreach (var variant in (GuidVariant[])variants)
        {
            var guid = Uuid.MaxValue.ReplaceVariant(variant);
            Assert.AreEqual(variant, guid.GetVariant());
        }
    }
#endif

    [TestMethod]
    public void ReplaceVariant_InvalidVariant_CatchArgumentOutOfRangeException()
    {
        foreach (var variant in 4..byte.MaxValue)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Guid.Empty.ReplaceVariant((GuidVariant)variant));
        }
    }

    [TestMethod]
    public void ReplaceDomainAndLocalId_Version2Guid_GetInputDomainAndLocalId()
    {
        var domain = DceSecurityDomain.Org;
        var localId = 0x6ba7b810;
        var original = Guid.Parse("00000000-9dad-21d1-b400-00c04fd430c8");
        var guid = original.ReplaceDomainAndLocalId(domain, localId);
        _ = guid.TryGetDomainAndLocalId(out var guidDoamin, out var guidLocalId);
        Assert.AreEqual(domain, guidDoamin);
        Assert.AreEqual(localId, guidLocalId);
    }

    [TestMethod]
    public void ReplaceDomainAndLocalId_OtherVersionGuids_GetOriginalGuidValues()
    {
        var domain = DceSecurityDomain.Org;
        var localId = 0x6ba7b810;
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var original = Guid.Parse(guidText);
            var guid = original.ReplaceDomainAndLocalId(domain, localId);
            Assert.AreEqual(original, guid);
        }
    }
}
