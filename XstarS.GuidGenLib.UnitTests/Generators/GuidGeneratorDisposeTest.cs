#if !UUIDREV_DISABLE
using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public class GuidGeneratorDisposeTest
{
    [TestMethod]
    public void NewGuid_SharedGenerator_KeepUsableAfterDisposed()
    {
        foreach (var guidGen in new[]
        {
            (GuidGenerator)GuidGenerator.Empty,
            (GuidGenerator)GuidGenerator.Version1,
            (GuidGenerator)GuidGenerator.Version2,
            (GuidGenerator)GuidGenerator.Version3,
            (GuidGenerator)GuidGenerator.Version4,
            (GuidGenerator)GuidGenerator.Version5,
            (GuidGenerator)GuidGenerator.Version6,
            (GuidGenerator)GuidGenerator.Version7,
            (GuidGenerator)GuidGenerator.Version8,
            (GuidGenerator)GuidGenerator.MaxValue,
            (GuidGenerator)GuidGenerator.Version1R,
            (GuidGenerator)GuidGenerator.Version6P,
            (GuidGenerator)GuidGenerator.Version6R,
            (GuidGenerator)GuidGenerator.Version8NSha256,
            (GuidGenerator)GuidGenerator.Version8NSha384,
            (GuidGenerator)GuidGenerator.Version8NSha512,
        }
#if NET8_0_OR_GREATER
        .Concat(
            !(SHA3_256.IsSupported && SHA3_384.IsSupported && SHA3_512.IsSupported &&
              Shake128.IsSupported && Shake256.IsSupported) ?
            Array.Empty<GuidGenerator>() :
            new[]
            {
                (GuidGenerator)GuidGenerator.Version8NSha3D256,
                (GuidGenerator)GuidGenerator.Version8NSha3D384,
                (GuidGenerator)GuidGenerator.Version8NSha3D512,
                (GuidGenerator)GuidGenerator.Version8NShake128,
                (GuidGenerator)GuidGenerator.Version8NShake256,
            })
#endif
        )
        {
            var guid0 = guidGen.NewGuid();
            guidGen.Dispose();
            var guid1 = guidGen.NewGuid();
            Assert.AreEqual(guid0.GetVersion(), guid1.GetVersion());
            guidGen.Dispose();
        }
    }

    [TestMethod]
    public void NewGuid_GeneratorOfVersion_KeepUsableAfterDisposed()
    {
        var versions = Enum.GetValues(typeof(GuidVersion));
        foreach (var version in (GuidVersion[])versions)
        {
            var guidGen = GuidGenerator.OfVersion(version);
            var guid0 = guidGen.NewGuid();
            guidGen.Dispose();
            var guid1 = guidGen.NewGuid();
            Assert.AreEqual(guid0.GetVersion(), guid1.GetVersion());
            guidGen.Dispose();
        }
    }

    [TestMethod]
    public void NewGuid_GeneratorOfHashAlgorithm_KeepUsableAfterDisposed()
    {
        foreach (var hashingName in new[]
        {
            HashAlgorithmName.MD5,
            HashAlgorithmName.SHA1,
            HashAlgorithmName.SHA256,
            HashAlgorithmName.SHA384,
            HashAlgorithmName.SHA512,
        })
        {
            var guidGen = GuidGenerator.OfHashAlgorithm(hashingName);
            var guid0 = guidGen.NewGuid(Guid.Empty, Array.Empty<byte>());
            guidGen.Dispose();
            var guid1 = guidGen.NewGuid(Guid.Empty, Array.Empty<byte>());
            Assert.AreEqual(guid0.GetVersion(), guid1.GetVersion());
            guidGen.Dispose();
        }
    }

    [TestMethod]
    public void NewGuid_CreatedTimeBasedGenerator_KeepUsableAfterDisposed()
    {
        foreach (var guidGen in new[]
        {
            GuidGenerator.CreateVersion1R(),
            GuidGenerator.CreateVersion6R(),
        })
        {
            var guid0 = guidGen.NewGuid();
            guidGen.Dispose();
            var guid1 = guidGen.NewGuid();
            Assert.AreEqual(guid0.GetVersion(), guid1.GetVersion());
            guidGen.Dispose();
        }
    }

    [TestMethod]
    public void NewGuid_PooledGenerator_ThrowExceptionAfterDisposed()
    {
        var factory = GuidGenerator.CreateVersion6R;
        var guidGen = GuidGenerator.CreatePooled(factory);
        _ = guidGen.NewGuid();
        guidGen.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(
            () => guidGen.NewGuid());
        guidGen.Dispose();
    }

    [TestMethod]
    public void GuidInfo_PooledGenerator_ThrowExceptionAfterDisposed()
    {
        var factory = GuidGenerator.CreateVersion6R;
        var guidGen = GuidGenerator.CreatePooled(factory);
        _ = guidGen.Version;
        _ = guidGen.Variant;
        guidGen.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(
            () => guidGen.Version);
        Assert.ThrowsException<ObjectDisposedException>(
            () => guidGen.Variant);
        guidGen.Dispose();
    }

    [TestMethod]
    public void NewGuid_NameBasedGeneratorByFactory_ThrowExceptionAfterDisposed()
    {
        var factory = (Func<HashAlgorithm>)SHA256.Create;
        var guidGen = GuidGenerator.CreateVersion8N(factory);
        _ = guidGen.NewGuid(Guid.Empty, Array.Empty<byte>());
        guidGen.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(
            () => guidGen.NewGuid(Guid.Empty, Array.Empty<byte>()));
        guidGen.Dispose();
    }

    [TestMethod]
    public void GuidInfo_NameBasedGeneratorByFactory_KeepUsableAfterDisposed()
    {
        var factory = (Func<HashAlgorithm>)SHA256.Create;
        var guidGen = GuidGenerator.CreateVersion8N(factory);
        var version0 = guidGen.Version;
        var variant0 = guidGen.Variant;
        guidGen.Dispose();
        var version1 = guidGen.Version;
        var variant1 = guidGen.Variant;
        Assert.AreEqual(version0, version1);
        Assert.AreEqual(variant0, variant1);
        guidGen.Dispose();
    }

    [TestMethod]
    public void NewGuid_NameBasedGeneratorByInstance_ThrowExceptionAfterDisposed()
    {
        using var hashing = SHA256.Create();
        var guidGen = GuidGenerator.CreateVersion8N(hashing);
        _ = guidGen.NewGuid(Guid.Empty, Array.Empty<byte>());
        guidGen.Dispose();
        Assert.ThrowsException<ObjectDisposedException>(
            () => guidGen.NewGuid(Guid.Empty, Array.Empty<byte>()));
        guidGen.Dispose();
    }

    [TestMethod]
    public void GuidInfo_NameBasedGeneratorByInstance_KeepUsableAfterDisposed()
    {
        using var hashing = SHA256.Create();
        var guidGen = GuidGenerator.CreateVersion8N(hashing);
        var version0 = guidGen.Version;
        var variant0 = guidGen.Variant;
        guidGen.Dispose();
        var version1 = guidGen.Version;
        var variant1 = guidGen.Variant;
        Assert.AreEqual(version0, version1);
        Assert.AreEqual(variant0, variant1);
        guidGen.Dispose();
    }
}
#endif
