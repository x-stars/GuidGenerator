#if !UUIDREV_DISABLE
using System;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides the <see langword="abstract"/> base class for a custom <see cref="GuidGenerator"/>.
/// </summary>
public abstract partial class CustomGuidGenerator : GuidGenerator
{
    private readonly DateTime EpochDateTime;

    private readonly TimestampProvider TimestampProvider;

    private readonly NodeIdProvider NodeIdProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomGuidGenerator"/> class.
    /// </summary>
    protected CustomGuidGenerator()
        : this(default(DateTime), NodeIdSource.None)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomGuidGenerator"/> class
    /// with the specified epoch <see cref="DateTime"/> and <see cref="NodeIdSource"/>.
    /// </summary>
    /// <param name="epochDateTime">The epoch <see cref="DateTime"/>
    /// for <see cref="CustomGuidGenerator.GetCurrentTimestamp()"/>.</param>
    /// <param name="nodeIdSource">The <see cref="NodeIdSource"/>
    /// for <see cref="CustomGuidGenerator.GetNodeIdByte(int)"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="nodeIdSource"/> is not a valid <see cref="NodeIdSource"/> value.</exception>
    protected CustomGuidGenerator(
        DateTime epochDateTime = default, NodeIdSource nodeIdSource = default)
    {
        this.EpochDateTime = epochDateTime.ToUniversalTime();
        this.TimestampProvider = TimestampProvider.Instance;
        this.NodeIdProvider = NodeIdProvider.GetInstance(nodeIdSource);
    }

    /// <inheritdoc/>
    public override GuidVersion Version => GuidVersion.Version8;

    /// <summary>
    /// Gets the current timestamp in number of ticks (100 ns).
    /// </summary>
    /// <returns>The current timestamp in number of ticks (100 ns).</returns>
    protected long GetCurrentTimestamp()
    {
        return this.TimestampProvider.CurrentTimestamp - this.EpochDateTime.Ticks;
    }

    /// <summary>
    /// Gets the node ID byte at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the node ID byte.</param>
    /// <returns>The node ID byte at <paramref name="index"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0 or equal to or greater than 6.</exception>
    /// <exception cref="NotSupportedException">
    /// The input <see cref="NodeIdSource"/> is <see cref="NodeIdSource.None"/>.</exception>
    protected byte GetNodeIdByte(int index)
    {
        if ((uint)index >= 6)
        {
            throw new ArgumentOutOfRangeException(nameof(index),
                "Index for node ID must be non-negative and less than 6.");
        }

        return this.NodeIdProvider.NodeIdBytes[index];
    }

    /// <summary>
    /// Fills the elements of the specified byte array with node ID bytes.
    /// </summary>
    /// <param name="buffer">The byte array to be filled with node ID bytes.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// The length of <paramref name="buffer"/> is less than 6.</exception>
    /// <exception cref="NotSupportedException">
    /// The input <see cref="NodeIdSource"/> is <see cref="NodeIdSource.None"/>.</exception>
    protected void GetNodeIdBytes(byte[] buffer)
    {
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        this.GetNodeIdBytes((Span<byte>)buffer);
#else
        var nodeId = this.NodeIdProvider.NodeIdBytes;
        Buffer.BlockCopy(nodeId, 0, buffer, 0, 6);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Fills the elements of the specified byte span with node ID bytes.
    /// </summary>
    /// <param name="buffer">The byte span to be filled with node ID bytes.</param>
    /// <exception cref="ArgumentException">
    /// The length of <paramref name="buffer"/> is less than 6.</exception>
    /// <exception cref="NotSupportedException">
    /// The input <see cref="NodeIdSource"/> is <see cref="NodeIdSource.None"/>.</exception>
    protected void GetNodeIdBytes(Span<byte> buffer)
    {
        var nodeId = this.NodeIdProvider.NodeIdBytes;
        ((ReadOnlySpan<byte>)nodeId).CopyTo(buffer);
    }
#endif

    /// <summary>
    /// Gets a random 32-bit signed integer.
    /// </summary>
    /// <returns>A random 32-bit signed integer.</returns>
    protected int GetRandomInt32()
    {
        var newGuid = Guid.NewGuid();
        return (int)newGuid.TimeLow();
    }

    /// <summary>
    /// Gets a random 64-bit signed integer.
    /// </summary>
    /// <returns>A random 64-bit signed integer.</returns>
    protected long GetRandomInt64()
    {
        var newGuid = Guid.NewGuid();
        return (long)(
            ((ulong)newGuid.TimeLow() << (0 * 8)) |
            ((ulong)newGuid.TimeMid() << (4 * 8)) |
            ((ulong)newGuid.NodeId(0) << (6 * 8)) |
            ((ulong)newGuid.NodeId(1) << (7 * 8)));
    }
}
#endif
