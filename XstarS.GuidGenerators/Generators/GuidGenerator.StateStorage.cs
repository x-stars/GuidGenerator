using System;

namespace XNetEx.Guids.Generators;

partial class GuidGenerator
{
    /// <summary>
    /// Gets the path of the state storage file.
    /// </summary>
    /// <returns>The path of the state storage file,
    /// or <see langword="null"/> if the state storage is disabled.</returns>
    public static string? StateStorageFile => GuidGeneratorState.StorageFile;

    /// <summary>
    /// Occurs when a state storage I/O operation throws an exception.
    /// </summary>
    public static event EventHandler<StateStorageExceptionEventArgs>? StateStorageException
    {
        add => GuidGeneratorState.StorageException += value;
        remove => GuidGeneratorState.StorageException -= value;
    }

    /// <summary>
    /// Sets the path of the state storage file and returns a value
    /// that indicates whether the state storage loading operation is successful.
    /// </summary>
    /// <param name="fileName">The path of the state storage file,
    /// or <see langword="null"/> to disable the state storage.</param>
    /// <returns><see langword="true"/> if the state storage loading operation
    /// is successful; otherwise, <see langword="false"/>.</returns>
    public static bool SetStateStorageFile(string? fileName)
    {
        return GuidGeneratorState.SetStorageFile(fileName);
    }
}
