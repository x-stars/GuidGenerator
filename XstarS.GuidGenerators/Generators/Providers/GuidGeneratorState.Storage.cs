using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

using StreamProvider = Func<string, FileAccess, Stream>;

partial class GuidGeneratorState
{
    private const int VersionNumber = 4122;

    private static readonly byte[] EmptyNodeId = new byte[6];

    private static volatile string? StorageFileName = GuidGeneratorState.SetSaveOnProcessExit();

    private static volatile StreamProvider StreamProvider = GuidGeneratorState.OpenLocalFile;

    private static readonly SemaphoreSlim StorageLock = new(initialCount: 1, maxCount: 1);

    private static readonly AutoRefreshCache<Task<bool>> LastSavingAsyncResultCache =
        new(GuidGeneratorState.SaveToStorageAsync, refreshPeriod: 10 * 1000, sleepAfter: 0);

    public static string? StorageFile => GuidGeneratorState.StorageFileName;

    internal static byte[]? RandomNodeId => GuidGeneratorState.RandomNodeState.LastNodeIdBytes;

    public static event EventHandler<StateStorageExceptionEventArgs>? StorageException;

    public static bool SetStorageFile(
        string? fileName, StreamProvider? streamProvider = null)
    {
        GuidGeneratorState.StorageLock.Wait();
        try
        {
            GuidGeneratorState.StorageFileName = fileName;
            GuidGeneratorState.StreamProvider = streamProvider ??
                GuidGeneratorState.OpenLocalFile;
            return GuidGeneratorState.LoadFromStorage();
        }
        finally
        {
            GuidGeneratorState.StorageLock.Release();
        }
    }

    public static async Task<bool> SetStorageFileAsync(
        string? fileName, StreamProvider? streamProvider = null)
    {
        await GuidGeneratorState.StorageLock.WaitAsync()
            .ConfigureAwait(continueOnCapturedContext: false);
        try
        {
            GuidGeneratorState.StorageFileName = fileName;
            GuidGeneratorState.StreamProvider = streamProvider ??
                GuidGeneratorState.OpenLocalFile;
            return await GuidGeneratorState.LoadFromStorageAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            GuidGeneratorState.StorageLock.Release();
        }
    }

    public static void ResetGlobal()
    {
        lock (GuidGeneratorState.PhysicalNodeState)
        {
            NodeIdProvider.RefreshPhysicalAddress();
            GuidGeneratorState.PhysicalNodeState.Reset();
        }
        lock (GuidGeneratorState.RandomNodeState)
        {
            NodeIdProvider.ResetNonVolatileRandom();
            GuidGeneratorState.RandomNodeState.Reset();
        }
        _ = GuidGeneratorState.SaveToStorageAsync();
    }

    private static string? SetSaveOnProcessExit()
    {
        static void SaveToStorage(object? sender, EventArgs e) =>
            GuidGeneratorState.SaveToStorage();
        AppDomain.CurrentDomain.ProcessExit += SaveToStorage;
        return null;
    }

    private static void OnStorageException(StateStorageExceptionEventArgs e)
    {
        Volatile.Read(ref GuidGeneratorState.StorageException)?.Invoke(null, e);
    }

    private static Stream OpenLocalFile(string storageFile, FileAccess operationType)
    {
        return operationType switch
        {
            FileAccess.Read => new FileStream(storageFile,
                FileMode.Open, FileAccess.Read, FileShare.Read),
            FileAccess.Write => new FileStream(storageFile,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.None),
            _ => throw new ArgumentOutOfRangeException(nameof(operationType))
        };
    }

    /// <summary>
    /// NOTE: Any reference to members of this type must be
    /// in the <see cref="GuidGeneratorState.StorageLock"/> scope.
    /// </summary>
    private static class BinaryBuffer
    {
        private const int Size = 4 + 4 + 8 + 4 + 6 + 6;

        internal static readonly byte[] Value = new byte[BinaryBuffer.Size];
        internal static readonly MemoryStream Stream = new(BinaryBuffer.Value);
        internal static readonly BinaryReader Reader = new(BinaryBuffer.Stream);
        internal static readonly BinaryWriter Writer = new(BinaryBuffer.Stream);

        internal static void Reset() => BinaryBuffer.Stream.Position = 0;
    }

    private static bool LoadFromStorage()
    {
        return GuidGeneratorState.LoadFromStorageAsync()
            .ConfigureAwait(continueOnCapturedContext: false)
            .GetAwaiter().GetResult();
    }

    private static async Task<bool> LoadFromStorageAsync()
    {
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Read))
            {
                var buffer = BinaryBuffer.Value;
                var length = await stream.ReadAsync(buffer, 0, buffer.Length)
                    .ConfigureAwait(continueOnCapturedContext: false);
                if (length != buffer.Length)
                {
                    throw new EndOfStreamException();
                }
            }
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
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Read));
            return false;
        }
    }

    private static bool SaveToStorage()
    {
        return GuidGeneratorState.SaveToStorageAsync()
            .ConfigureAwait(continueOnCapturedContext: false)
            .GetAwaiter().GetResult();
    }

    private static async Task<bool> SaveToStorageAsync()
    {
        await GuidGeneratorState.StorageLock.WaitAsync()
            .ConfigureAwait(continueOnCapturedContext: false);
        try
        {
            return await GuidGeneratorState.SaveToStorageAsyncCore()
                .ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            GuidGeneratorState.StorageLock.Release();
        }
    }

    private static async Task<bool> SaveToStorageAsyncCore()
    {
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
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
            var streamProvider = GuidGeneratorState.StreamProvider;
            using (var stream = streamProvider.Invoke(storageFile, FileAccess.Write))
            {
                var buffer = BinaryBuffer.Value;
                await stream.WriteAsync(buffer, 0, buffer.Length)
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
}
