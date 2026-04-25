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
        string? fileName, StreamProvider? streamProvider = null,
        CancellationToken cancellationToken = default)
    {
        await GuidGeneratorState.StorageLock.WaitAsync(cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        try
        {
            GuidGeneratorState.StorageFileName = fileName;
            GuidGeneratorState.StreamProvider = streamProvider ??
                GuidGeneratorState.OpenLocalFile;
            return await GuidGeneratorState.LoadFromStorageAsync(cancellationToken)
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

    private static Stream OpenLocalFile(string storageFile, FileAccess operationType)
    {
        // Very small file, don't use OS async I/O in the default provider to avoid overhead.
        return operationType switch
        {
            FileAccess.Read => new FileStream(storageFile,
                FileMode.Open, FileAccess.Read, FileShare.Read),
            FileAccess.Write => new FileStream(storageFile,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.None),
            _ => throw new ArgumentOutOfRangeException(nameof(operationType))
        };
    }

    private static bool SaveToStorage()
    {
        GuidGeneratorState.StorageLock.Wait();
        try
        {
            return GuidGeneratorState.SaveToStorageCore();
        }
        finally
        {
            GuidGeneratorState.StorageLock.Release();
        }
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

    private static void OnStorageException(StateStorageExceptionEventArgs e)
    {
        Volatile.Read(ref GuidGeneratorState.StorageException)?.Invoke(null, e);
    }
}
