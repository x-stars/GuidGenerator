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
        [<FieldOffset( 6)>] val mutable TimeHi_Ver: int16
        [<FieldOffset( 8)>] val mutable ClkSeqHi_Var: byte
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

    type Generator = GuidGenerator

    [<CompiledName("Empty")>]
    let empty: Guid = Guid.Empty

    [<CompiledName("NamespaceDns")>]
    let nsDns: Guid = Namespace.Dns

    [<CompiledName("NamespaceUrl")>]
    let nsUrl: Guid = Namespace.Url

    [<CompiledName("NamespaceOid")>]
    let nsOid: Guid = Namespace.Oid

    [<CompiledName("NamespaceX500")>]
    let nsX500: Guid = Namespace.X500

    [<CompiledName("DomainPerson")>]
    let domainPerson: Domain = Domain.Person

    [<CompiledName("DomainGroup")>]
    let domainGroup: Domain = Domain.Group

    [<CompiledName("DomainOrg")>]
    let domainOrg: Domain = Domain.Org

    [<CompiledName("ToValueOption")>]
    let inline private toVOption (res, out) =
        if res then ValueSome out else ValueNone

    [<CompiledName("ToValueOption2")>]
    let inline private toVOption2 (res, out1, out2) =
        if res then ValueSome struct (out1, out2) else ValueNone

    [<CompiledName("NewVersion1")>]
    let newV1 () = Generator.Version1.NewGuid()

    [<CompiledName("NewVersion1RSequence")>]
    let newV1RSeq () =
        let guidGen = Generator.CreateVersion1R()
        Seq.initInfinite (fun _ -> guidGen.NewGuid())

    [<CompiledName("NewVersion2")>]
    let newV2 (domain: Domain) =
        Generator.Version2.NewGuid(domain)

    [<CompiledName("NewVersion2OfOrg")>]
    let newV2Org (localId: int) =
        Generator.Version2.NewGuid(domainOrg, localId)

    [<CompiledName("NewVersion3")>]
    let newV3 (nsId: Guid) (name: byte[]) =
        Generator.Version3.NewGuid(nsId, name)

    [<CompiledName("NewVersion3ByString")>]
    let newV3S (nsId: Guid) (name: string) =
        Generator.Version3.NewGuid(nsId, name)

    [<CompiledName("NewVersion3ByEncoding")>]
    let newV3Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version3.NewGuid(nsId, name, enc)

    [<CompiledName("NewVersion4")>]
    let newV4 () = Generator.Version4.NewGuid()

    [<CompiledName("NewVersion5")>]
    let newV5 (nsId: Guid) (name: byte[]) =
        Generator.Version5.NewGuid(nsId, name)

    [<CompiledName("NewVersion5ByString")>]
    let newV5S (nsId: Guid) (name: string) =
        Generator.Version5.NewGuid(nsId, name)

    [<CompiledName("NewVersion5ByEncoding")>]
    let newV5Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version5.NewGuid(nsId, name, enc)

    [<CompiledName("OfFields")>]
    let ofFields (a: int) b c (d, e) (f, g, h, i, j, k) =
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
        struct (fields.TimeLow, fields.TimeMid, fields.TimeHi_Ver,
                struct (fields.ClkSeqHi_Var, fields.ClkSeqLow),
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
    let variant (guid: Guid) : Variant = guid.GetVariant()

    [<CompiledName("GetVersion")>]
    let version (guid: Guid) : Version = guid.GetVersion()

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
    let tryGetTime (guid: Guid) : DateTime voption =
        guid.TryGetTimestamp() |> toVOption

    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid: Guid) : int16 voption =
        guid.TryGetClockSequence() |> toVOption

    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid: Guid) : struct (Domain * int) voption =
        guid.TryGetDomainAndLocalId() |> toVOption2

    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid: Guid) : byte[] voption =
        guid.TryGetNodeId() |> toVOption
