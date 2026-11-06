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

    private static readonly SemaphoreSlim StorageLock = SemaphoreSlim.CreateLock();

    private static readonly AutoRefreshCache<Task<bool>> LastSavingAsyncResultCache =
        new(GuidGeneratorState.SaveToStorageAsync, refreshPeriod: 10 * 1000, sleepAfter: 1);

    public static string? StorageFile => GuidGeneratorState.StorageFileName;

    internal static byte[]? RandomNodeId => GuidGeneratorState.RandomNodeState.LastNodeIdBytes;

    public static event EventHandler<StateStorageExceptionEventArgs>? StorageException;

    public static bool SetStorageFile(
        string? fileName, StreamProvider? streamProvider = null)
    {
        if (fileName is null) { return false; }

        try
        {
            using (GuidGeneratorState.StorageLock.EnterScope())
            {
                GuidGeneratorState.StorageFileName = fileName;
                GuidGeneratorState.StreamProvider = streamProvider ??
                    GuidGeneratorState.OpenLocalFile;
                GuidGeneratorState.LoadFromStorage(fileName);
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, fileName, FileAccess.Read));
            return false;
        }
    }

    public static async Task<bool> SetStorageFileAsync(
        string? fileName, StreamProvider? streamProvider = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (fileName is null) { return false; }

        try
        {
            using (await GuidGeneratorState.StorageLock.EnterScopeAsync(cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false))
            {
                GuidGeneratorState.StorageFileName = fileName;
                GuidGeneratorState.StreamProvider = streamProvider ??
                    GuidGeneratorState.OpenLocalFile;
                await GuidGeneratorState.LoadFromStorageAsync(fileName, cancellationToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, fileName, FileAccess.Read));
            return false;
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

    private static Stream OpenLocalFile(string fileName, FileAccess operationType)
    {
        // Very small file, don't use OS async I/O in the default provider to avoid overhead.
        return operationType switch
        {
            FileAccess.Read => new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.Read),
            FileAccess.Write => new FileStream(fileName,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.None),
            _ => throw new ArgumentOutOfRangeException(nameof(operationType))
        };
    }

    private static bool SaveToStorage()
    {
        var fileName = GuidGeneratorState.StorageFile;
        if (fileName is null) { return false; }

        try
        {
            using (GuidGeneratorState.StorageLock.EnterScope())
            {
                fileName = GuidGeneratorState.StorageFile;
                if (fileName is null) { return false; }
                GuidGeneratorState.SaveToStorageCore(fileName);
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, fileName!, FileAccess.Write));
            return false;
        }
    }

    private static async Task<bool> SaveToStorageAsync()
    {
        var fileName = GuidGeneratorState.StorageFile;
        if (fileName is null) { return false; }

        try
        {
            using (await GuidGeneratorState.StorageLock.EnterScopeAsync()
                .ConfigureAwait(continueOnCapturedContext: false))
            {
                fileName = GuidGeneratorState.StorageFile;
                if (fileName is null) { return false; }
                await GuidGeneratorState.SaveToStorageAsyncCore(fileName)
                    .ConfigureAwait(continueOnCapturedContext: false);
                return true;
            }
        }
        catch (Exception ex)
        {
            GuidGeneratorState.OnStorageException(
                new StateStorageExceptionEventArgs(ex, fileName!, FileAccess.Write));
            return false;
        }
    }

    private static void OnStorageException(StateStorageExceptionEventArgs e)
    {
        Volatile.Read(ref GuidGeneratorState.StorageException)?.Invoke(null, e);
    }
}
