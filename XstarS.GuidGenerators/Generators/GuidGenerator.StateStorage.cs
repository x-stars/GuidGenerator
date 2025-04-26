using System;
using System.Threading.Tasks;

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

    /// <summary>
    /// Resets the state that can be saving to the state storage file.
    /// </summary>
    public static void ResetState()
    {
        GuidGeneratorState.ResetGlobal();
    }

#if !UUIDREV_DISABLE
    /// <summary>
    /// Creates a new blocking-free <see cref="IGuidGenerator"/> instance that uses an object pool
    /// of <see cref="IBlockingGuidGenerator"/> created by the specified creation delegate.
    /// </summary>
    /// <param name="factory">The delegate used to create <see cref="IBlockingGuidGenerator"/>.</param>
    /// <param name="capacity">The maximum number of instances to retain in the pool,
    /// or -1 to retain unlimited number of instances in the pool.</param>
    /// <returns>A new blocking-free <see cref="IGuidGenerator"/> instance that uses an object pool
    /// of <see cref="IBlockingGuidGenerator"/> created by <paramref name="factory"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="capacity"/> is not a positive value and is not equal to -1.</exception>
    public static IGuidGenerator CreatePooled(Func<IBlockingGuidGenerator> factory, int capacity = -1)
    {
        return new GuidGeneratorPool(factory, capacity);
    }
#endif

    /// <summary>
    /// Creates a new <see cref="CustomStateGuidGeneratorBuilder"/> instance
    /// of the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    /// <returns>The <see cref="CustomStateGuidGeneratorBuilder"/>
    /// instance of <paramref name="version"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version"/> does not support using custom states.</exception>
    public static CustomStateGuidGeneratorBuilder CreateCustomStateBuilder(GuidVersion version)
    {
        return CustomStateGuidGeneratorBuilder.Create(version);
    }
}
