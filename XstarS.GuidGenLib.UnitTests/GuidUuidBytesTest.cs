using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public class GuidUuidBytesTest
{
#if !UUIDREV_DISABLE
    [TestMethod]
    public void GuidMaxValue_ToUuidByteArray_GetAllBytesMaxValue()
    {
        var guid = Uuid.MaxValue;
        foreach (var item in guid.ToUuidByteArray())
        {
            Assert.AreEqual(byte.MaxValue, item);
        }
    }
#endif

    [TestMethod]
    public void FromUuidByteArray_ToByteArray_GetReversedByteOrderFields()
    {
        var uuidBytes = new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var guid = Uuid.FromByteArray(uuidBytes);
        var expected = new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var guidBytes = guid.ToByteArray();
        CollectionAssert.AreEqual(expected, guidBytes);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void FromUuidBytes_TryWriteBytes_GetReversedByteOrderFields()
    {
        var uuidBytes = (ReadOnlySpan<byte>)(new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        });
        var guid = Uuid.FromBytes(uuidBytes);
        var guidBytes = (stackalloc byte[16]);
        var result = guid.TryWriteBytes(guidBytes);
        Assert.IsTrue(result);
        var expected = (ReadOnlySpan<byte>)(new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        });
        Assert.IsTrue(expected.SequenceEqual(guidBytes));
    }
#endif

    [TestMethod]
    public void ToUuidByteArray_NewByByteArray_GetReversedByteOrderFields()
    {
        var guidBytes = new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var guid = new Guid(guidBytes);
        var expected = new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        };
        var uuidBytes = guid.ToUuidByteArray();
        CollectionAssert.AreEqual(expected, uuidBytes);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [TestMethod]
    public void TryWriteUuidBytes_NewByByteSpan_GetReversedByteOrderFields()
    {
        var guidBytes = (ReadOnlySpan<byte>)(new byte[]
        {
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        });
        var guid = new Guid(guidBytes);
        var uuidBytes = (stackalloc byte[16]);
        var result = guid.TryWriteUuidBytes(uuidBytes);
        Assert.IsTrue(result);
        var expected = (ReadOnlySpan<byte>)(new byte[]
        {
            0x33, 0x22, 0x11, 0x00, 0x55, 0x44, 0x77, 0x66,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF,
        });
        Assert.IsTrue(expected.SequenceEqual(uuidBytes));
    }
#endif
}
