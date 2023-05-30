// Copyright (c) 2023 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace FSharp.Compiler.Internals

open System
open System.Diagnostics
open System.Diagnostics.CodeAnalysis

/// <summary>
/// This Attribute is used to make Value bindings like
/// <code>let x = some code</code>
/// operate like static properties.
/// </summary>
[<DebuggerNonUserCode; ExcludeFromCodeCoverage>]
[<AttributeUsage(AttributeTargets.Property, AllowMultiple = false)>]
[<Sealed>]
type internal ValueAsStaticPropertyAttribute() =
    inherit Attribute()
