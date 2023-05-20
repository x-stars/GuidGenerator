#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class MonotonicGuidGeneratorBenchmark
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
    public void GuidV7LocalGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.Version7;
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV7GlobalGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.Version7M;
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }
}
#endif
