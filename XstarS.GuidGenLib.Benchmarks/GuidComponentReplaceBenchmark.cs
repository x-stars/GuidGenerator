using System;
using BenchmarkDotNet.Attributes;
using XNetEx.Guids.Generators;

namespace XNetEx.Guids;

public class GuidComponentReplaceBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int GuidCount;

    private Guid[] GuidValues = Array.Empty<Guid>();

    private readonly DateTime Timestamp =
        new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly byte[] GuidNodeId = new byte[6];

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
    public void ReplaceVersion()
    {
        var version = default(GuidVersion);
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceVersion(version);
        }
    }

    [Benchmark]
    public void ReplaceVariant()
    {
        var variant = default(GuidVariant);
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceVariant(variant);
        }
    }

    [Benchmark]
    public void ReplaceTimestamp()
    {
        var timestamp = this.Timestamp;
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceTimestamp(timestamp);
        }
    }

    [Benchmark]
    public void ReplaceClockSequence()
    {
        var clockSeq = default(short);
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceClockSequence(clockSeq);
        }
    }

    [Benchmark]
    public void ReplaceDomainAndLocalId()
    {
        var domain = default(DceSecurityDomain);
        var localId = default(int);
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceDomainAndLocalId(domain, localId);
        }
    }

    [Benchmark]
    public void ReplaceNodeId()
    {
        var nodeId = this.GuidNodeId;
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceNodeId(nodeId);
        }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark]
    public void ReplaceNodeIdSpan()
    {
        var nodeId = (Span<byte>)this.GuidNodeId;
        var guids = this.GuidValues;
        foreach (var guid in guids)
        {
            _ = guid.ReplaceNodeId(nodeId);
        }
    }
#endif
}
