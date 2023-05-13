using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class GuidGeneratorBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    [Benchmark(Baseline = true)]
    public void GuidNewGuid()
    {
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Guid.NewGuid();
        }
    }

    [Benchmark]
    public void EmptyGenerate()
    {
        var empty = GuidVersion.Empty;
        var guidGen = GuidGenerator.OfVersion(empty);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV1Generate()
    {
        var guidGen = GuidGenerator.OfVersion(1);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV2Generate()
    {
        var guidGen = GuidGenerator.OfVersion(2);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        var guidGen = GuidGenerator.OfVersion(3);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV4Generate()
    {
        var guidGen = GuidGenerator.OfVersion(4);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        var guidGen = GuidGenerator.OfVersion(5);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

#if !FEATURE_DISABLE_UUIDREV
    [Benchmark]
    public void GuidV6Generate()
    {
        var guidGen = GuidGenerator.OfVersion(6);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV7Generate()
    {
        var guidGen = GuidGenerator.OfVersion(7);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV8Generate()
    {
        var guidGen = GuidGenerator.OfVersion(8);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void MaxValueGenerate()
    {
        var maxValue = GuidVersion.MaxValue;
        var guidGen = GuidGenerator.OfVersion(maxValue);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }
#endif
}
