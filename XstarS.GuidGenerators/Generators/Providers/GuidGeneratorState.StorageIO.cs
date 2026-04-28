using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorState
{
#if DEBUG
    /// <remarks>
    /// Any reference to members of this type must be
    /// in the <see cref="GuidGeneratorState.StorageLock"/> scope.
    /// </remarks>
#endif
    private static class BinaryBuffer
    {
        private const int Size = 4 + 4 + 8 + 4 + 6 + 6;

        internal static readonly byte[] Value = new byte[BinaryBuffer.Size];
        internal static readonly MemoryStream Stream = new(BinaryBuffer.Value);
        internal static readonly BinaryReader Reader = new(BinaryBuffer.Stream);
        internal static readonly BinaryWriter Writer = new(BinaryBuffer.Stream);

        internal static void Reset() => BinaryBuffer.Stream.Position = 0L;
    }

    private static bool LoadFromStorage()
    {
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Read))
            {
                var buffer = BinaryBuffer.Value;
                var length = stream.Read(buffer, 0, buffer.Length);
                if (length != buffer.Length)
                {
                    throw new EndOfStreamException();
                }
            }
            GuidGeneratorState.LoadFromBuffer();
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Read));
            return false;
        }
    }

    private static async Task<bool> LoadFromStorageAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Read))
            {
                var buffer = BinaryBuffer.Value;
                var length =
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    await stream.ReadAsync(buffer, cancellationToken)
#else
                    await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
#endif
                        .ConfigureAwait(continueOnCapturedContext: false);
                if (length != buffer.Length)
                {
                    throw new EndOfStreamException();
                }
            }
            GuidGeneratorState.LoadFromBuffer();
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Read));
            return false;
        }
    }

    private static void LoadFromBuffer()
    {
        BinaryBuffer.Reset();
        var reader = BinaryBuffer.Reader;
        const int nodeIdSize = 6;
        var version = reader.ReadInt32();
        if (version != GuidGeneratorState.VersionNumber)
        {
            throw new InvalidDataException($"Unknown version number: {version}.");
        }
        var fieldFlags = reader.ReadInt32();
        var timestamp = reader.ReadInt64();
        var clockSeq = reader.ReadInt32();
        var phyNodeId = reader.ReadBytes(nodeIdSize);
        var randNodeId = reader.ReadBytes(nodeIdSize);
        var isIndClkSeq = (fieldFlags >> (2 * 8)) != 0;

        var phyState = GuidGeneratorState.PhysicalNodeState;
        var randState = GuidGeneratorState.RandomNodeState;
        foreach (var state in new[] { phyState, randState })
        {
            var isRandom = state.NodeIdSource.IsRandomValue();
            var clkSeqFlag = (isRandom ? 0x02 : 0x01) << (2 * 8);
            var clkSeqShift = isRandom ? (2 * 8) : (0 * 8);
            var nodeIdFlag = isRandom ? 0x08 : 0x04;
            var nodeIdBytes = isRandom ? randNodeId : phyNodeId;
            lock (state)
            {
                state.Reset();
                if ((fieldFlags & 0x01) == 0x01)
                {
                    state.LastTimestamp = timestamp;
                }
                if (!isIndClkSeq && ((fieldFlags & 0x02) == 0x02))
                {
                    state.ClockSequence = clockSeq;
                }
                if (isIndClkSeq && ((fieldFlags & clkSeqFlag) == clkSeqFlag))
                {
                    state.ClockSequence = (ushort)(clockSeq >> clkSeqShift);
                }
                if ((fieldFlags & nodeIdFlag) == nodeIdFlag)
                {
                    state.SetLastNodeId(nodeIdBytes);
                }
                state.IsRefreshed = false;
            }
        }
    }

    private static bool SaveToStorageCore()
    {
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            GuidGeneratorState.SaveToBuffer();
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Write))
            {
                var buffer = BinaryBuffer.Value;
                stream.Write(buffer, 0, buffer.Length);
            }
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Write));
            return false;
        }
    }

    private static async Task<bool> SaveToStorageAsyncCore(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            GuidGeneratorState.SaveToBuffer();
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Write))
            {
                var buffer = BinaryBuffer.Value;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                await stream.WriteAsync(buffer, cancellationToken)
#else
                await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken)
#endif
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Write));
            return false;
        }
    }

    private static void SaveToBuffer()
    {
        var fieldFlags = 0x01 | 0x02;
        var timestamp = default(long);
        var clockSeq = default(int);
        var phyNodeId = GuidGeneratorState.EmptyNodeId;
        var randNodeId = GuidGeneratorState.EmptyNodeId;
        lock (GuidGeneratorState.PhysicalNodeState)
        {
            lock (GuidGeneratorState.RandomNodeState)
            {
                fieldFlags |= (0x01 | 0x02) << (2 * 8);
                var phyState = GuidGeneratorState.PhysicalNodeState;
                var randState = GuidGeneratorState.RandomNodeState;
                timestamp = (!phyState.IsRefreshed && !randState.IsRefreshed) ?
                    Math.Max(phyState.LastTimestamp, randState.LastTimestamp) :
                    Math.Max(
                        phyState.IsRefreshed ? phyState.LastTimestamp : 0L,
                        randState.IsRefreshed ? randState.LastTimestamp : 0L);
                clockSeq = (int)(
                    ((uint)(ushort)phyState.ClockSequence << (0 * 8)) |
                    ((uint)(ushort)randState.ClockSequence << (2 * 8)));
                if (phyState.LastNodeIdBytes is not null)
                {
                    fieldFlags |= 0x04;
                    phyNodeId = phyState.LastNodeIdBytes;
                }
                if (randState.LastNodeIdBytes is not null)
                {
                    fieldFlags |= 0x08;
                    randNodeId = randState.LastNodeIdBytes;
                }
            }
        }

        BinaryBuffer.Reset();
        var writer = BinaryBuffer.Writer;
        const int nodeIdSize = 6;
        writer.Write(GuidGeneratorState.VersionNumber);
        writer.Write(fieldFlags);
        writer.Write(timestamp);
        writer.Write(clockSeq);
        writer.Write(phyNodeId, 0, nodeIdSize);
        writer.Write(randNodeId, 0, nodeIdSize);
    }
}
