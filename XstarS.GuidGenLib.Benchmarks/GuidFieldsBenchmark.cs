using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids;

public class GuidFieldsBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

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

    [Benchmark]
    public void DeconstructToFields()
    {
        var guid = Uuid.MaxValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var (a, b, c, d, e, f, g, h, i, j, k) = guid;
        }
    }
}
