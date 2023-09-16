using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class NameBasedGuidGeneratorBenchmark
{
    [CLSCompliant(false)]
    [Params(0, 10, 1000, 1000000)]
    public int NameLength;

    private byte[] Name = Array.Empty<byte>();

    [GlobalSetup]
    public void Initialize()
    {
        var length = this.NameLength;
        var name = new byte[length];
        new Random().NextBytes(name);
        this.Name = name;
    }

    [Benchmark(Baseline = true)]
    public void SumNameBytes()
    {
        var sum = 0;
        var name = this.Name;
        foreach (var @byte in name)
        {
            sum += @byte;
        }
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        var guidGen = GuidGenerator.Version3;
        var guid = guidGen.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        var guidGen = GuidGenerator.Version5;
        var guid = guidGen.NewGuid(Guid.Empty, this.Name);
    }

#if !UUIDREV_DISABLE
    [Benchmark]
    public void GuidV8NSha256Generate()
    {
        var guidGen = GuidGenerator.Version8NSha256;
        var guid = guidGen.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha384Generate()
    {
        var guidGen = GuidGenerator.Version8NSha384;
        var guid = guidGen.NewGuid(Guid.Empty, this.Name);
    }

    [Benchmark]
    public void GuidV8NSha512Generate()
    {
        var guidGen = GuidGenerator.Version8NSha512;
        var guid = guidGen.NewGuid(Guid.Empty, this.Name);
    }
#endif
}
