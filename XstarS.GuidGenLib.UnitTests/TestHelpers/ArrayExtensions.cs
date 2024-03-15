using System;

namespace XNetEx;

internal static class ArrayExtensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, array.Length);
        result[array.Length] = item;
        return result;
    }

    public static T[] Concat<T>(this T[] array, T[] other)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var result = new T[array.Length + other.Length];
        Array.Copy(array, 0, result, 0, array.Length);
        Array.Copy(other, 0, result, array.Length, other.Length);
        return result;
    }

    public static T[] Insert<T>(this T[] array, int index, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index),
                "Index must be non-negative and less than the length of the array.");
        }

        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, index);
        result[index] = item;
        Array.Copy(array, index, result, index + 1, array.Length - index);
        return result;
    }

    public static T[] InsertRange<T>(this T[] array, int index, T[] items)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index),
                "Index must be non-negative and less than the length of the array.");
        }

        var result = new T[array.Length + items.Length];
        Array.Copy(array, 0, result, 0, index);
        Array.Copy(items, 0, result, index, items.Length);
        Array.Copy(array, index, result, index + items.Length, array.Length - index);
        return result;
    }

    public static T[] Prepend<T>(this T[] array, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var result = new T[array.Length + 1];
        result[0] = item;
        Array.Copy(array, 0, result, 1, array.Length);
        return result;
    }
}
