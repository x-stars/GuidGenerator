using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids.Generators;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidCreateBenchmark
{
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion4")]
    public void GuidNewGuid()
    {
        _ = Guid.NewGuid();
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion4")]
    public void GuidV4Generate()
    {
        _ = GuidGenerator.Version4.NewGuid();
    }

#if !UUIDREV_DISABLE
#if NET9_0_OR_GREATER
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion7")]
    public void GuidCreateVersion7()
    {
        _ = Guid.CreateVersion7();
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion7")]
    public void GuidV7Generate()
    {
        _ = GuidGenerator.Version7.NewGuid();
    }
#endif
#endif
}
