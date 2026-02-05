#if NET7_0_OR_GREATER
using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids;

public class GuidUInt128Benchmark
{
    private readonly Guid GuidValue = Guid.NewGuid();

    [Benchmark]
    public void FromUInt128()
    {
        _ = Guid.FromUInt128(UInt128.MaxValue);
    }

    [Benchmark]
    public void ToUInt128()
    {
        _ = this.GuidValue.ToUInt128();
    }
}
#endif
