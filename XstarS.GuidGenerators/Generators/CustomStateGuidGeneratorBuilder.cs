using System;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides a mechanism for building <see cref="IGuidGenerator"/> using custom state providers.
/// </summary>
public readonly struct CustomStateGuidGeneratorBuilder
{
    /// <summary>
    /// Gets the <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>The <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 4122 UUID version 1.</returns>
    public static CustomStateGuidGeneratorBuilder Version1 =>
        new(GuidVersion.Version1) { NodeIdSourceType = NodeIdSource.PhysicalAddress };

#if !UUIDREV_DISABLE
    /// <summary>
    /// Gets the <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 9562 UUID version 6.
    /// </summary>
    /// <returns>The <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 9562 UUID version 6.</returns>
    public static CustomStateGuidGeneratorBuilder Version6 =>
        new(GuidVersion.Version6) { NodeIdSourceType = NodeIdSource.None };

    /// <summary>
    /// Gets the <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 9562 UUID version 7.
    /// </summary>
    /// <returns>The <see cref="CustomStateGuidGeneratorBuilder"/> instance of RFC 9562 UUID version 7.</returns>
    public static CustomStateGuidGeneratorBuilder Version7 =>
        new(GuidVersion.Version7) { NodeIdSourceType = NodeIdSource.None };
#endif

    /// <summary>
    /// Gets the version of the <see cref="IGuidGenerator"/> to build of this instance.
    /// </summary>
    /// <returns>The version of the <see cref="IGuidGenerator"/> to build of this instance.</returns>
    public GuidVersion Version { get; }

    /// <summary>
    /// Gets or initializes the initial clock sequence to use
    /// of the <see cref="IGuidGenerator"/> building by this instance.
    /// </summary>
    /// <returns>The initial clock sequence to use
    /// of the <see cref="IGuidGenerator"/> building by this instance;
    /// or <see langword="null"/> to use the default clock sequence.</returns>
    private short? ClockSequence { get; init; }

    /// <summary>
    /// Gets or initializes the <see cref="NodeIdSource"/> type to use
    /// of the <see cref="IGuidGenerator"/> building by this instance.
    /// </summary>
    /// <returns>The <see cref="NodeIdSource"/> type to use
    /// of the <see cref="IGuidGenerator"/> building by this instance.</returns>
    private NodeIdSource NodeIdSourceType { get; init; }

    /// <summary>
    /// Gets or initializes the custom timestamp provider function to use
    /// of the <see cref="IGuidGenerator"/> building by this instance.
    /// </summary>
    /// <returns>The custom timestamp provider function to use
    /// of the <see cref="IGuidGenerator"/> building by this instance;
    /// or <see langword="null"/> to use the default timestamp provider.</returns>
    private Func<DateTime>? TimestampProvider { get; init; }

    /// <summary>
    /// Gets or initializes the custom node ID provider function to use
    /// of the <see cref="IGuidGenerator"/> building by this instance.
    /// </summary>
    /// <returns>The custom node ID provider function to use
    /// of the <see cref="IGuidGenerator"/> building by this instance;
    /// or <see langword="null"/> to use the default node ID provider.</returns>
    private Func<byte[]>? NodeIdProvider { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomStateGuidGeneratorBuilder"/> structure
    /// of the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    private CustomStateGuidGeneratorBuilder(GuidVersion version) : this()
    {
        this.Version = version;
    }

    /// <summary>
    /// Creates a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    /// <returns>The <see cref="CustomStateGuidGeneratorBuilder"/>
    /// instance of <paramref name="version"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version"/> does not support using custom states.</exception>
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

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified custom timestamp provider function.
    /// </summary>
    /// <param name="timestampProvider">The custom timestamp provider function to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="timestampProvider"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="timestampProvider"/> is <see langword="null"/>.</exception>
    public CustomStateGuidGeneratorBuilder UseTimestampProvider(Func<DateTime> timestampProvider)
    {
        return this with
        {
            TimestampProvider = timestampProvider ??
                throw new ArgumentNullException(nameof(timestampProvider)),
        };
    }

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified custom timestamp provider function.
    /// </summary>
    /// <param name="timestampProvider">The custom timestamp provider function to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="timestampProvider"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="timestampProvider"/> is <see langword="null"/>.</exception>
    public CustomStateGuidGeneratorBuilder UseTimestampProvider(Func<DateTimeOffset> timestampProvider)
    {
        return this.UseTimestampProvider((timestampProvider ??
            throw new ArgumentNullException(nameof(timestampProvider))).UtcDateTime);
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified custom <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">The custom <see cref="TimeProvider"/> to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="timeProvider"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="timeProvider"/> is <see langword="null"/>.</exception>
    public CustomStateGuidGeneratorBuilder UseTimeProvider(TimeProvider timeProvider)
    {
        return this.UseTimestampProvider((timeProvider ??
            throw new ArgumentNullException(nameof(timeProvider))).GetUtcNow);
    }
#endif

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified initial clock sequence.
    /// </summary>
    /// <param name="initClockSeq">The initial clock sequence to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="initClockSeq"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="initClockSeq"/> is negative or greater than 0x3FFF.</exception>
    public CustomStateGuidGeneratorBuilder UseClockSequence(short initClockSeq)
    {
        if ((ushort)initClockSeq > 0x3FFF)
        {
            throw new ArgumentOutOfRangeException(nameof(initClockSeq),
                "Clock sequence for Guid must be between 0 and 0x3FFF.");
        }

        return this with { ClockSequence = initClockSeq };
    }

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified <see cref="NodeIdSource"/>.
    /// </summary>
    /// <param name="nodeIdSource">The <see cref="NodeIdSource"/> to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="nodeIdSource"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="nodeIdSource"/> is not a valid enum value.</exception>
    public CustomStateGuidGeneratorBuilder UseNodeIdSource(NodeIdSource nodeIdSource)
    {
        if (nodeIdSource is < NodeIdSource.None or > NodeIdSource.NonVolatileRandom)
        {
            throw new ArgumentOutOfRangeException(nameof(nodeIdSource));
        }

        return this with { NodeIdSourceType = nodeIdSource, NodeIdProvider = null };
    }

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified custom node ID provider function.
    /// </summary>
    /// <param name="nodeIdProvider">The custom node ID provider function to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="nodeIdProvider"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="nodeIdProvider"/> is <see langword="null"/>.</exception>
    public CustomStateGuidGeneratorBuilder UseNodeIdProvider(Func<byte[]> nodeIdProvider)
    {
        return this with
        {
            NodeIdSourceType = NodeIdSource.VolatileRandom,
            NodeIdProvider = nodeIdProvider ??
                throw new ArgumentNullException(nameof(nodeIdProvider)),
        };
    }

    /// <summary>
    /// Returns a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using the specified custom node ID.
    /// </summary>
    /// <param name="nodeId">The custom node ID to use.</param>
    /// <returns>A new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the current state and using <paramref name="nodeId"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="nodeId"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="nodeId"/> is not 6 bytes long.</exception>
    public CustomStateGuidGeneratorBuilder UseNodeId(byte[] nodeId)
    {
        if (nodeId is null)
        {
            throw new ArgumentNullException(nameof(nodeId));
        }
        if (nodeId.Length != 6)
        {
            throw new ArgumentException(
                "Node ID for Guid must be exactly 6 bytes long.", nameof(nodeId));
        }

        return this.UseNodeIdProvider(nodeId.Identity);
    }

    /// <summary>
    /// Creates a new <see cref="IGuidGenerator"/> instance
    /// based on custom state providers used in this instance.
    /// </summary>
    /// <returns>A new <see cref="IGuidGenerator"/> instance
    /// based on custom state providers used in this instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// This instance is not initialized correctly.</exception>
    public IGuidGenerator ToGuidGenerator()
    {
        if (this.NodeIdSourceType is < NodeIdSource.None or > NodeIdSource.NonVolatileRandom)
        {
            throw new InvalidOperationException("This instance is not initialized correctly.");
        }
        if ((this.ClockSequence is short clockSeqValue) && ((ushort)clockSeqValue > 0x3FFF))
        {
            throw new InvalidOperationException("This instance is not initialized correctly.");
        }

        return this.Version switch
        {
            GuidVersion.Version1 => TimeBasedGuidGenerator.CreateCustomState(
                this.NodeIdSourceType, this.ClockSequence, this.TimestampProvider, this.NodeIdProvider),
#if !UUIDREV_DISABLE
            GuidVersion.Version6 => TimeBasedGuidGenerator.Sequential.CreateCustomState(
                this.NodeIdSourceType, this.ClockSequence, this.TimestampProvider, this.NodeIdProvider),
            GuidVersion.Version7 => UnixTimeBasedGuidGenerator.CreateCustomState(this.TimestampProvider),
#endif
            _ => throw new InvalidOperationException("This instance is not initialized correctly."),
        };
    }
}
