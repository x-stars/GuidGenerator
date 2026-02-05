using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidUuidBytesBenchmark
{
    private readonly Guid GuidValue = new(
        0xffffffff, 0xffff, 0xffff,
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);

    private readonly byte[] GuidBytes =
    [
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    ];

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FromByteArray")]
    public void FromByteArray()
    {
        _ = new Guid(this.GuidBytes);
    }

    [Benchmark]
    [BenchmarkCategory("FromByteArray")]
    public void FromUuidByteArray()
    {
        _ = Guid.FromUuidByteArray(this.GuidBytes);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FromByteSpan")]
    public void FromByteSpan()
    {
        _ = new Guid((ReadOnlySpan<byte>)this.GuidBytes);
    }

    [Benchmark]
    [BenchmarkCategory("FromByteSpan")]
    public void FromUuidByteSpan()
    {
        _ = Guid.FromUuidBytes((ReadOnlySpan<byte>)this.GuidBytes);
    }
#endif

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("ToByteArray")]
    public void ToByteArray()
    {
        _ = this.GuidValue.ToByteArray();
    }

    [Benchmark]
    [BenchmarkCategory("ToByteArray")]
    public void ToUuidByteArray()
    {
        _ = this.GuidValue.ToUuidByteArray();
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("TryWriteByteSpan")]
    public void TryWriteByteSpan()
    {
        var bytes = (stackalloc byte[16]);
        _ = this.GuidValue.TryWriteBytes(bytes);
    }

    [Benchmark]
    [BenchmarkCategory("TryWriteByteSpan")]
    public void TryWriteUuidByteSpan()
    {
        var bytes = (stackalloc byte[16]);
        _ = this.GuidValue.TryWriteUuidBytes(bytes);
    }
#endif
}
