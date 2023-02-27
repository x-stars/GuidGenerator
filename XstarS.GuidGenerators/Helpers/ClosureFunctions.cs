using System;

namespace XNetEx.Runtime.CompilerServices;

/// <summary>
/// Provides a set of extension methods that can
/// use the current instance as the closure object when used as delegates.
/// </summary>
internal static class ClosureFunctions
{
    /// <summary>
    /// Returns the current instance.
    /// </summary>
    /// <typeparam name="T">The type of the current instance.</typeparam>
    /// <param name="value">The current instance.</param>
    /// <returns>The current instance.</returns>
    public static T Identity<T>(this T value) where T : class => value;

    /// <summary>
    /// Returns the current instance.
    /// </summary>
    /// <typeparam name="T">The type of the current instance.</typeparam>
    /// <param name="value">The current instance.</param>
    /// <returns>The current instance.</returns>
    /// <exception cref="NullReferenceException">
    /// <paramref name="value"/> is <see langword="null"/>,
    /// and <typeparamref name="T"/> is a non-nullable value type.</exception>
    /// <exception cref="InvalidCastException"><paramref name="value"/>
    /// is not an instance of <typeparamref name="T"/>.</exception>
    public static T Identity<T>(this ValueType? value) => (T)(object?)value!;
}
