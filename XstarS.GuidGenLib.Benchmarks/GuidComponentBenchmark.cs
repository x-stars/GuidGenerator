using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids;

public class GuidComponentBenchmark
{
    private readonly Guid GuidValue =
        Guid.NewVersion2(DceSecurityDomain.Person);


    [Benchmark]
    public void GetVersion()
    {
        _ = this.GuidValue.GetVersion();
    }

    [Benchmark]
    public void GetVariant()
    {
        _ = this.GuidValue.GetVariant();
    }

    [Benchmark]
    public void TryGetTimestamp()
    {
        _ = this.GuidValue.TryGetTimestamp(out var timestamp);

    }

    [Benchmark]
    public void TryGetClockSequence()
    {
        _ = this.GuidValue.TryGetClockSequence(out var clockSeq);
    }

    [Benchmark]
    public void TryGetDomainAndLocalId()
    {
        _ = this.GuidValue.TryGetDomainAndLocalId(
            out var domain, out var localId);
    }

    [Benchmark]
    public void TryGetNodeId()
    {
        _ = this.GuidValue.TryGetNodeId(out var nodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark]
    public void TryWriteNodeId()
    {
        var nodeId = (stackalloc byte[6]);
        _ = this.GuidValue.TryWriteNodeId(nodeId);
    }
#endif
}
