﻿// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

#pragma warning disable
#nullable enable

#if !(KEY_VALUE_PAIR_DECONSTRUCT_EXTERNAL || NETCOREAPP3_0_OR_GREATER)
#if !(NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

[DebuggerNonUserCode, ExcludeFromCodeCoverage]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("This type supports the key/value pair deconstruction syntax " +
          "for early frameworks and should not be used directly in user code.")]
internal static class __KeyValuePairItems
{
    /// <summary>
    /// Deconstructs the current <see cref="DictionaryEntry"/>.
    /// </summary>
    /// <param name="entry">The current <see cref="DictionaryEntry"/>.</param>
    /// <param name="key">The key of the current <see cref="DictionaryEntry"/>.</param>
    /// <param name="value">The value of the current <see cref="DictionaryEntry"/>.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Deconstruct(
        this DictionaryEntry entry, out object key, out object? value)
    {
        key = entry.Key;
        value = entry.Value;
    }

    /// <summary>
    /// Deconstructs the current <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="pair">The current <see cref="KeyValuePair{TKey, TValue}"/>.</param>
    /// <param name="key">The key of the current <see cref="KeyValuePair{TKey, TValue}"/>.</param>
    /// <param name="value">The value of the current <see cref="KeyValuePair{TKey, TValue}"/>.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Deconstruct<TKey, TValue>(
        this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
        key = pair.Key;
        value = pair.Value;
    }
}
#endif
#endif

#if !(KEY_VALUE_PAIR_CREATE_EXTERNAL || NETCOREAPP3_0_OR_GREATER)
#if !(NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Collections.Generic
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Creates instances of the <see cref="KeyValuePair{TKey, TValue}"/> struct.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    internal static class KeyValuePair
    {
        /// <summary>
        /// Creates a new key/value pair instance using provided values.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key of the new <see cref="KeyValuePair{TKey, TValue}"/> to be created.</param>
        /// <param name="value">The key of the new <see cref="KeyValuePair{TKey, TValue}"/> to be created.</param>
        /// <returns>A key/value pair containing the provided arguments as values.</returns>
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
#endif
#endif

#if !(EXCLUDE_FROM_CODE_COVERAGE_ATTRIBUTE || NETCOREAPP3_0_OR_GREATER)
#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    // Excludes the attributed code from code coverage information.
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
