﻿// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace global

open System.Diagnostics
open System.Diagnostics.CodeAnalysis

/// <summary>
/// Contains a set of extension operators and functions.
/// </summary>
[<DebuggerNonUserCode; ExcludeFromCodeCoverage>]
[<AutoOpen>]
module internal ExtensionOperators =

    /// <summary>
    /// Build an enum value from an underlying value.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <returns>The value as an enumeration.</returns>
    [<CompiledName("EnumOfValue")>]
    let inline enumof (value: 'T) : 'Enum =
        LanguagePrimitives.EnumOfValue value

    /// <summary>
    /// Reverses the order of arguments of an infix operator
    /// to translate it into a normal F# function.
    /// </summary>
    /// <param name="func">The infix operator function.</param>
    /// <param name="arg2">The right operand of the operator.</param>
    /// <param name="arg1">The left operand of the operator.</param>
    /// <returns>The return value of the infix operator.</returns>
    [<CompiledName("OperatorAsFunc")>]
    let inline op (func: 'T1 -> 'T2 -> 'U) : 'T2 -> 'T1 -> 'U =
        fun arg2 arg1 -> func arg1 arg2

    /// <summary>
    /// Apply an action to a value and returns the value.
    /// </summary>
    /// <param name="action">The action to apply.</param>
    /// <param name="value">The input value.</param>
    /// <returns>The input value.</returns>
    [<CompiledName("TeeAction")>]
    let inline tee (action: 'T -> unit) (value: 'T) : 'T =
        action value
        value

    /// <summary>
    /// Apply an action to a value and returns the value,
    /// the value being on the left, the action on the right.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="action">The action to apply.</param>
    /// <returns>The input value.</returns>
    [<CompiledName("op_TeeAction")>]
    let inline ( |- ) (value: 'T) (action: 'T -> unit) : 'T =
        action value
        value
