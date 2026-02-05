using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class GuidGeneratorBenchmark
{
    [Benchmark(Baseline = true)]
    public void GuidNewGuid()
    {
        _ = Guid.NewGuid();
    }

    [Benchmark]
    public void EmptyGenerate()
    {
        var empty = GuidVersion.Empty;
        _ = GuidGenerator.OfVersion(empty).NewGuid();
    }

    [Benchmark]
    public void GuidV1Generate()
    {
        _ = GuidGenerator.OfVersion(1).NewGuid();
    }

    [Benchmark]
    public void GuidV2Generate()
    {
        _ = GuidGenerator.OfVersion(2).NewGuid();
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        _ = GuidGenerator.OfVersion(3).NewGuid();
    }

    [Benchmark]
    public void GuidV4Generate()
    {
        _ = GuidGenerator.OfVersion(4).NewGuid();
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        _ = GuidGenerator.OfVersion(5).NewGuid();
    }

#if !UUIDREV_DISABLE
    [Benchmark]
    public void GuidV6Generate()
    {
        _ = GuidGenerator.OfVersion(6).NewGuid();
    }

    [Benchmark]
    public void GuidV7Generate()
    {
        _ = GuidGenerator.OfVersion(7).NewGuid();
    }

    [Benchmark]
    public void GuidV8Generate()
    {
        _ = GuidGenerator.OfVersion(8).NewGuid();
    }

    [Benchmark]
    public void MaxValueGenerate()
    {
        var maxValue = GuidVersion.MaxValue;
        _ = GuidGenerator.OfVersion(maxValue).NewGuid();
    }
#endif
}
