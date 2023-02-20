using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidFieldsBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    private readonly byte[] GuidLowerBytes =
        new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };

    [BenchmarkCategory("ByFieldValues")]
    [Benchmark(Baseline = true)]
    public void ConstructFromFields()
    {
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = new Guid(0xffffffff, 0xffff, 0xffff,
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);
        }
    }

    [BenchmarkCategory("ByFieldValues")]
    [Benchmark(Baseline = false)]
    public void DeconstructToFields()
    {
        var guid = Uuid.MaxValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var (a, b, c, d, e, f, g, h, i, j, k) = guid;
        }
    }

    [BenchmarkCategory("ByFieldValuesAndArray")]
    [Benchmark(Baseline = true)]
    public void ConstructFromFieldsAndArray()
    {
        var lower = this.GuidLowerBytes;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = new Guid(-1, -1, -1, lower);
        }
    }

    [BenchmarkCategory("ByFieldValuesAndArray")]
    [Benchmark(Baseline = false)]
    public void DeconstructToFieldsAndArray()
    {
        var guid = Uuid.MaxValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var (a, b, c, d) = guid;
        }
    }
}
