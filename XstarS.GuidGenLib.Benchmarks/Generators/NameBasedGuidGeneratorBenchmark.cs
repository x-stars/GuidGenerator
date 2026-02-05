using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class NameBasedGuidGeneratorBenchmark
{
    [CLSCompliant(false)]
    [Params(0, 10, 1000, 1000000)]
    public int NameLength;

    private byte[] Name = [];

    [GlobalSetup]
    public void Initialize()
    {
        var length = this.NameLength;
        var name = new byte[length];
        new Random().NextBytes(name);
        this.Name = name;
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        _ = GuidGenerator.Version3.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        _ = GuidGenerator.Version5.NewGuid(Guid.Empty, this.Name);
    }

#if !UUIDREV_DISABLE
    [Benchmark]
    public void GuidV8NSha256Generate()
    {
        _ = GuidGenerator.Version8NSha256.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha384Generate()
    {
        _ = GuidGenerator.Version8NSha384.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha512Generate()
    {
        _ = GuidGenerator.Version8NSha512.NewGuid(Guid.Empty, this.Name);
    }

#if NET8_0_OR_GREATER
    [Benchmark]
    public void GuidV8NSha3D256Generate()
    {
        _ = GuidGenerator.Version8NSha3D256.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha3D384Generate()
    {
        _ = GuidGenerator.Version8NSha3D384.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha3D512Generate()
    {
        _ = GuidGenerator.Version8NSha3D512.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NShake128Generate()
    {
        _ = GuidGenerator.Version8NShake128.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NShake256Generate()
    {
        _ = GuidGenerator.Version8NShake256.NewGuid(Guid.Empty, this.Name);
    }
#endif
#endif
}
