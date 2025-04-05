using System;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public readonly struct CustomStateGuidGeneratorBuilder
{
    public static CustomStateGuidGeneratorBuilder Version1 { get; } =
        new(GuidVersion.Version1) { NodeIdSource = NodeIdSource.PhysicalAddress };

#if !UUIDREV_DISABLE
    public static CustomStateGuidGeneratorBuilder Version6 { get; } =
        new(GuidVersion.Version6) { NodeIdSource = NodeIdSource.None };

    public static CustomStateGuidGeneratorBuilder Version7 { get; } =
        new(GuidVersion.Version7) { NodeIdSource = NodeIdSource.None };
#endif

    public GuidVersion Version { get; }

    private short? ClockSequence { get; init; }

    private NodeIdSource NodeIdSource { get; init; }

    private Func<DateTime>? TimestampProvider { get; init; }

    private Func<byte[]>? NodeIdProvider { get; init; }

    private CustomStateGuidGeneratorBuilder(GuidVersion version) : this()
    {
        this.Version = version;
    }

    public static CustomStateGuidGeneratorBuilder Create(GuidVersion version) => version switch
    {
        GuidVersion.Version1 => CustomStateGuidGeneratorBuilder.Version1,
#if !UUIDREV_DISABLE
        GuidVersion.Version6 => CustomStateGuidGeneratorBuilder.Version6,
        GuidVersion.Version7 => CustomStateGuidGeneratorBuilder.Version7,
#endif
        _ => throw new ArgumentOutOfRangeException(
            nameof(version), "This GUID version does not support using custom states."),
    };

    public CustomStateGuidGeneratorBuilder UseTimestampProvider(Func<DateTime> timestampProvider)
    {
        if (timestampProvider is null)
        {
            throw new ArgumentNullException(nameof(timestampProvider));
        }

        return this with { TimestampProvider = timestampProvider };
    }

    public CustomStateGuidGeneratorBuilder UseTimestampProvider(Func<DateTimeOffset> timestampProvider)
    {
        if (timestampProvider is null)
        {
            throw new ArgumentNullException(nameof(timestampProvider));
        }

        return this with { TimestampProvider = timestampProvider.UtcDateTime };
    }

#if NET8_0_OR_GREATER
    public CustomStateGuidGeneratorBuilder UseTimeProvider(TimeProvider timeProvider)
    {
        if (timeProvider is null)
        {
            throw new ArgumentNullException(nameof(timeProvider));
        }

        var timestampProvider = ((Func<DateTimeOffset>)timeProvider.GetUtcNow).UtcDateTime;
        return this with { TimestampProvider = timestampProvider };
    }
#endif

    public CustomStateGuidGeneratorBuilder UseClockSequence(short initClockSeq)
    {
        return this with { ClockSequence = initClockSeq };
    }

    public CustomStateGuidGeneratorBuilder UseNodeIdSource(NodeIdSource nodeIdSource)
    {
        if (nodeIdSource is < NodeIdSource.None or > NodeIdSource.NonVolatileRandom)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeIdSource));
        }

        return this with { NodeIdSource = nodeIdSource, NodeIdProvider = null };
    }

    public CustomStateGuidGeneratorBuilder UseNodeIdProvider(Func<byte[]> nodeIdProvider)
    {
        if (nodeIdProvider is null)
        {
            throw new ArgumentNullException(nameof(nodeIdProvider));
        }

        return this with { NodeIdSource = NodeIdSource.VolatileRandom, NodeIdProvider = nodeIdProvider, };
    }

    public CustomStateGuidGeneratorBuilder UseNodeIdBytes(byte[] nodeId)
    {
        if (nodeId is null)
        {
            throw new ArgumentNullException(nameof(nodeId));
        }
        if (nodeId.Length != 6)
        {
            throw new ArgumentException("Node ID for Guid must be exactly 6 bytes long.", nameof(nodeId));
        }

        return this with { NodeIdSource = NodeIdSource.VolatileRandom, NodeIdProvider = nodeId.Identity, };
    }

    public IGuidGenerator ToGuidGenerator()
    {
        if (NodeIdSource is < NodeIdSource.None or > NodeIdSource.NonVolatileRandom)
        {
            throw new InvalidOperationException("This instance is not initialized correctly.");
        }

        return this.Version switch
        {
            GuidVersion.Version1 => TimeBasedGuidGenerator.CreateCustomState(
                this.NodeIdSource, this.ClockSequence, this.TimestampProvider, this.NodeIdProvider),
#if !UUIDREV_DISABLE
            GuidVersion.Version6 => TimeBasedGuidGenerator.Sequential.CreateCustomState(
                this.NodeIdSource, this.ClockSequence, this.TimestampProvider, this.NodeIdProvider),
            GuidVersion.Version7 => UnixTimeBasedGuidGenerator.CreateCustomState(this.TimestampProvider),
#endif
            _ => throw new InvalidOperationException("This instance is not initialized correctly."),
        };
    }
}

internal static class TimestampFunc
{
    internal static DateTime UtcDateTime(this Func<DateTimeOffset> timeFunc)
    {
        return timeFunc.Invoke().UtcDateTime;
    }
}
