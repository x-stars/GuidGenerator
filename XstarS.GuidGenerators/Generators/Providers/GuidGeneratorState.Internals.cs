using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace XNetEx.Guids.Generators;

static partial class GuidGeneratorState
{
    private const int VersionNumber = 4122;

    private static readonly byte[] EmptyNodeIdBytes = new byte[6];

    private static readonly object SyncRoot = GuidGeneratorState.SetSaveOnProcessExit();

    private static volatile byte[]? LastMacNodeIdBytes = null;

    private static volatile byte[]? LastRandomNodeIdBytes = null;

    private static long Volatile_LastTimestamp = 0L;

    private static volatile int ClockSequence = GuidGeneratorState.GetInitClockSequence();

    private static volatile bool LastLoadingResult = GuidGeneratorState.LoadFromStorage();

    private static readonly AutoRefreshCache<Task<bool>> LastSavingAsyncResultCache =
        new AutoRefreshCache<Task<bool>>(GuidGeneratorState.SaveToStorageAsync, 1 * 1000, 0);

    private static long LastTimestamp
    {
        get => Volatile.Read(ref GuidGeneratorState.Volatile_LastTimestamp);
        set => Volatile.Write(ref GuidGeneratorState.Volatile_LastTimestamp, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetInitClockSequence()
    {
        var newGuid = Guid.NewGuid();
        return (int)newGuid.TimeLow();
    }

    private static object SetSaveOnProcessExit()
    {
        AppDomain.CurrentDomain.ProcessExit +=
            (sender, e) => GuidGeneratorState.SaveToStorage();
        return new object();
    }

    private static Task<bool> SaveToStorageAsync()
    {
        return Task.Run(GuidGeneratorState.SaveToStorage);
    }

    internal static int RefreshState(byte[]? nodeId, long timestamp, int tsShift = 0)
    {
        lock (GuidGeneratorState.SyncRoot)
        {
            if (nodeId is not null)
            {
                var isRandomNodeId = ((nodeId[0] & 0x01) == 0x01);
                ref var lastIdField = ref (isRandomNodeId ?
                    ref GuidGeneratorState.LastRandomNodeIdBytes :
                    ref GuidGeneratorState.LastMacNodeIdBytes);
                var lastId = lastIdField;
                if ((nodeId != lastId) && (lastId is not null))
                {
                    if ((nodeId[0] != lastId[0]) || (nodeId[1] != lastId[1]) ||
                        (nodeId[2] != lastId[2]) || (nodeId[3] != lastId[3]) ||
                        (nodeId[4] != lastId[4]) || (nodeId[5] != lastId[5]))
                    {
                        GuidGeneratorState.ClockSequence =
                            GuidGeneratorState.GetInitClockSequence();
                    }
                }
                lastIdField = nodeId;
            }

            var lastTs = GuidGeneratorState.LastTimestamp;
            if ((timestamp >> tsShift) <= (lastTs >> tsShift))
            {
                GuidGeneratorState.ClockSequence++;
            }
            GuidGeneratorState.LastTimestamp = timestamp;

            _ = GuidGeneratorState.LastSavingAsyncResultCache.Value;
            return GuidGeneratorState.ClockSequence;
        }
    }

    private static bool LoadFromStorage()
    {
        var storageFilePath = GuidGeneratorState.StorageFilePath;
        if (storageFilePath is null) { return false; }

        try
        {
            using var stream = new FileStream(storageFilePath, FileMode.Open);
            using var reader = new BinaryReader(stream);
            var version = reader.ReadInt32();
            if (version != GuidGeneratorState.VersionNumber)
            {
                throw new InvalidDataException($"Unknown version number: {version}.");
            }
            var fieldFlags = reader.ReadInt32();
            var macNodeIdBytes = reader.ReadBytes(6);
            var randomNodeIdBytes = reader.ReadBytes(6);
            var lastTimestamp = reader.ReadInt64();
            var clockSequence = reader.ReadInt32();
            if ((fieldFlags & 0x01) == 0x01)
            {
                GuidGeneratorState.LastMacNodeIdBytes = macNodeIdBytes;
            }
            if ((fieldFlags & 0x02) == 0x02)
            {
                GuidGeneratorState.LastRandomNodeIdBytes = randomNodeIdBytes;
            }
            Debug.Assert((fieldFlags & 0x04) == 0x04);
            GuidGeneratorState.LastTimestamp = lastTimestamp;
            Debug.Assert((fieldFlags & 0x08) == 0x08);
            GuidGeneratorState.ClockSequence = clockSequence;
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStateStorageException(
                new StateStorageExceptionEventArgs(ex, storageFilePath, FileAccess.Read));
            return false;
        }
    }

    private static bool SaveToStorage()
    {
        var storageFilePath = GuidGeneratorState.StorageFilePath;
        if (storageFilePath is null) { return false; }

        try
        {
            using var stream = new FileStream(storageFilePath, FileMode.Create);
            using var writer = new BinaryWriter(stream);
            writer.Write(GuidGeneratorState.VersionNumber);
            var fieldFlags = 0x04 | 0x08;
            var macNodeIdBytes = GuidGeneratorState.LastMacNodeIdBytes;
            if (macNodeIdBytes is not null) { fieldFlags |= 0x01; }
            var randomNodeIdBytes = GuidGeneratorState.LastRandomNodeIdBytes;
            if (randomNodeIdBytes is not null) { fieldFlags |= 0x02; }
            var emptyNodeIdBytes = GuidGeneratorState.EmptyNodeIdBytes;
            writer.Write(fieldFlags);
            writer.Write(macNodeIdBytes ?? emptyNodeIdBytes, 0, 6);
            writer.Write(randomNodeIdBytes ?? emptyNodeIdBytes, 0, 6);
            writer.Write(GuidGeneratorState.LastTimestamp);
            writer.Write(GuidGeneratorState.ClockSequence);
            return true;
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStateStorageException(
                new StateStorageExceptionEventArgs(ex, storageFilePath, FileAccess.Write));
            return false;
        }
    }
}
