using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.GuidGenerators
{
    [TestClass]
    [DoNotParallelize]
    public class GuidGeneratorTest
    {
        [TestMethod]
        public void NewGuid_EmptyVersion_GetEmptyGuid()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Empty);
            Assert.AreEqual(Guid.Empty, guid);
        }

        [TestMethod]
        public void NewGuid_EmptyVersion_GetGuidWithEmptyVersion()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Empty);
            Assert.AreEqual(GuidVersion.Empty, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_EmptyVersion_GetGuidWithNcsVariant()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Empty);
            Assert.AreEqual(GuidVariant.Ncs, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version1_GetExpectedTimestamp()
        {
            var guid = GuidGenerator.Version1.NewGuid();
            var hasTs = guid.TryGetTimestamp(out var timestamp);
            Assert.IsTrue(hasTs);
            var ticksTs = timestamp.Ticks;
            var ticksNow = DateTime.UtcNow.Ticks;
            var ticksDiff = ticksNow - ticksTs;
            var ticks1s = TimeSpan.FromSeconds(1).Ticks;
            Assert.IsTrue(ticksDiff < ticks1s);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithVersion1()
        {
            var guid = GuidGenerator.Version1.NewGuid();
            Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version1_GetIncrementClockSeq()
        {
            var guid0 = GuidGenerator.Version1.NewGuid();
            var guidBytes0 = guid0.ToByteArray();
            var clockSeq0 = (guidBytes0[8] << 8) | guidBytes0[9];
            clockSeq0 &= (ushort)(clockSeq0 & ~0xC000);
            var guid1 = GuidGenerator.Version1.NewGuid();
            var guidBytes1 = guid1.ToByteArray();
            var clockSeq1 = (guidBytes1[8] << 8) | guidBytes1[9];
            clockSeq1 = (ushort)(clockSeq1 & ~0xC000);
            var expected = (ushort)((clockSeq0 + 1) & ~0xC000);
            Assert.AreEqual(expected, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithRfc4122Variant()
        {
            var guid = GuidGenerator.Version1.NewGuid();
            Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version2_GetExpectedTimestamp()
        {
            var domain = DceSecurityDomain.Person;
            var guid = GuidGenerator.Version2.NewGuid(domain);
            var hasTs = guid.TryGetTimestamp(out var timestamp);
            Assert.IsTrue(hasTs);
            var ticksTs = timestamp.Ticks;
            var ticksNow = DateTime.UtcNow.Ticks;
            var ticksDiff = ticksNow - ticksTs;
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
        public void NewGuid_Version2_GetIncrementClockSeq()
        {
            var domain = DceSecurityDomain.Person;
            var guid0 = GuidGenerator.Version2.NewGuid(domain);
            var guidBytes0 = guid0.ToByteArray();
            var clockSeq0 = guidBytes0[8];
            clockSeq0 = (byte)(clockSeq0 & ~0xC0);
            var guid1 = GuidGenerator.Version2.NewGuid(domain);
            var guidBytes1 = guid1.ToByteArray();
            var clockSeq1 = guidBytes1[8];
            clockSeq1 = (byte)(clockSeq1 & ~0xC0);
            var expected = (byte)((clockSeq0 + 1) & ~0xC0);
            Assert.AreEqual(expected, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version2_GetGuidWithRfc4122Variant()
        {
            var domain = DceSecurityDomain.Person;
            var guid = GuidGenerator.Version2.NewGuid(domain);
            Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version2InvalidDomain_CatchArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => GuidGenerator.Version2.NewGuid((DceSecurityDomain)(-1)));
        }

        [TestMethod]
        public void NewGuid_Version3_GetExpectedGuid()
        {
            var nsId = GuidNamespaces.Url;
            var name = "https://github.com/x-stars/GuidGenerator";
            var guid = GuidGenerator.Version3.NewGuid(nsId, name);
            var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
            Assert.AreEqual(expected, guid);
        }

        [TestMethod]
        public void NewGuid_Version3NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.Version3.NewGuid(Guid.Empty, null!));
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithVersion4()
        {
            var guid = GuidGenerator.Version4.NewGuid();
            Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithRfc4122Variant()
        {
            var guid = GuidGenerator.Version4.NewGuid();
            Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
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

        [TestMethod]
        public void NewGuid_Version5NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.Version5.NewGuid(Guid.Empty, null!));
        }

        [TestMethod]
        public void NewGuid_InvalidVersionEnum_CatchArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => GuidGenerator.NewGuid((GuidVersion)(-1)));
        }
    }
}