namespace XNetEx.FSharp.Core

open System
open XNetEx.FSharp.Core

/// <summary>
/// Contains active patterns for matching values of type <see cref="T:System.Guid"/>.
/// </summary>
[<AutoOpen>]
module GuidPatterns =

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is time-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp.</returns>
    [<CompiledName("TimeBasedGuidPattern")>]
    let (|TimeBasedGuid|_|) (guid: Guid) : DateTime option =
        Guid.tryGetTime guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is name-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the hash data and its bitmask.</returns>
    [<CompiledName("NameBasedGuidPattern")>]
    let (|NameBasedGuid|_|) (guid: Guid) : Guid.DataAndMask option =
        Guid.tryGetHashData guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the random data and its bitmask.</returns>
    [<CompiledName("RandomizedGuidPattern")>]
    let (|RandomizedGuid|_|) (guid: Guid) : Guid.DataAndMask option =
        Guid.tryGetRandomData guid |> ValueOption.toOption

#if !UUIDREV_DISABLE
    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains custom data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the custom data and its bitmask.</returns>
    [<CompiledName("CustomizedGuidPattern")>]
    let (|CustomizedGuid|_|) (guid: Guid) : Guid.DataAndMask option =
        Guid.tryGetCustomData guid |> ValueOption.toOption
#endif

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the clock sequence.</returns>
    [<CompiledName("GuidClockSequencePattern")>]
    let (|GuidClockSeq|_|) (guid: Guid) : int16 option =
        Guid.tryGetClockSeq guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains local ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the DCE Security domain and local ID.</returns>
    [<CompiledName("GuidDomainAndLocalIdPattern")>]
    let (|GuidLocalId|_|) (guid: Guid) : struct (Guid.Domain * int) option =
        Guid.tryGetLocalId guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the node ID.</returns>
    [<CompiledName("GuidNodeIdPattern")>]
    let (|GuidNodeId|_|) (guid: Guid) : byte[] option =
        Guid.tryGetNodeId guid |> ValueOption.toOption

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values whose value is all zeros.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit option.</returns>
    [<CompiledName("GuidEmptyPattern")>]
    let (|GuidEmpty|_|) (guid: Guid) : unit option =
        if Guid.(=)(guid, Guid.empty) then Some () else None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 1.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp, clock sequence and node ID.</returns>
    [<CompiledName("GuidVersion1Pattern")>]
    let (|GuidVersion1|_|) (guid: Guid) : struct (DateTime * int16 * byte[]) option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version1 -> Some struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 2.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp, clock sequence,
    /// DCE Security domain and local ID and node ID.</returns>
    [<CompiledName("GuidVersion2Pattern")>]
    let (|GuidVersion2|_|) (guid: Guid)
        : struct (DateTime * int16 * struct (Guid.Domain * int) * byte[]) option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version2 -> Some struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetLocalId guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 3.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the hash data and its bitmask.</returns>
    [<CompiledName("GuidVersion3Pattern")>]
    let (|GuidVersion3|_|) (guid: Guid) : Guid.DataAndMask option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version3 -> Some (
            Guid.tryGetHashData guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 4.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the random data and its bitmask.</returns>
    [<CompiledName("GuidVersion4Pattern")>]
    let (|GuidVersion4|_|) (guid: Guid) : Guid.DataAndMask option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version4 -> Some (
            Guid.tryGetRandomData guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 5.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the hash data and its bitmask.</returns>
    [<CompiledName("GuidVersion5Pattern")>]
    let (|GuidVersion5|_|) (guid: Guid) : Guid.DataAndMask option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version5 -> Some (
            Guid.tryGetHashData guid |> ValueOption.get)
        | _, _ -> None

#if !UUIDREV_DISABLE
    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 6.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp, clock sequence and node ID.</returns>
    [<CompiledName("GuidVersion6Pattern")>]
    let (|GuidVersion6|_|) (guid: Guid) : struct (DateTime * int16 * byte[]) option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version6 -> Some struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 7.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the timestamp and random data and its bitmask.</returns>
    [<CompiledName("GuidVersion7Pattern")>]
    let (|GuidVersion7|_|) (guid: Guid) : struct (DateTime * Guid.DataAndMask) option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version7 -> Some struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetRandomData guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 8.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An option of the custom data and its bitmask.</returns>
    [<CompiledName("GuidVersion8Pattern")>]
    let (|GuidVersion8|_|) (guid: Guid) : Guid.DataAndMask option =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version8 -> Some (
            Guid.tryGetCustomData guid |> ValueOption.get)
        | _, _ -> None

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values whose value is all ones.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit option.</returns>
    [<CompiledName("GuidMaxValuePattern")>]
    let (|GuidMaxValue|_|) (guid: Guid) : unit option =
        if Guid.(=)(guid, Guid.maxValue) then Some () else None
#endif
