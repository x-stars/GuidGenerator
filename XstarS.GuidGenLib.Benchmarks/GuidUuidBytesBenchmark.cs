using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids;

[CLSCompliant(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class GuidUuidBytesBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    private readonly Guid GuidValue = new Guid(
        -1, -1, -1, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff);

    private readonly byte[] GuidBytes = new byte[]
    {
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
    };

    [BenchmarkCategory("FromByteArray")]
    [Benchmark(Baseline = true)]
    public void FromByteArray()
    {
        var bytes = this.GuidBytes;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = new Guid(bytes);
        }
    }

    [BenchmarkCategory("FromByteArray")]
    [Benchmark(Baseline = false)]
    public void FromUuidByteArray()
    {
        var bytes = this.GuidBytes;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Uuid.FromByteArray(bytes);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [BenchmarkCategory("FromByteSpan")]
    [Benchmark(Baseline = true)]
    public void FromByteSpan()
    {
        var bytes = (ReadOnlySpan<byte>)this.GuidBytes;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = new Guid(bytes);
        }
    }

    [BenchmarkCategory("FromByteSpan")]
    [Benchmark(Baseline = false)]
    public void FromUuidByteSpan()
    {
        var bytes = (ReadOnlySpan<byte>)this.GuidBytes;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var guid = Uuid.FromBytes(bytes);
        }
    }
#endif

    [BenchmarkCategory("ToByteArray")]
    [Benchmark(Baseline = true)]
    public void ToByteArray()
    {
        var guid = this.GuidValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var bytes = guid.ToByteArray();
        }
    }

    [BenchmarkCategory("ToByteArray")]
    [Benchmark(Baseline = false)]
    public void ToUuidByteArray()
    {
        var guid = this.GuidValue;
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            var bytes = guid.ToUuidByteArray();
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [BenchmarkCategory("TryWriteByteSpan")]
    [Benchmark(Baseline = true)]
    public void TryWriteByteSpan()
    {
        var guid = this.GuidValue;
        var bytes = (stackalloc byte[16]);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            _ = guid.TryWriteBytes(bytes);
        }
    }

    [BenchmarkCategory("TryWriteByteSpan")]
    [Benchmark(Baseline = false)]
    public void TryWriteUuidByteSpan()
    {
        var guid = this.GuidValue;
        var bytes = (stackalloc byte[16]);
        var count = this.GuidCount;
        for (int index = 0; index < count; index++)
        {
            _ = guid.TryWriteUuidBytes(bytes);
        }
    }
#endif
}
