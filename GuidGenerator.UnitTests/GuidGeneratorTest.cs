using System;
using System.ComponentModel;
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
        public void NewGuid_InvalidVersionEnum_CatchInvalidEnumArgumentException()
        {
            Assert.ThrowsException<InvalidEnumArgumentException>(
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
            var clockSeq0 = BitConverter.ToInt32(guidBytes0, 8);
            clockSeq0 &= ~((int)0xC0 << (3 * 8));
            var guid1 = GuidGenerator.NewGuid(GuidVersion.Version1);
            var guidBytes1 = guid1.ToByteArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(guidBytes1, 8, 2);
            }
            var clockSeq1 = BitConverter.ToInt32(guidBytes1, 8);
            clockSeq1 &= ~((int)0xC0 << (3 * 8));
            Assert.AreEqual(clockSeq0 + 1, clockSeq1);
        }

        [TestMethod]
        public void NewGuid_Version1_GetGuidWithReservedBits10()
        {
            var guid = GuidGenerator.NewGuid(GuidVersion.Version1);
            var reserved = guid.ToByteArray()[8] & 0xC0;
            Assert.AreEqual(0x80, reserved);
        }

        [TestMethod]
        public void NewGuid_Version2_CatchNotImplementedException()
        {
            Assert.ThrowsException<NotImplementedException>(
                () => GuidGenerator.NewGuid(GuidVersion.Version2));
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