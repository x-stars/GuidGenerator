using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !FEATURE_DISABLE_UUIDREV
using System.Security.Cryptography;
#endif

namespace XNetEx.Guids.Generators;

[TestClass]
public partial class GuidGeneratorTest
{
    [TestMethod]
    public void OfVersion_AllVersionBytes_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Empty, GuidGenerator.OfVersion((byte)0));
        Assert.AreSame(GuidGenerator.Version1, GuidGenerator.OfVersion(1));
        Assert.AreSame(GuidGenerator.Version2, GuidGenerator.OfVersion(2));
        Assert.AreSame(GuidGenerator.Version3, GuidGenerator.OfVersion(3));
        Assert.AreSame(GuidGenerator.Version4, GuidGenerator.OfVersion(4));
        Assert.AreSame(GuidGenerator.Version5, GuidGenerator.OfVersion(5));
#if !FEATURE_DISABLE_UUIDREV
        Assert.AreSame(GuidGenerator.Version6, GuidGenerator.OfVersion(6));
        Assert.AreSame(GuidGenerator.Version7, GuidGenerator.OfVersion(7));
        Assert.AreSame(GuidGenerator.Version8, GuidGenerator.OfVersion(8));
        Assert.AreSame(GuidGenerator.MaxValue, GuidGenerator.OfVersion(15));
#endif
    }

    [TestMethod]
    public void OfVersion_AllVersionEnums_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Empty,
                       GuidGenerator.OfVersion(GuidVersion.Empty));
        Assert.AreSame(GuidGenerator.Version1,
                       GuidGenerator.OfVersion(GuidVersion.Version1));
        Assert.AreSame(GuidGenerator.Version2,
                       GuidGenerator.OfVersion(GuidVersion.Version2));
        Assert.AreSame(GuidGenerator.Version3,
                       GuidGenerator.OfVersion(GuidVersion.Version3));
        Assert.AreSame(GuidGenerator.Version4,
                       GuidGenerator.OfVersion(GuidVersion.Version4));
        Assert.AreSame(GuidGenerator.Version5,
                       GuidGenerator.OfVersion(GuidVersion.Version5));
#if !FEATURE_DISABLE_UUIDREV
        Assert.AreSame(GuidGenerator.Version6,
                       GuidGenerator.OfVersion(GuidVersion.Version6));
        Assert.AreSame(GuidGenerator.Version7,
                       GuidGenerator.OfVersion(GuidVersion.Version7));
        Assert.AreSame(GuidGenerator.Version8,
                       GuidGenerator.OfVersion(GuidVersion.Version8));
        Assert.AreSame(GuidGenerator.MaxValue,
                       GuidGenerator.OfVersion(GuidVersion.MaxValue));
#endif
    }

    [TestMethod]
    public void OfVersion_InvalidVersionByte_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfVersion(0xFF));
    }

    [TestMethod]
    public void OfVersion_InvalidVersionEnum_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfVersion((GuidVersion)0xFF));
    }

#if !FEATURE_DISABLE_UUIDREV
    [TestMethod]
    public void OfHashAlgorithm_AllSupportedNameStrings_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Version3,
                       GuidGenerator.OfHashAlgorithm(nameof(MD5)));
        Assert.AreSame(GuidGenerator.Version5,
                       GuidGenerator.OfHashAlgorithm(nameof(SHA1)));
        Assert.AreSame(GuidGenerator.Version8NSha256,
                       GuidGenerator.OfHashAlgorithm(nameof(SHA256)));
        Assert.AreSame(GuidGenerator.Version8NSha384,
                       GuidGenerator.OfHashAlgorithm(nameof(SHA384)));
        Assert.AreSame(GuidGenerator.Version8NSha512,
                       GuidGenerator.OfHashAlgorithm(nameof(SHA512)));
    }

    [TestMethod]
    public void OfHashAlgorithm_AllSupportedNameObjects_GetSameInstanceOfVersion()
    {
        Assert.AreSame(GuidGenerator.Version3,
                       GuidGenerator.OfHashAlgorithm(HashAlgorithmName.MD5));
        Assert.AreSame(GuidGenerator.Version5,
                       GuidGenerator.OfHashAlgorithm(HashAlgorithmName.SHA1));
        Assert.AreSame(GuidGenerator.Version8NSha256,
                       GuidGenerator.OfHashAlgorithm(HashAlgorithmName.SHA256));
        Assert.AreSame(GuidGenerator.Version8NSha384,
                       GuidGenerator.OfHashAlgorithm(HashAlgorithmName.SHA384));
        Assert.AreSame(GuidGenerator.Version8NSha512,
                       GuidGenerator.OfHashAlgorithm(HashAlgorithmName.SHA512));
    }

    [TestMethod]
    public void OfHashAlgorithm_NullNameString_CatchArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.OfHashAlgorithm(null!));
    }

    [TestMethod]
    public void OfHashAlgorithm_InvalidNameStrings_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfHashAlgorithm(string.Empty));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfHashAlgorithm("SHA-512/256"));
    }

    [TestMethod]
    public void OfHashAlgorithm_InvalidNameObjects_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfHashAlgorithm(default(HashAlgorithmName)));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.OfHashAlgorithm(new HashAlgorithmName("SHA-512/256")));
    }

    [TestMethod]
    public void CreatePooled_BySpecifiedVersion_GetSameVersionAndVariant()
    {
        var factory = GuidGenerator.CreateVersion1R;
        var guidGen = factory.Invoke();
        using var guidGenPool = GuidGenerator.CreatePooled(factory);
        Assert.AreEqual(guidGen.Version, guidGenPool.Version);
        Assert.AreEqual(guidGen.Variant, guidGenPool.Variant);
    }

    [TestMethod]
    public void CreatePooled_NullGeneratorFactory_CatchArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => GuidGenerator.CreatePooled(null!));
    }

    [TestMethod]
    public void CreatePooled_InvalidPoolCapacity_CatchArgumentOutOfRangeException()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.CreatePooled(GuidGenerator.CreateVersion1R, 0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => GuidGenerator.CreatePooled(GuidGenerator.CreateVersion1R, int.MinValue));
    }
#endif
}
