using System;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides the state storage for <see cref="GuidGenerator"/>.
/// </summary>
public static partial class GuidGeneratorState
{
    private static volatile string? CurrentStorageFile = null;

    /// <summary>
    /// Gets the full path of the state storage file.
    /// </summary>
    /// <returns>The full path of the state storage file,
    /// or <see langword="null"/> if the state storage is disabled.</returns>
    public static string? StorageFilePath => GuidGeneratorState.CurrentStorageFile;

    /// <summary>
    /// Occurs when a state storage I/O operation throws an exception.
    /// </summary>
    public static event EventHandler<StateStorageExceptionEventArgs>? StorageException;

    /// <summary>
    /// Sets the full path of the state storage file and returns a value
    /// that indicates whether the state storage loading operation is successful.
    /// </summary>
    /// <param name="filePath">The full path of the state storage file,
    /// or <see langword="null"/> to disable the state storage.</param>
    /// <returns><see langword="true"/> if the state storage loading operation
    /// is successful; otherwise, <see langword="false"/>.</returns>
    public static bool SetStorageFilePath(string? filePath)
    {
        GuidGeneratorState.CurrentStorageFile = filePath;
        return GuidGeneratorState.LoadFromStorage();
    }

    /// <summary>
    /// Raises the <see cref="GuidGeneratorState.StorageException"/> event.
    /// </summary>
    /// <param name="e">A <see cref="StateStorageExceptionEventArgs"/>
    /// that contains the event data.</param>
    private static void OnStorageException(StateStorageExceptionEventArgs e)
    {
        GuidGeneratorState.StorageException?.Invoke(null, e);
    }
}
