using System;
using BenchmarkDotNet.Attributes;
using XNetEx.Guids.Generators;

namespace XNetEx.Guids;

public class GuidComponentBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    private Guid[] GuidValues = [];

    [GlobalSetup]
    public void PrepareGuidValues()
    {
        var count = this.GuidCount;
        var guids = new Guid[count];
        var guidGen = GuidGenerator.Version2;
        foreach (var index in ..count)
        {
            var domain = (DceSecurityDomain)(index % 2);
            guids[index] = guidGen.NewGuid(domain);
        }
        this.GuidValues = guids;
    }

    [Benchmark]
    public void GetVersion()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            var version = guid.GetVersion();
        }
    }

    [Benchmark]
    public void GetVariant()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            var variant = guid.GetVariant();
        }
    }

    [Benchmark]
    public void TryGetTimestamp()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.TryGetTimestamp(out var timestamp);
        }
    }

    [Benchmark]
    public void TryGetClockSequence()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.TryGetClockSequence(out var clockSeq);
        }
    }

    [Benchmark]
    public void TryGetDomainAndLocalId()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.TryGetDomainAndLocalId(
                out var domain, out var localId);
        }
    }

    [Benchmark]
    public void TryGetNodeId()
    {
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.TryGetNodeId(out var nodeId);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark]
    public void TryWriteNodeId()
    {
        var nodeId = (stackalloc byte[6]);
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.TryWriteNodeId(nodeId);
        }
    }
#endif
}
