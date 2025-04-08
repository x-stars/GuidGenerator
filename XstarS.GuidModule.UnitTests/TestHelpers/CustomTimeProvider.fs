namespace XNetEx.FSharp.Core

#if NET8_0_OR_GREATER

open System

[<Sealed>]
type internal CustomTimeProvider
    (timestampProvider: unit -> DateTimeOffset) =
    inherit TimeProvider()

    override _.GetUtcNow() = timestampProvider ()

#endif
