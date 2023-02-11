using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorTest
{
    [TestMethod]
    public void NewGuid_Version2_GetExpectedTimestamp()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        var hasTs = guid.TryGetTimestamp(out var timestamp);
        Assert.IsTrue(hasTs);
        var ticksTs = timestamp.Ticks;
        var ticksNow = DateTime.UtcNow.Ticks;
        var ticksDiff = Math.Abs(ticksNow - ticksTs);
        var ticks10m = TimeSpan.FromMinutes(10).Ticks;
        Assert.IsTrue(ticksDiff < ticks10m);
    }

    [TestMethod]
    public void NewGuid_Version2_GetGuidWithVersion2()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        Assert.AreEqual(GuidVersion.Version2, guid.GetVersion());
    }

    [TestMethod]
    public void NewGuid_Version2_GetInputDomainAndLocalId()
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
    public void NewGuid_Version2_GetGuidWithRfc4122Variant()
    {
        var domain = DceSecurityDomain.Person;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
    }

    [TestMethod]
    public void NewGuid_Version2UnknownDomain_GetGuidWithInputDomain()
    {
        var domain = (DceSecurityDomain)0xFF;
        var guid = GuidGenerator.Version2.NewGuid(domain);
        var hasLocalId = guid.TryGetDomainAndLocalId(
            out var guidDomain, out var guidLocalId);
        Assert.IsTrue(hasLocalId);
        Assert.AreEqual(domain, guidDomain);
        Assert.AreEqual(guidLocalId, default(int));
    }
}
