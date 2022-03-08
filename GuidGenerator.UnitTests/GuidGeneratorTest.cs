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
        public void NewGuid_InvalidVersionEnum_CatchArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => GuidGenerator.NewGuid((GuidVersion)(-1)));
        }

        [TestMethod]
        public void NewGuid_Version1_GetExpectedTimestamp()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version1);
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
            var guid = GuidGenerator.NewGuid(GuidVersion.Version1);
            Assert.AreEqual(GuidVersion.Version1, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version1_GetIncrementClockSeq()
        {
            var guid0 = GuidGenerator.NewGuid(GuidVersion.Version1);
            var guidBytes0 = guid0.ToByteArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(guidBytes0, 8, 2);
            }
            var clockSeq0 = BitConverter.ToUInt16(guidBytes0, 8);
            clockSeq0 &= (ushort)(clockSeq0 & ~0xC000);
            var guid1 = GuidGenerator.NewGuid(GuidVersion.Version1);
            var guidBytes1 = guid1.ToByteArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(guidBytes1, 8, 2);
            }
            var clockSeq1 = BitConverter.ToUInt16(guidBytes1, 8);
            clockSeq1 = (ushort)(clockSeq1 & ~0xC000);
            var expected = (ushort)((clockSeq0 + 1) & ~0xC000);
            Assert.AreEqual(expected, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithVariantBits10()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version1);
            var reserved = guid.ToByteArray()[8] & 0xC0;
            Assert.AreEqual(0x80, reserved);
        }

        [TestMethod]
        public void NewGuid_Version2_GetExpectedTimestamp()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version2);
            var hasTs = guid.TryGetTimestamp(out var timestamp);
            Assert.IsTrue(hasTs);
            var ticksTs = timestamp.Ticks;
            var ticksNow = DateTime.UtcNow.Ticks;
            var ticksDiff = ticksNow - ticksTs;
            var ticks10m = TimeSpan.FromMinutes(10).Ticks;
            Assert.IsTrue(ticksDiff < ticks10m);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithVersion2()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version2);
            Assert.AreEqual(GuidVersion.Version2, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version2_GetIncrementClockSeq()
        {
            var guid0 = GuidGenerator.NewGuid(GuidVersion.Version2);
            var guidBytes0 = guid0.ToByteArray();
            var clockSeq0 = guidBytes0[8];
            clockSeq0 = (byte)(clockSeq0 & ~0xC0);
            var guid1 = GuidGenerator.NewGuid(GuidVersion.Version2);
            var guidBytes1 = guid1.ToByteArray();
            var clockSeq1 = guidBytes1[8];
            clockSeq1 = (byte)(clockSeq1 & ~0xC0);
            var expected = (byte)((clockSeq0 + 1) & ~0xC0);
            Assert.AreEqual(expected, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version2_GetGuidWithVariantBits10()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version2);
            var reserved = guid.ToByteArray()[8] & 0xC0;
            Assert.AreEqual(0x80, reserved);
        }

        [TestMethod]
        public void NewGuid_Version3_GetExpectedGuid()
        {
            var ns = GuidNamespaces.URL;
            var name = "https://github.com/x-stars/GuidGenerator";
            var guid = GuidGenerator.NewGuid(GuidVersion.Version3, ns, name);
            var expected = Guid.Parse("a9ec4420-7252-3c11-ab70-512e10273537");
            Assert.AreEqual(expected, guid);
        }

        [TestMethod]
        public void NewGuid_Version3NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.NewGuid(GuidVersion.Version3, Guid.Empty, null!));
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithVersion4()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version4);
            Assert.AreEqual(GuidVersion.Version4, guid.GetVersion());
        }

        [TestMethod]
        public void NewGuid_Version4_GetGuidWithReservedBits10()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version4);
            var reserved = guid.ToByteArray()[8] & 0xC0;
            Assert.AreEqual(0x80, reserved);
        }

        [TestMethod]
        public void NewGuid_Version5_GetExpectedGuid()
        {
            var ns = GuidNamespaces.URL;
            var name = "https://github.com/x-stars/GuidGenerator";
            var guid = GuidGenerator.NewGuid(GuidVersion.Version5, ns, name);
            var expected = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
            Assert.AreEqual(expected, guid);
        }

        [TestMethod]
        public void NewGuid_Version5NullName_CatchArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => GuidGenerator.NewGuid(GuidVersion.Version5, Guid.Empty, null!));
        }
    }
}