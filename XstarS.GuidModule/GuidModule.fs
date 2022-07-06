namespace XNetEx

open System
open System.Runtime.InteropServices
open XNetEx.Guids
open XNetEx.Guids.Generators

[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =

    [<Struct; StructLayout(LayoutKind.Explicit)>]
    [<NoEquality; NoComparison; AutoSerializable(false)>]
    type internal Rfc4122Fields =
        [<FieldOffset( 0)>] val mutable Guid: Guid
        [<FieldOffset( 0)>] val mutable TimeLow: int
        [<FieldOffset( 4)>] val mutable TimeMid: int16
        [<FieldOffset( 6)>] val mutable TimeHi_Var: int16
        [<FieldOffset( 8)>] val mutable ClkSeqHi_Ver: byte
        [<FieldOffset( 9)>] val mutable ClkSeqLow: byte
        [<FieldOffset(10)>] val mutable NodeId0: byte
        [<FieldOffset(11)>] val mutable NodeId1: byte
        [<FieldOffset(12)>] val mutable NodeId2: byte
        [<FieldOffset(13)>] val mutable NodeId3: byte
        [<FieldOffset(14)>] val mutable NodeId4: byte
        [<FieldOffset(15)>] val mutable NodeId5: byte

    type Variant = GuidVariant

    type Version = GuidVersion

    type Domain = DceSecurityDomain

    type Namespace = GuidNamespaces

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

    [<CompiledName("ToValueOption")>]
    let inline private toVOption (result, output) =
        if result then ValueSome output else ValueNone

    [<CompiledName("ToValueOption2")>]
    let inline private toVOption2 (result, output1, output2) =
        if result then ValueSome struct (output1, output2) else ValueNone

    [<CompiledName("NewVersion1")>]
    let newV1 () =
        GuidGenerator.Version1.NewGuid()

    [<CompiledName("NewVersion2")>]
    let newV2 domain =
        GuidGenerator.Version2.NewGuid(domain)

    [<CompiledName("NewVersion2OfOrg")>]
    let newV2Org localId =
        GuidGenerator.Version2.NewGuid(domainOrg, Nullable localId)

    [<CompiledName("NewVersion3")>]
    let newV3 nsId name =
        GuidGenerator.Version3.NewGuid(nsId, name)

    [<CompiledName("NewVersion3ByString")>]
    let newV3S nsId name =
        GuidGenerator.Version3.NewGuid(nsId, name, null)

    [<CompiledName("NewVersion3ByEncoding")>]
    let newV3Enc nsId encoding name =
        GuidGenerator.Version3.NewGuid(nsId, name, encoding)

    [<CompiledName("NewVersion4")>]
    let newV4 () =
        GuidGenerator.Version4.NewGuid()

    [<CompiledName("NewVersion5")>]
    let newV5 nsId name =
        GuidGenerator.Version5.NewGuid(nsId, name)

    [<CompiledName("NewVersion5ByString")>]
    let newV5S nsId name =
        GuidGenerator.Version5.NewGuid(nsId, name, null)

    [<CompiledName("NewVersion5ByEncoding")>]
    let newV5Enc nsId encoding name =
        GuidGenerator.Version5.NewGuid(nsId, name, encoding)

    [<CompiledName("OfFields")>]
    let ofFields (a: int) b c d e f g h i j k =
        Guid(a, b, c, d, e, f, g, h, i, j, k)

    [<CompiledName("OfByteArray")>]
    let ofBytes (bytes: byte[]) = Guid(bytes)

    [<CompiledName("OfUuidByteArray")>]
    let ofBytesUuid (bytes: byte[]) =
        Guid(bytes) |> ignore
        let revBytes index =
            match index with
            | 0 | 1 | 2 | 3 -> bytes.[(0 + 3) - index]
            | 4 | 5 -> bytes.[(4 + 5) - index]
            | 6 | 7 -> bytes.[(6 + 7) - index]
            | _ -> bytes.[index]
        Guid(Array.init 16 revBytes)

    [<CompiledName("ToFields")>]
    let toFields (guid: Guid) =
        let fields = Rfc4122Fields(Guid = guid)
        struct (fields.TimeLow, fields.TimeMid, fields.TimeHi_Var,
                struct (fields.ClkSeqHi_Ver, fields.ClkSeqLow),
                struct (fields.NodeId0, fields.NodeId1, fields.NodeId2,
                        fields.NodeId3, fields.NodeId4, fields.NodeId5))

    [<CompiledName("ToByteArray")>]
    let toBytes (guid: Guid) = guid.ToByteArray()

    [<CompiledName("ToUuidByteArray")>]
    let toBytesUuid (guid: Guid) = guid.ToUuidByteArray()

    [<CompiledName("Parse")>]
    let parse (input: string) = Guid.Parse(input)

    [<CompiledName("ParseExact")>]
    let parseExact (format: string) (input: string) =
        Guid.ParseExact(input, format)

    [<CompiledName("TryParse")>]
    let tryParse (input: string) =
        Guid.TryParse(input) |> toVOption

    [<CompiledName("TryParseExact")>]
    let tryParseExact (format: string) (input: string) =
        Guid.TryParseExact(input, format) |> toVOption

    [<CompiledName("Format")>]
    let format (format: string) (guid: Guid) =
        guid.ToString(format)

    [<CompiledName("GetVariant")>]
    let variant (guid: Guid) = guid.GetVariant()

    [<CompiledName("GetVersion")>]
    let version (guid: Guid) = guid.GetVersion()

    [<CompiledName("IsRfc4122Uuid")>]
    let internal isRfc4122 (guid: Guid) =
        variant guid = GuidVariant.Rfc4122

    [<CompiledName("IsTimeBased")>]
    let isTimeBased (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsTimeBased()

    [<CompiledName("IsNameBased")>]
    let isNameBased (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsNameBased()

    [<CompiledName("IsRandomized")>]
    let isRandomized (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsRandomized()

    [<CompiledName("ContainsLocalId")>]
    let hasLocalId (guid: Guid) =
        (isRfc4122 guid) && (version guid).ContainsLocalId()

    [<CompiledName("ContainsNodeId")>]
    let hasNodeId (guid: Guid) =
        (isRfc4122 guid) && (version guid).ContainsNodeId()

    [<CompiledName("TryGetTimestamp")>]
    let tryGetTimestamp (guid: Guid) =
        guid.TryGetTimestamp() |> toVOption

    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid: Guid) =
        guid.TryGetClockSequence() |> toVOption

    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid: Guid) =
        guid.TryGetDomainAndLocalId() |> toVOption2

    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid: Guid) =
        guid.TryGetNodeId() |> toVOption
