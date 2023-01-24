﻿using System;
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

    private static volatile byte[]? LastPhysicalNodeIdBytes = null;

    private static volatile byte[]? LastRandomNodeIdBytes = null;

    private static long Volatile_LastTimestamp = 0L;

    private static volatile int ClockSequence = GuidGeneratorState.GetInitClockSequence();

    private static readonly AutoRefreshCache<Task<bool>> LastSavingAsyncResultCache =
        new AutoRefreshCache<Task<bool>>(GuidGeneratorState.SaveToStorageAsync,
            refreshPeriod: 10 * 1000, sleepAfter: 0);

    internal static byte[]? RandomNodeIdBytes => GuidGeneratorState.LastRandomNodeIdBytes;

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

    internal static int RefreshState(byte[]? nodeId, long timestamp)
    {
        lock (GuidGeneratorState.SyncRoot)
        {
            var clockSeq = GuidGeneratorState.ClockSequence;
            if (nodeId is not null)
            {
                var isRandomNodeId = (nodeId[0] & 0x01) == 0x01;
                ref var lastNodeField = ref (isRandomNodeId ?
                    ref GuidGeneratorState.LastRandomNodeIdBytes :
                    ref GuidGeneratorState.LastPhysicalNodeIdBytes);
                var lastNode = lastNodeField;
                if ((nodeId != lastNode) && (lastNode is not null))
                {
                    if ((nodeId[0] != lastNode[0]) || (nodeId[1] != lastNode[1]) ||
                        (nodeId[2] != lastNode[2]) || (nodeId[3] != lastNode[3]) ||
                        (nodeId[4] != lastNode[4]) || (nodeId[5] != lastNode[5]))
                    {
                        clockSeq = GuidGeneratorState.GetInitClockSequence();
                    }
                }
                lastNodeField = nodeId;
            }

            var lastTs = GuidGeneratorState.LastTimestamp;
            if (timestamp <= lastTs) { clockSeq++; }
            GuidGeneratorState.LastTimestamp = timestamp;

            _ = (clockSeq != GuidGeneratorState.ClockSequence) ?
                GuidGeneratorState.SaveToStorageAsync() :
                GuidGeneratorState.LastSavingAsyncResultCache.Value;
            GuidGeneratorState.ClockSequence = clockSeq;
            return clockSeq;
        }
    }

    private static bool LoadFromStorage()
    {
        var storageFile = GuidGeneratorState.StorageFilePath;
        if (storageFile is null) { return false; }

        try
        {
            lock (GuidGeneratorState.SyncRoot)
            {
                using var stream = new FileStream(storageFile,
                    FileMode.Open, FileAccess.Read, FileShare.Read);
                using var reader = new BinaryReader(stream);
                var version = reader.ReadInt32();
                if (version != GuidGeneratorState.VersionNumber)
                {
                    throw new InvalidDataException($"Unknown version number: {version}.");
                }
                var fieldFlags = reader.ReadInt32();
                var phyNodeId = reader.ReadBytes(6);
                var randNodeId = reader.ReadBytes(6);
                var timestamp = reader.ReadInt64();
                var clockSeq = reader.ReadInt32();

                if ((fieldFlags & 0x01) == 0x01)
                {
                    phyNodeId[0] &= 0xFE;
                    GuidGeneratorState.LastPhysicalNodeIdBytes = phyNodeId;
                }
                if ((fieldFlags & 0x02) == 0x02)
                {
                    randNodeId[0] |= 0x01;
                    GuidGeneratorState.LastRandomNodeIdBytes = randNodeId;
                }
                Debug.Assert((fieldFlags & 0x04) == 0x04);
                GuidGeneratorState.LastTimestamp = timestamp;
                Debug.Assert((fieldFlags & 0x08) == 0x08);
                GuidGeneratorState.ClockSequence = clockSeq;
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStateStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Read));
            return false;
        }
    }

    private static bool SaveToStorage()
    {
        var storageFile = GuidGeneratorState.StorageFilePath;
        if (storageFile is null) { return false; }

        try
        {
            lock (GuidGeneratorState.SyncRoot)
            {
                using var stream = new FileStream(storageFile,
                    FileMode.Create, FileAccess.Write, FileShare.None);
                using var writer = new BinaryWriter(stream);
                var fieldFlags = 0x04 | 0x08;
                var phyNodeId = GuidGeneratorState.LastPhysicalNodeIdBytes;
                if (phyNodeId is not null) { fieldFlags |= 0x01; }
                var randNodeId = GuidGeneratorState.LastRandomNodeIdBytes;
                if (randNodeId is not null) { fieldFlags |= 0x02; }
                var emptyNodeId = GuidGeneratorState.EmptyNodeIdBytes;

                writer.Write(GuidGeneratorState.VersionNumber);
                writer.Write(fieldFlags);
                writer.Write(phyNodeId ?? emptyNodeId, 0, 6);
                writer.Write(randNodeId ?? emptyNodeId, 0, 6);
                writer.Write(GuidGeneratorState.LastTimestamp);
                writer.Write(GuidGeneratorState.ClockSequence);
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStateStorageException(
                new StateStorageExceptionEventArgs(ex, storageFile, FileAccess.Write));
            return false;
        }
    }
}
