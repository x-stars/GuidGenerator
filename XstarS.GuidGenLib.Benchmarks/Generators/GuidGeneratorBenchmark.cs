using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

using static GuidVersion;

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
        var guidGen = GuidGenerator.OfVersion(Empty);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV1Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version1);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV2Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version2);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version3);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV4Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version4);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version5);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV6Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version6);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV7Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version7);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void GuidV8Generate()
    {
        var guidGen = GuidGenerator.OfVersion(Version8);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

    [Benchmark]
    public void MaxValueGenerate()
    {
        var guidGen = GuidGenerator.OfVersion(MaxValue);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }
}
