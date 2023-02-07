﻿using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids.Generators;

using static GuidVersion;

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
            var guidGen = GuidGenerator.OfVersion(Empty);
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
            var guidGen = GuidGenerator.OfVersion(Version1);
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
            var guidGen = GuidGenerator.OfVersion(Version2);
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
            var guidGen = GuidGenerator.OfVersion(Version3);
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
            var guidGen = GuidGenerator.OfVersion(Version4);
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
            var guidGen = GuidGenerator.OfVersion(Version5);
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
            var guidGen = GuidGenerator.OfVersion(Version6);
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
            var guidGen = GuidGenerator.OfVersion(Version7);
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
            var guidGen = GuidGenerator.OfVersion(Version8);
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
            var guidGen = GuidGenerator.OfVersion(MaxValue);
            for (int index = 0; index < count; index++)
            {
                var guid = guidGen.NewGuid();
            }
        });
    }
}
