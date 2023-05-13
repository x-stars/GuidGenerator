#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
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
    public void GuidNewGuid()
    {
        this.ParallelInvoke(count =>
        {
            for (int index = 0; index < count; index++)
            {
                var guid = Guid.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV1Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.Version1R;
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV1PoolGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.CreatePooled(
                GuidGenerator.CreateVersion1R);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV6Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.Version6;
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV6PoolGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.CreatePooled(
                GuidGenerator.CreateVersion6);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }
}
#endif
