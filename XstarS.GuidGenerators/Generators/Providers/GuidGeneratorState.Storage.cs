using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorState
{
    private const int VersionNumber = 4122;

    private static readonly byte[] EmptyNodeId = new byte[6];

    private static volatile string? StorageFileName = GuidGeneratorState.SetSaveOnProcessExit();

    private static readonly AutoRefreshCache<Task<bool>> LastSavingAsyncResultCache =
        new(GuidGeneratorState.SaveToStorageAsync, refreshPeriod: 10 * 1000, sleepAfter: 0);

    public static string? StorageFile => GuidGeneratorState.StorageFileName;

    internal static byte[]? RandomNodeId => GuidGeneratorState.RandomNodeState.LastNodeIdBytes;

    public static event EventHandler<StateStorageExceptionEventArgs>? StorageException;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool SetStorageFile(string? fileName)
    {
        GuidGeneratorState.StorageFileName = fileName;
        return GuidGeneratorState.LoadFromStorage();
    }

    public static void ResetGlobal()
    {
        lock (GuidGeneratorState.PhysicalNodeState)
        {
            GuidGeneratorState.PhysicalNodeState.Reset();
        }
        lock (GuidGeneratorState.RandomNodeState)
        {
            NodeIdProvider.ResetNonVolatileRandom();
            GuidGeneratorState.RandomNodeState.Reset();
        }
    }

    private static string? SetSaveOnProcessExit()
    {
        static void SaveToStorage(object? sender, EventArgs e) =>
            GuidGeneratorState.SaveToStorage();
        AppDomain.CurrentDomain.ProcessExit += SaveToStorage;
        return null;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static Task<bool> SaveToStorageAsync()
    {
        return Task.Run(GuidGeneratorState.SaveToStorage);
    }

    private static void OnStorageException(StateStorageExceptionEventArgs e)
    {
        Volatile.Read(ref GuidGeneratorState.StorageException)?.Invoke(null, e);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static bool LoadFromStorage()
    {
        var storageFile = GuidGeneratorState.StorageFile;
        if (storageFile is null) { return false; }

        try
        {
            using var stream = new FileStream(storageFile,
                FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(stream);
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
            stream.Close();

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

    [MethodImpl(MethodImplOptions.Synchronized)]
    private static bool SaveToStorage()
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
                    timestamp = Math.Max(
                        phyState.LastTimestamp, randState.LastTimestamp);
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

            using var stream = new FileStream(storageFile,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var writer = new BinaryWriter(stream);
            const int nodeIdSize = 6;
            writer.Write(GuidGeneratorState.VersionNumber);
            writer.Write(fieldFlags);
            writer.Write(timestamp);
            writer.Write(clockSeq);
            writer.Write(phyNodeId, 0, nodeIdSize);
            writer.Write(randNodeId, 0, nodeIdSize);
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
