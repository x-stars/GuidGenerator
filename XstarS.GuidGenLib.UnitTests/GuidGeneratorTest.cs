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
            Assert.AreEqual(GuidVariant.NCS, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version1_GetExpectedTimestamp()
        {
            var guid = GuidGenerator.NewGuidV1();
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
            var guid = GuidGenerator.NewGuidV1();
            Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version1_GetIncrementClockSeq()
        {
            var guid0 = GuidGenerator.NewGuidV1();
            var guidBytes0 = guid0.ToByteArray();
            var clockSeq0 = (guidBytes0[8] << 8) | guidBytes0[9];
            clockSeq0 &= (ushort)(clockSeq0 & ~0xC000);
            var guid1 = GuidGenerator.NewGuidV1();
            var guidBytes1 = guid1.ToByteArray();
            var clockSeq1 = (guidBytes1[8] << 8) | guidBytes1[9];
            clockSeq1 = (ushort)(clockSeq1 & ~0xC000);
            var expected = (ushort)((clockSeq0 + 1) & ~0xC000);
            Assert.AreEqual(expected, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithRfc4122Variant()
        {
            var guid = GuidGenerator.NewGuidV1();
            Assert.AreEqual(GuidVariant.RFC4122, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version2_GetExpectedTimestamp()
        {
            var domain = DceSecurityDomain.Person;
            var guid = GuidGenerator.NewGuidV2(domain);
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
            var guid = GuidGenerator.NewGuidV2(domain);
            Assert.AreEqual(GuidVersion.Version2, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version2_GetInputDomainAndLocalID()
        {
            var domain = DceSecurityDomain.Org;
            var localID = 0x12345678;
            var guid = GuidGenerator.NewGuidV2(domain, localID);
            var hasLocalID = guid.TryGetDomainAndLocalID(
                out var guidDomain, out var guidLocalID);
            Assert.IsTrue(hasLocalID);
            Assert.AreEqual(domain, guidDomain);
            Assert.AreEqual(localID, guidLocalID);
        }

        [TestMethod]
        public void NewGuid_Version2_GetIncrementClockSeq()
        {
            var domain = DceSecurityDomain.Person;
            var guid0 = GuidGenerator.NewGuidV2(domain);
            var guidBytes0 = guid0.ToByteArray();
            var clockSeq0 = guidBytes0[8];
            clockSeq0 = (byte)(clockSeq0 & ~0xC0);
            var guid1 = GuidGenerator.NewGuidV2(domain);
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
            var guid = GuidGenerator.NewGuidV2(domain);
            Assert.AreEqual(GuidVariant.RFC4122, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version2InvalidDomain_CatchArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => GuidGenerator.NewGuidV2((DceSecurityDomain)(-1)));
        }

        [TestMethod]
        public void NewGuid_Version3_GetExpectedGuid()
        {
            var ns = GuidNamespaces.URL;
            var name = "https://github.com/x-stars/GuidGenerator";
            var guid = GuidGenerator.NewGuidV3(ns, name);
            var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
            Assert.AreEqual(expected, guid);
        }

        [TestMethod]
        public void NewGuid_Version3NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.NewGuidV3(Guid.Empty, null!));
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithVersion4()
        {
            var guid = GuidGenerator.NewGuidV4();
            Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithRfc4122Variant()
        {
            var guid = GuidGenerator.NewGuidV4();
            Assert.AreEqual(GuidVariant.RFC4122, guid.GetVariant());
        }

        [TestMethod]
        public void NewGuid_Version5_GetExpectedGuid()
        {
            var ns = GuidNamespaces.URL;
            var name = "https://github.com/x-stars/GuidGenerator";
            var guid = GuidGenerator.NewGuidV5(ns, name);
            var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
            Assert.AreEqual(expected, guid);
        }

        [TestMethod]
        public void NewGuid_Version5NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.NewGuidV5(Guid.Empty, null!));
        }

        [TestMethod]
        public void NewGuid_InvalidVersionEnum_CatchArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => GuidGenerator.NewGuid((GuidVersion)(-1)));
        }
    }
}