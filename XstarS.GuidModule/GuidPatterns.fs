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
    /// <returns>A value option of the timestamp.</returns>
    [<CompiledName("TimeBasedGuidPattern"); return: Struct>]
    let (|TimeBasedGuid|_|) (guid: Guid) : DateTime voption =
        Guid.tryGetTime guid

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is name-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the hash data and its bitmask.</returns>
    [<CompiledName("NameBasedGuidPattern"); return: Struct>]
    let (|NameBasedGuid|_|) (guid: Guid) : Guid.DataAndMask voption =
        Guid.tryGetHashData guid

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the random data and its bitmask.</returns>
    [<CompiledName("RandomizedGuidPattern"); return: Struct>]
    let (|RandomizedGuid|_|) (guid: Guid) : Guid.DataAndMask voption =
        Guid.tryGetRandomData guid

#if !UUIDREV_DISABLE
    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains custom data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the custom data and its bitmask.</returns>
    [<CompiledName("CustomizedGuidPattern"); return: Struct>]
    let (|CustomizedGuid|_|) (guid: Guid) : Guid.DataAndMask voption =
        Guid.tryGetCustomData guid
#endif

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the clock sequence.</returns>
    [<CompiledName("GuidClockSequencePattern"); return: Struct>]
    let (|GuidClockSeq|_|) (guid: Guid) : int16 voption =
        Guid.tryGetClockSeq guid

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains local ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the DCE Security domain and local ID.</returns>
    [<CompiledName("GuidDomainAndLocalIdPattern"); return: Struct>]
    let (|GuidLocalId|_|) (guid: Guid) : struct (Guid.Domain * int) voption =
        Guid.tryGetLocalId guid

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values that contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the node ID.</returns>
    [<CompiledName("GuidNodeIdPattern"); return: Struct>]
    let (|GuidNodeId|_|) (guid: Guid) : byte[] voption =
        Guid.tryGetNodeId guid

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values whose value is all zeros.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit value option.</returns>
    [<CompiledName("GuidEmptyPattern"); return: Struct>]
    let (|GuidEmpty|_|) (guid: Guid) : unit voption =
        Guid.(=)(guid, Guid.empty) |> ValueOption.ofBool

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 1.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the timestamp, clock sequence and node ID.</returns>
    [<CompiledName("GuidVersion1Pattern"); return: Struct>]
    let (|GuidVersion1|_|) (guid: Guid) : struct (DateTime * int16 * byte[]) voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version1 -> ValueSome struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 2.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the timestamp, clock sequence,
    /// DCE Security domain and local ID and node ID.</returns>
    [<CompiledName("GuidVersion2Pattern"); return: Struct>]
    let (|GuidVersion2|_|) (guid: Guid)
        : struct (DateTime * int16 * struct (Guid.Domain * int) * byte[]) voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version2 -> ValueSome struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetLocalId guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 3.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the hash data and its bitmask.</returns>
    [<CompiledName("GuidVersion3Pattern"); return: Struct>]
    let (|GuidVersion3|_|) (guid: Guid) : Guid.DataAndMask voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version3 -> ValueSome (
            Guid.tryGetHashData guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 4.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the random data and its bitmask.</returns>
    [<CompiledName("GuidVersion4Pattern"); return: Struct>]
    let (|GuidVersion4|_|) (guid: Guid) : Guid.DataAndMask voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version4 -> ValueSome (
            Guid.tryGetRandomData guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID version 5.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the hash data and its bitmask.</returns>
    [<CompiledName("GuidVersion5Pattern"); return: Struct>]
    let (|GuidVersion5|_|) (guid: Guid) : Guid.DataAndMask voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version5 -> ValueSome (
            Guid.tryGetHashData guid |> ValueOption.get)
        | _, _ -> ValueNone

#if !UUIDREV_DISABLE
    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 6.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the timestamp, clock sequence and node ID.</returns>
    [<CompiledName("GuidVersion6Pattern"); return: Struct>]
    let (|GuidVersion6|_|) (guid: Guid) : struct (DateTime * int16 * byte[]) voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version6 -> ValueSome struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetClockSeq guid |> ValueOption.get,
            Guid.tryGetNodeId guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 7.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the timestamp and random data and its bitmask.</returns>
    [<CompiledName("GuidVersion7Pattern"); return: Struct>]
    let (|GuidVersion7|_|) (guid: Guid) : struct (DateTime * Guid.DataAndMask) voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version7 -> ValueSome struct (
            Guid.tryGetTime guid |> ValueOption.get,
            Guid.tryGetRandomData guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values of RFC 4122 UUID revision version 8.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A value option of the custom data and its bitmask.</returns>
    [<CompiledName("GuidVersion8Pattern"); return: Struct>]
    let (|GuidVersion8|_|) (guid: Guid) : Guid.DataAndMask voption =
        match Guid.isRfc4122 guid, Guid.version guid with
        | true, Guid.Version.Version8 -> ValueSome (
            Guid.tryGetCustomData guid |> ValueOption.get)
        | _, _ -> ValueNone

    /// <summary>
    /// Matches <see cref="T:System.Guid"/> values whose value is all ones.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A unit value option.</returns>
    [<CompiledName("GuidMaxValuePattern"); return: Struct>]
    let (|GuidMaxValue|_|) (guid: Guid) : unit voption =
        Guid.(=)(guid, Guid.maxValue) |> ValueOption.ofBool
#endif
