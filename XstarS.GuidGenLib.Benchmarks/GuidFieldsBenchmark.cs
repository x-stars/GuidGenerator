using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidFieldsBenchmark
{
    private readonly Guid GuidValue = new(
        0xffffffff, 0xffff, 0xffff,
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);

    private readonly byte[] GuidLowerBytes =
        [0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff];

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ByFieldValues")]
    public void ConstructFromFields()
    {
        _ = new Guid(0xffffffff, 0xffff, 0xffff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);
    }

    [Benchmark]
    [BenchmarkCategory("ByFieldValues")]
    public void DeconstructToFields()
    {
        var (a, b, c, d, e, f, g, h, i, j, k) = this.GuidValue;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ByFieldValuesAndArray")]
    public void ConstructFromFieldsAndArray()
    {
        _ = new Guid(-1, -1, -1, this.GuidLowerBytes);
    }

    [Benchmark]
    [BenchmarkCategory("ByFieldValuesAndArray")]
    public void DeconstructToFieldsAndArray()
    {
        var (a, b, c, d) = this.GuidValue;
    }
}
