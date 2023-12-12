namespace XNetEx.FSharp.Core

/// <summary>
/// Contains functions for converting between options and value options.
/// </summary>
[<AutoOpen>]
module internal OptionConvert =

    /// <summary>
    /// Contains functions for converting options from and to value options.
    /// </summary>
    [<RequireQualifiedAccess>]
    module Option =

        /// <summary>
        /// Convert a Boolean value to a unit option.
        /// </summary>
        /// <param name="value">The input Boolean value.</param>
        /// <returns>The result option.</returns>
        [<CompiledName("OfBoolean")>]
        let inline ofBool (value: bool) : unit option =
            if value then Some () else None

        /// <summary>
        /// Convert a value option to an option.
        /// </summary>
        /// <param name="voption">The input value option.</param>
        /// <returns>The result option.</returns>
        [<CompiledName("OfValueOption")>]
        let inline ofValueOption (voption: 'T voption) : 'T option =
            match voption with
            | ValueSome value -> Some value
            | ValueNone -> None

        /// <summary>
        /// Convert the option to a value option.
        /// </summary>
        /// <param name="option">The input option.</param>
        /// <returns>The result value option.</returns>
        [<CompiledName("ToValueOption")>]
        let inline toValueOption (option: 'T option) : 'T voption =
            match option with
            | Some value -> ValueSome value
            | None -> ValueNone

    /// <summary>
    /// Contains functions for converting value options from and to options.
    /// </summary>
    [<RequireQualifiedAccess>]
    module ValueOption =

        /// <summary>
        /// Convert a Boolean value to a unit value option.
        /// </summary>
        /// <param name="value">The input Boolean value.</param>
        /// <returns>The result value option.</returns>
        [<CompiledName("OfBoolean")>]
        let inline ofBool (value: bool) : unit voption =
            if value then ValueSome () else ValueNone

        /// <summary>
        /// Convert an option to a value option.
        /// </summary>
        /// <param name="option">The input option.</param>
        /// <returns>The result value option.</returns>
        [<CompiledName("OfOption")>]
        let inline ofOption (option: 'T option) : 'T voption =
            match option with
            | Some value -> ValueSome value
            | None -> ValueNone

        /// <summary>
        /// Convert the value option to an option.
        /// </summary>
        /// <param name="voption">The input value option.</param>
        /// <returns>The result option.</returns>
        [<CompiledName("ToOption")>]
        let inline toOption (voption: 'T voption) : 'T option =
            match voption with
            | ValueSome value -> Some value
            | ValueNone -> None
