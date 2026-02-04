using System;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Guids;

public class GuidComponentReplaceBenchmark
{
    private readonly Guid GuidValue =
        Guid.NewVersion2(DceSecurityDomain.Person);

    private readonly DateTime Timestamp =
        new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly byte[] GuidNodeId = new byte[6];

    [Benchmark]
    public void ReplaceVersion()
    {
        _ = this.GuidValue.ReplaceVersion(default(GuidVersion));
    }

    [Benchmark]
    public void ReplaceVariant()
    {
        _ = this.GuidValue.ReplaceVariant(default(GuidVariant));
    }

    [Benchmark]
    public void ReplaceTimestamp()
    {
        _ = this.GuidValue.ReplaceTimestamp(this.Timestamp);
    }

    [Benchmark]
    public void ReplaceClockSequence()
    {
        _ = this.GuidValue.ReplaceClockSequence(default(short));
    }

    [Benchmark]
    public void ReplaceDomainAndLocalId()
    {
        _ = this.GuidValue.ReplaceDomainAndLocalId(
            default(DceSecurityDomain), default(int));
    }

    [Benchmark]
    public void ReplaceNodeId()
    {
        _ = this.GuidValue.ReplaceNodeId(this.GuidNodeId);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    [Benchmark]
    public void ReplaceNodeIdSpan()
    {
        _ = this.GuidValue.ReplaceNodeId(
            (ReadOnlySpan<byte>)this.GuidNodeId);
    }
#endif
}
