using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

[MemoryDiagnoser]
public class GuidGeneratorConcurrentBenchmark
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
    public void EmptyGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var empty = GuidVersion.Empty;
            var guidGen = GuidGenerator.OfVersion(empty);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV1Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(1);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV2Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(2);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV3Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(3);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV4Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(4);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV5Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(5);
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
            var guidGen = GuidGenerator.OfVersion(6);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV7Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(7);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void GuidV8Generate()
    {
        this.ParallelInvoke(count =>
        {
            var guidGen = GuidGenerator.OfVersion(8);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }

    [Benchmark]
    public void MaxValueGenerate()
    {
        this.ParallelInvoke(count =>
        {
            var maxValue = GuidVersion.MaxValue;
            var guidGen = GuidGenerator.OfVersion(maxValue);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }
}
