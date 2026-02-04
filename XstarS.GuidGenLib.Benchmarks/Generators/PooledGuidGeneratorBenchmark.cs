#if !UUIDREV_DISABLE
using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace XNetEx.Guids.Generators;

[CLSCompliant(false)]
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class PooledGuidGeneratorBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 1000, 1000000)]
    public int Concurrency;

    public readonly int GuidCount = 1000000;

    private void ParallelInvoke(Action<int> action)
    {
        Parallel.For(0, this.Concurrency, part =>
        {
            action.Invoke(this.GuidCount / this.Concurrency);
        });
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion1")]
    public void GuidV1Generate()
    {
        var guidGen = GuidGenerator.Version1R;
        this.ParallelInvoke(count =>
        {
            foreach (var index in ..count)
            {
                _ = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion1")]
    public void GuidV1PoolGenerate()
    {
        var guidGen = GuidGenerator.CreatePooled(
            GuidGenerator.CreateVersion1R);
        this.ParallelInvoke(count =>
        {
            foreach (var index in ..count)
            {
                _ = guidGen.NewGuid();
            }
        });
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GuidVersion6")]
    public void GuidV6Generate()
    {
        var guidGen = GuidGenerator.Version6R;
        this.ParallelInvoke(count =>
        {
            foreach (var index in ..count)
            {
                _ = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    [BenchmarkCategory("GuidVersion6")]
    public void GuidV6PoolGenerate()
    {
        var guidGen = GuidGenerator.CreatePooled(
            GuidGenerator.CreateVersion6R);
        this.ParallelInvoke(count =>
        {
            foreach (var index in ..count)
            {
                _ = guidGen.NewGuid();
            }
        });
    }
}
#endif
