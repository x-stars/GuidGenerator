using System;
using System.IO;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides data for the <see cref="GuidGenerator.StateStorageException"/> event.
/// </summary>
public sealed class StateStorageExceptionEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StateStorageExceptionEventArgs"/> class.
    /// </summary>
    /// <param name="exception">The exception thrown by the storage operation.</param>
    /// <param name="fileName">The path of the state storage file.</param>
    /// <param name="operationType">A value that indicates where the exception was thrown,
    /// <see cref="FileAccess.Read"/> for a loading operation and
    /// <see cref="FileAccess.Write"/> for a saving operation.</param>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/>
    /// or <paramref name="fileName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="operationType"/> is
    /// neither <see cref="FileAccess.Read"/> nor <see cref="FileAccess.Write"/>.</exception>
    internal StateStorageExceptionEventArgs(
        Exception exception, string fileName, FileAccess operationType)
    {
        this.Exception = exception ??
            throw new ArgumentNullException(nameof(exception));
        this.FileName = fileName ??
            throw new ArgumentNullException(nameof(fileName));
        var isValidType = operationType is FileAccess.Read or FileAccess.Write;
        this.OperationType = isValidType ? operationType :
            throw new ArgumentOutOfRangeException(nameof(operationType));
    }

    /// <summary>
    /// Gets the exception thrown by the storage operation.
    /// </summary>
    /// <returns>The exception thrown by the storage operation.</returns>
    public Exception Exception { get; }

    /// <summary>
    /// Gets the path of the state storage file.
    /// </summary>
    /// <returns>The path of the state storage file.</returns>
    public string FileName { get; }

    /// <summary>
    /// Gets a value that indicates where the exception was thrown,
    /// <see cref="FileAccess.Read"/> for a loading operation and
    /// <see cref="FileAccess.Write"/> for a saving operation.
    /// </summary>
    /// <returns><see cref="FileAccess.Read"/> if the exception
    /// is thrown by a loading operation; <see cref="FileAccess.Write"/>
    /// if the exception is thrown by a saving operation.</returns>
    public FileAccess OperationType { get; }
}
