#if NET7_0_OR_GREATER
using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids;

public class GuidUInt128Benchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    private readonly Guid GuidValue = Guid.NewGuid();

    [Benchmark]
    public void FromUInt128()
    {
        var value = UInt128.MaxValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Uuid.FromUInt128(value);
        }
    }

    [Benchmark]
    public void ToUInt128()
    {
        var guid = this.GuidValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var value = guid.ToUInt128();
        }
    }
}
#endif
