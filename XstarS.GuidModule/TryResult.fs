namespace XNetEx.FSharp.Core

/// <summary>
/// Contains operations for working with results of C#-style try-operations.
/// </summary>
[<RequireQualifiedAccess>]
module internal TryResult =

    /// <summary>
    /// Converts the result of a C#-style try-operation to an <see cref="T:Microsoft.FSharp.Core.option`1"/>.
    /// </summary>
    /// <param name="ret">The value indicates whether the try-operation was successful.</param>
    /// <param name="out">The output value the try-operation.</param>
    /// <returns>An <see cref="T:Microsoft.FSharp.Core.option`1"/> of the try-operation result.</returns>
    [<CompiledName("ToOption")>]
    let inline toOption (ret: bool, out: 'T) : 'T option =
        if ret then Some out else None

    /// <summary>
    /// Converts the result of a C#-style try-operation to an <see cref="T:Microsoft.FSharp.Core.option`1"/>.
    /// </summary>
    /// <param name="ret">The value indicates whether the try-operation was successful.</param>
    /// <param name="out1">The first output value the try-operation.</param>
    /// <param name="out2">The second output value the try-operation.</param>
    /// <returns>An <see cref="T:Microsoft.FSharp.Core.option`1"/> of the try-operation result.</returns>
    [<CompiledName("ToOption2")>]
    let inline toOption2 (ret: bool, out1: 'T1, out2: 'T2) : ('T1 * 'T2) option =
        if ret then Some (out1, out2) else None

    /// <summary>
    /// Converts the result of a C#-style try-operation to a <see cref="T:Microsoft.FSharp.Core.voption`1"/>.
    /// </summary>
    /// <param name="ret">The value indicates whether the try-operation was successful.</param>
    /// <param name="out">The output value the try-operation.</param>
    /// <returns>A <see cref="T:Microsoft.FSharp.Core.voption`1"/> of the try-operation result.</returns>
    [<CompiledName("ToValueOption")>]
    let inline toValueOption (ret: bool, out: 'T) : 'T voption =
        if ret then ValueSome out else ValueNone

    /// <summary>
    /// Converts the result of a C#-style try-operation to a <see cref="T:Microsoft.FSharp.Core.voption`1"/>.
    /// </summary>
    /// <param name="ret">The value indicates whether the try-operation was successful.</param>
    /// <param name="out1">The first output value the try-operation.</param>
    /// <param name="out2">The second output value the try-operation.</param>
    /// <returns>A <see cref="T:Microsoft.FSharp.Core.voption`1"/> of the try-operation result.</returns>
    [<CompiledName("ToValueOption2")>]
    let inline toValueOption2 (ret: bool, out1: 'T1, out2: 'T2) : struct ('T1 * 'T2) voption =
        if ret then ValueSome struct (out1, out2) else ValueNone
