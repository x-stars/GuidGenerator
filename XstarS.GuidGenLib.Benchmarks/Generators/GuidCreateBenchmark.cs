using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids.Generators;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidCreateBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion4")]
    public void GuidNewGuid()
    {
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Guid.NewGuid();
        }
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion4")]
    public void GuidV4Generate()
    {
        var guidGen = GuidGenerator.OfVersion(4);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }

#if !UUIDREV_DISABLE
#if NET9_0_OR_GREATER
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion7")]
    public void GuidCreateVersion7()
    {
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Guid.CreateVersion7();
        }
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion7")]
    public void GuidV7Generate()
    {
        var guidGen = GuidGenerator.OfVersion(7);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = guidGen.NewGuid();
        }
    }
#endif
#endif
}
