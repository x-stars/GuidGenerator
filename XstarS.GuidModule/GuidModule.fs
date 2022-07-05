namespace XNetEx

open System
open XNetEx.Guids
open XNetEx.Guids.Generators

[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =

    [<CompiledName("Empty")>]
    let empty = Guid.Empty

    [<CompiledName("NamespaceDns")>]
    let nsDns = GuidNamespaces.Dns

    [<CompiledName("NamespaceUrl")>]
    let nsUrl = GuidNamespaces.Url

    [<CompiledName("NamespaceOid")>]
    let nsOid = GuidNamespaces.Oid

    [<CompiledName("NamespaceX500")>]
    let nsX500 = GuidNamespaces.X500

    [<CompiledName("DomainPerson")>]
    let domainPerson = DceSecurityDomain.Person

    [<CompiledName("DomainGroup")>]
    let domainGroup = DceSecurityDomain.Group

    [<CompiledName("DomainOrg")>]
    let domainOrg = DceSecurityDomain.Org

    [<CompiledName("NewVersion1")>]
    let newV1 () =
        GuidGenerator.Version1.NewGuid()

    [<CompiledName("NewVersion2")>]
    let newV2 domain =
        GuidGenerator.Version2.NewGuid(domain)

    [<CompiledName("NewVersion2Org")>]
    let newV2Org localId =
        GuidGenerator.Version2.NewGuid(domainOrg, Nullable localId)

    [<CompiledName("NewVersion3")>]
    let newV3 nsId name =
        GuidGenerator.Version3.NewGuid(nsId, name)

    [<CompiledName("NewVersion3")>]
    let newV3S nsId name =
        GuidGenerator.Version3.NewGuid(nsId, name, null)

    [<CompiledName("NewVersion3")>]
    let newV3Enc nsId encoding name =
        GuidGenerator.Version3.NewGuid(nsId, name, encoding)

    [<CompiledName("NewVersion4")>]
    let newV4 () =
        GuidGenerator.Version4.NewGuid()

    [<CompiledName("NewVersion5")>]
    let newV5 nsId name =
        GuidGenerator.Version5.NewGuid(nsId, name)

    [<CompiledName("NewVersion5")>]
    let newV5S nsId name =
        GuidGenerator.Version5.NewGuid(nsId, name, null)

    [<CompiledName("NewVersion5")>]
    let newV5Enc nsId encoding name =
        GuidGenerator.Version5.NewGuid(nsId, name, encoding)

    [<CompiledName("FromFields")>]
    let ofFields (a : int) b c d e f g h i j k =
        Guid(a, b, c, d, e, f, g, h, i, j, k)

    [<CLSCompliant(false)>]
    [<CompiledName("FromFields")>]
    let ofFieldsU (a : uint) b c d e f g h i j k =
        Guid(a, b, c, d, e, f, g, h, i, j, k)

    [<CompiledName("FromByteArray")>]
    let ofBytes (bytes : byte[]) = Guid(bytes)

    [<CompiledName("FromUuidByteArray")>]
    let ofUuidBytes (bytes : byte[]) =
        Guid(bytes) |> ignore
        let copy = Array.copy bytes
        copy.[0] <- bytes.[3]
        copy.[1] <- bytes.[2]
        copy.[2] <- bytes.[1]
        copy.[3] <- bytes.[0]
        copy.[4] <- bytes.[5]
        copy.[5] <- bytes.[4]
        copy.[6] <- bytes.[7]
        copy.[7] <- bytes.[6]
        Guid(copy)

    [<CompiledName("ToByteArray")>]
    let toBytes (guid : Guid) =
        guid.ToByteArray()

    [<CompiledName("ToUuidByteArray")>]
    let toUuidBytes (guid : Guid) =
        guid.ToUuidByteArray()

    [<CompiledName("Parse")>]
    let parse (input : string) =
        Guid.Parse(input)

    [<CompiledName("ParseExact")>]
    let parseExact (format : string) (input : string) =
        Guid.ParseExact(input, format)

    [<CompiledName("Format")>]
    let format (format : string) (guid : Guid) =
        guid.ToString(format)

    [<CompiledName("GetVariant")>]
    let variant (guid : Guid) = guid.GetVariant()

    [<CompiledName("GetVersion")>]
    let version (guid : Guid) = guid.GetVersion()

    [<CompiledName("IsRfc4122")>]
    let internal isRfc4122 (guid : Guid) =
        variant guid = GuidVariant.Rfc4122

    [<CompiledName("IsTimeBased")>]
    let isTimeBased (guid : Guid) =
        (isRfc4122 guid) && (version guid).IsTimeBased()

    [<CompiledName("IsNameBased")>]
    let isNameBased (guid : Guid) =
        (isRfc4122 guid) && (version guid).IsNameBased()

    [<CompiledName("IsRandomized")>]
    let isRandomized (guid : Guid) =
        (isRfc4122 guid) && (version guid).IsRandomized()

    [<CompiledName("ContainsLocalId")>]
    let hasLocalId (guid : Guid) =
        (isRfc4122 guid) && (version guid).ContainsLocalId()

    [<CompiledName("ContainsNodeId")>]
    let hasNodeId (guid : Guid) =
        (isRfc4122 guid) && (version guid).ContainsNodeId()

    [<CompiledName("TryGetTimestamp")>]
    let tryGetTimestamp (guid : Guid) =
        let result, timestamp = guid.TryGetTimestamp()
        if result then Some timestamp else None

    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid : Guid) =
        let result, clockSeq = guid.TryGetClockSequence()
        if result then Some clockSeq else None

    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid : Guid) =
        let result, domain, localId = guid.TryGetDomainAndLocalId()
        if result then Some (domain, localId) else None

    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid : Guid) =
        let result, nodeId = guid.TryGetNodeId()
        if result then Some nodeId else None
