using System;
using System.IO;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides data for the <see cref="GuidGeneratorState.StateStorageException"/> event.
/// </summary>
public sealed class StateStorageExceptionEventArgs : EventArgs
{
    /// <summary>
    /// Initialize a new instance of the <see cref="StateStorageExceptionEventArgs"/> class.
    /// </summary>
    /// <param name="exception">The exception thrown by the state storage IO operation.</param>
    /// <param name="filePath">The full path of the state storage file.</param>
    /// <param name="operation">A value that indicates where the exception was thrown,
    /// <see cref="FileAccess.Read"/> for a loading operation and
    /// <see cref="FileAccess.Write"/> for a saving operation.</param>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/>
    /// or <paramref name="filePath"/> is <see langword="null"/>.</exception>
    internal StateStorageExceptionEventArgs(
        Exception exception, string filePath, FileAccess operation)
    {
        this.StorageException = exception ??
            throw new ArgumentNullException(nameof(exception));
        this.StorageFilePath = filePath ??
            throw new ArgumentNullException(nameof(filePath));
        this.StorageOperation = operation;
    }

    /// <summary>
    /// Gets the exception thrown by the state storage IO operation.
    /// </summary>
    /// <returns>The exception thrown by the state storage IO operation.</returns>
    public Exception StorageException { get; }

    /// <summary>
    /// Gets the full path of the state storage file.
    /// </summary>
    /// <returns>The full path of the state storage file.</returns>
    public string StorageFilePath { get; }

    /// <summary>
    /// Gets a value that indicates where the exception was thrown,
    /// <see cref="FileAccess.Read"/> for a loading operation and
    /// <see cref="FileAccess.Write"/> for a saving operation.
    /// </summary>
    /// <returns><see cref="FileAccess.Read"/> if the exception
    /// is thrown by a loading operation; <see cref="FileAccess.Write"/>
    /// if the exception is thrown by a saving operation.</returns>
    public FileAccess StorageOperation { get; }

    /// <summary>
    /// Sets the full path of the state storage file and returns a value
    /// that indicates the state storage loading operation is successful.
    /// </summary>
    /// <param name="filePath">The full path of the state storage file,
    /// or <see langword="null"/> to disable the state storage.</param>
    /// <returns><see langword="true"/> if the state storage loading operation
    /// is successful; otherwise, <see langword="false"/>.</returns>
    public bool SetStorageFilePath(string? filePath)
    {
        return GuidGeneratorState.SetStorageFilePath(filePath);
    }
}
