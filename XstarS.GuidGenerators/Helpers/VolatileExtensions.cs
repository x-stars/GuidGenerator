using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Threading;

/// <summary>
/// Provides extension methods for the <see cref="Volatile"/> class.
/// </summary>
internal static class VolatileExtensions
{
    extension(Volatile)
    {
        /// <summary>
        /// Writes the specified object reference to the specified field
        /// with volatile write semantics and returns the written value.
        /// </summary>
        /// <typeparam name="T">The type of field to write.
        /// This must be a reference type, not a value type.</typeparam>
        /// <param name="location">The object reference to write.</param>
        /// <param name="value">The value to set.</param>
        [return: NotNullIfNotNull(nameof(value))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WriteValue<T>(ref T location, T value)
            where T : class?
        {
            Volatile.Write(ref location, value);
            return value;
        }
    }
}
