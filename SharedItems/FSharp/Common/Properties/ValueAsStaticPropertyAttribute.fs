// Copyright (c) 2023 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace FSharp.Compiler.Internals

open System

/// <summary>
/// This Attribute is used to make Value bindings like
/// <code>let x = some code</code>
/// operate like static properties.
/// </summary>
[<Sealed>]
[<AttributeUsage(AttributeTargets.Property,
                 AllowMultiple = false)>]
type internal ValueAsStaticPropertyAttribute() =
    inherit Attribute()
