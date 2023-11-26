namespace XNetEx.FSharp.Core

open System
open XNetEx.FSharp.Core

/// <summary>
/// Contains active patterns for matching values of type <see cref="T:System.Guid"/>.
/// </summary>
[<AutoOpen>]
module GuidPatterns =

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is generated based on the current time.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp.</returns>
    [<CompiledName("TimeBasedPattern")>]
    let (|TimeBased|_|) (guid: Guid) : DateTime option =
        Guid.tryGetTime guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is generated based on the input name.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit option.</returns>
    [<CompiledName("NameBasedPattern")>]
    let (|NameBased|_|) (guid: Guid) : unit option =
        if Guid.isNameBased guid then Some () else None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit option.</returns>
    [<CompiledName("RandomizedPattern")>]
    let (|Randomized|_|) (guid: Guid) : unit option =
        if Guid.isRandomized guid then Some () else None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the clock sequence.</returns>
    [<CompiledName("ClockSequencePattern")>]
    let (|ClockSeq|_|) (guid: Guid) : int16 option =
        Guid.tryGetClockSeq guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains local ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the DCE Security domain and local ID.</returns>
    [<CompiledName("DomainAndLocalIdPattern")>]
    let (|LocalId|_|) (guid: Guid) : struct (Guid.Domain * int) option =
        Guid.tryGetLocalId guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the node ID.</returns>
    [<CompiledName("NodeIdPattern")>]
    let (|NodeId|_|) (guid: Guid) : byte[] option =
        Guid.tryGetNodeId guid |> ValueOption.toOption
