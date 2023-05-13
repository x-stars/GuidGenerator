using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version2_GetGuidWithVersion2()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        Assert.AreEqual(GuidVersion.Version2, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version2_GetGuidWithRfc4122Variant()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version2OfOrg_GetInputDomainAndLocalId()
    {
        var domain = DceSecurityDomain.Org;
        var localId = 0x12345678;
        var guid = GuidGenerator.Version2.NewGuid(domain, localId);
        var hasLocalId = guid.TryGetDomainAndLocalId(
            out var guidDomain, out var guidLocalId);
        Assert.IsTrue(hasLocalId);
        Assert.AreEqual(domain, guidDomain);
        Assert.AreEqual(localId, guidLocalId);
    }

    [TestMethod]
    public void NewGuid_Version2OfDomain_GetInputDomainAndLocalId()
    {
        var domains = Enum.GetValues(typeof(DceSecurityDomain));
        foreach (var domain in (DceSecurityDomain[])domains)
        {
            var localId = 0x12345678;
            var guid = GuidGenerator.Version2.NewGuid(domain, localId);
            _ = guid.TryGetDomainAndLocalId(
                out var guidDomain, out var guidLocalId);
            Assert.AreEqual(domain, guidDomain);
            Assert.AreEqual(localId, guidLocalId);
        }
    }

    [TestMethod]
    public void NewGuid_Version2_GetExpectedTimestamp()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        _ = guid.TryGetTimestamp(out var timestamp);
        var tsTicks = timestamp.Ticks;
        var nowTicks = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(nowTicks - tsTicks);
        var ticksPerSec = TimeSpan.FromSeconds(10).Ticks;
        Assert.IsTrue(ticksDiff < (ticksPerSec * 60 * 10));
    }

    [TestMethod]
    public void NewGuid_Version2_GetIncClockSeqWhenTimeBackward()
    {
        var domain = DceSecurityDomain.Person;
        var guid0 = GuidGenerator.Version2.NewGuid(domain);
        _ = guid0.TryGetTimestamp(out var timestamp0);
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        var guid1 = GuidGenerator.Version2.NewGuid(domain);
        _ = guid1.TryGetTimestamp(out var timestamp1);
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        if (timestamp1.Ticks < timestamp0.Ticks)
        {
            var expected = (byte)((clockSeq0 + 1) & ~0xC0);
            Assert.AreEqual(expected, (byte)clockSeq1);
        }
    }

    [TestMethod]
    public void NewGuid_Version2UnknownDomain_GetGuidWithInputDomain()
    {
        var domain = (DceSecurityDomain)0xFF;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        _ = guid.TryGetDomainAndLocalId(
            out var guidDomain, out var guidLocalId);
        Assert.AreEqual(domain, guidDomain);
        Assert.AreEqual(guidLocalId, default(int));
    }

    [TestMethod]
    public void NewGuid_NonDceSecurityVersions_CatchNotSupportedException()
    {
        foreach (var version in new[]
        {
            GuidVersion.Empty,
            GuidVersion.Version1,
            GuidVersion.Version3,
            GuidVersion.Version4,
            GuidVersion.Version5,
#if !FEATURE_DISABLE_UUIDREV
            GuidVersion.Version6,
            GuidVersion.Version7,
            GuidVersion.Version8,
            GuidVersion.MaxValue,
#endif
        })
        {
            Assert.ThrowsException<NotSupportedException>(
                () => GuidGenerator.OfVersion(version)
                    .NewGuid(DceSecurityDomain.Person, 0));
        }
    }
}
