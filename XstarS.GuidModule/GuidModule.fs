namespace XNetEx

open System
open System.Runtime.InteropServices
open XNetEx.Guids
open XNetEx.Guids.Generators

/// <summary>
/// Contains operations for working with values of type <see cref="T:System.Guid"/>.
/// </summary>
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =

    /// <summary>
    /// Provides the fields of a <see cref="T:System.Guid"/> defined in RFC 4122.
    /// </summary>
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

    /// <summary>
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.GuidVariant"/>.
    /// </summary>
    type Variant = GuidVariant

    /// <summary>
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.GuidVersion"/>.
    /// </summary>
    type Version = GuidVersion

    /// <summary>
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.DceSecurityDomain"/>.
    /// </summary>
    type Domain = DceSecurityDomain

    /// <summary>
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.GuidNamespaces"/>.
    /// </summary>
    type Namespace = GuidNamespaces

    /// <summary>
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.Generators.GuidGenerator"/>.
    /// </summary>
    type Generator = GuidGenerator

    /// <summary>
    /// Represents the <see cref="T:System.Guid"/> instance whose value is all zeros.
    /// </summary>
    [<CompiledName("Empty")>]
    let empty: Guid = Guid.Empty

    /// <summary>
    /// Represents the namespace ID of a fully-qualified domain name.
    /// </summary>
    [<CompiledName("NamespaceDns")>]
    let nsDns: Guid = Namespace.Dns

    /// <summary>
    /// Represents the namespace ID of a URL.
    /// </summary>
    [<CompiledName("NamespaceUrl")>]
    let nsUrl: Guid = Namespace.Url

    /// <summary>
    /// Represents the namespace ID of an ISO OID.
    /// </summary>
    [<CompiledName("NamespaceOid")>]
    let nsOid: Guid = Namespace.Oid

    /// <summary>
    /// Represents the namespace ID of an X.500 DN.
    /// </summary>
    [<CompiledName("NamespaceX500")>]
    let nsX500: Guid = Namespace.X500

    /// <summary>
    /// Represents the DCE Security principal domain.
    /// </summary>
    [<CompiledName("DomainPerson")>]
    let domainPerson: Domain = Domain.Person

    /// <summary>
    /// Represents the DCE Security group domain.
    /// </summary>
    [<CompiledName("DomainGroup")>]
    let domainGroup: Domain = Domain.Group

    /// <summary>
    /// Represents the DCE Security organization domain.
    /// </summary>
    [<CompiledName("DomainOrg")>]
    let domainOrg: Domain = Domain.Org

    /// <summary>
    /// Converts the result of a C#-style try-operation to a <see cref="T:Microsoft.FSharp.Core.voption`1"/>.
    /// </summary>
    /// <param name="res">The value indicates whether the try-operation was successful.</param>
    /// <param name="out">The output value the try-operation.</param>
    /// <returns>A <see cref="T:Microsoft.FSharp.Core.voption`1"/> of the try-operation result.</returns>
    [<CompiledName("ToValueOption")>]
    let inline private toVOption (res, out) =
        if res then ValueSome out else ValueNone

    /// <summary>
    /// Converts the result of a C#-style try-operation to a <see cref="T:Microsoft.FSharp.Core.voption`1"/>.
    /// </summary>
    /// <param name="res">The value indicates whether the try-operation was successful.</param>
    /// <param name="out1">The first output value the try-operation.</param>
    /// <param name="out2">The second output value the try-operation.</param>
    /// <returns>A <see cref="T:Microsoft.FSharp.Core.voption`1"/> of the try-operation result.</returns>
    [<CompiledName("ToValueOption2")>]
    let inline private toVOption2 (res, out1, out2) =
        if res then ValueSome struct (out1, out2) else ValueNone

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.</returns>
    [<CompiledName("NewVersion1")>]
    let newV1 () = Generator.Version1.NewGuid()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID version 1 using a random node ID.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID version 1 using a random node ID.</returns>
    [<CompiledName("NewVersion1RSequence")>]
    let newV1RSeq () =
        seq { let guidGen = Generator.CreateVersion1R()
              while true do yield guidGen.NewGuid() }

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 based on the specified DCE Security domain.
    /// </summary>
    /// <param name="domain">The DCE Security domain.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="domain"/> is not a valid <see cref="T:XNetEx.Guid.Domain"/> value.</exception>
    [<CompiledName("NewVersion2")>]
    let newV2 (domain: Domain) =
        Generator.Version2.NewGuid(domain)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 of the organization domain based on the specified local ID.
    /// </summary>
    /// <param name="localId">The local ID of the organization.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    [<CompiledName("NewVersion2OfOrg")>]
    let newV2Org (localId: int) =
        Generator.Version2.NewGuid(domainOrg, localId)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 3 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 3.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion3")>]
    let newV3 (nsId: Guid) (name: byte[]) =
        Generator.Version3.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 3 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string (encoded in UTF-8).</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 3.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion3ByString")>]
    let newV3S (nsId: Guid) (name: string) =
        Generator.Version3.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 3 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string (encoded in <paramref name="enc"/>).</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 3.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion3ByEncoding")>]
    let newV3Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version3.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 4.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 5.</returns>
    [<CompiledName("NewVersion4")>]
    let newV4 () = Generator.Version4.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 5 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 5.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion5")>]
    let newV5 (nsId: Guid) (name: byte[]) =
        Generator.Version5.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 5 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string (encoded in UTF-8).</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 5.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion5ByString")>]
    let newV5S (nsId: Guid) (name: string) =
        Generator.Version5.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 5 based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string (encoded in <paramref name="enc"/>).</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 5.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion5ByEncoding")>]
    let newV5Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version5.NewGuid(nsId, name, enc)

    /// <summary>
    /// Creates a new <see cref="T:System.Guid"/> instance
    /// by using the specified fields of integers and bytes.
    /// </summary>
    /// <param name="a">The first 4 bytes of the GUID.</param>
    /// <param name="b">The next 2 bytes of the GUID.</param>
    /// <param name="c">The next 2 bytes of the GUID.</param>
    /// <param name="d">The next byte of the GUID.</param>
    /// <param name="e">The next byte of the GUID.</param>
    /// <param name="f">The next byte of the GUID.</param>
    /// <param name="g">The next byte of the GUID.</param>
    /// <param name="h">The next byte of the GUID.</param>
    /// <param name="i">The next byte of the GUID.</param>
    /// <param name="j">The next byte of the GUID.</param>
    /// <param name="k">The next byte of the GUID.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance
    /// of the specified fields of integers and bytes.</returns>
    [<CompiledName("OfFields")>]
    let ofFields (a: int) b c (d, e) (f, g, h, i, j, k) =
        Guid(a, b, c, d, e, f, g, h, i, j, k)

    /// <summary>
    /// Creates a new <see cref="T:System.Guid"/> instance
    /// by using the specified byte array of fields in little-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element byte array containing
    /// fields of the GUID in little-endian order.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="bytes"/> not 16 bytes long.</exception>
    [<CompiledName("OfByteArray")>]
    let ofBytes (bytes: byte[]) = Guid(bytes)

    /// <summary>
    /// Creates a new <see cref="T:System.Guid"/> instance
    /// by using the specified byte array of fields in big-endian order (RFC 4122 compliant).
    /// </summary>
    /// <param name="bytes">A 16-element byte array containing
    /// fields of the GUID in big-endian order (RFC 4122 compliant).</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="bytes"/> not 16 bytes long.</exception>
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

    /// <summary>
    /// Returns fields of integers and bytes of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An 11-element tuple that contains fields of the <see cref="T:System.Guid"/>.</returns>
    [<CompiledName("ToFields")>]
    let toFields (guid: Guid) =
        let fields = Rfc4122Fields(Guid = guid)
        struct (fields.TimeLow, fields.TimeMid, fields.TimeHi_Ver,
                struct (fields.ClkSeqHi_Var, fields.ClkSeqLow),
                struct (fields.NodeId0, fields.NodeId1, fields.NodeId2,
                        fields.NodeId3, fields.NodeId4, fields.NodeId5))

    /// <summary>
    /// Returns a 16-element byte array that contains fields
    /// in little-endian order of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields
    /// of the <see cref="T:System.Guid"/> in little-endian order.</returns>
    [<CompiledName("ToByteArray")>]
    let toBytes (guid: Guid) = guid.ToByteArray()

    /// <summary>
    /// Returns a 16-element byte array that contains fields
    /// in big-endian order of the <see cref="T:System.Guid"/> (RFC 4122 compliant).
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields
    /// of the <see cref="T:System.Guid"/> in big-endian order (RFC 4122 compliant).</returns>
    [<CompiledName("ToUuidByteArray")>]
    let toBytesUuid (guid: Guid) = guid.ToUuidByteArray()

    /// <summary>
    /// Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid"/> instance.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input"/> is not in a recognized format.</exception>
    [<CompiledName("Parse")>]
    let parse (input: string) = Guid.Parse(input)

    /// <summary>
    /// Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid"/> instance,
    /// provided that the string is in the specified format.
    /// </summary>
    /// <param name="format">One of the following specifiers that indicates the exact format
    /// to use when interpreting input: "N", "D", "B", "P", or "X".</param>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input"/> or <paramref name="format"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input"/> is not in the format specified by <paramref name="format"/>.</exception>
    [<CompiledName("ParseExact")>]
    let parseExact (format: string) (input: string) =
        Guid.ParseExact(input, format)

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance. Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParse")>]
    let tryParse (input: string) =
        Guid.TryParse(input) |> toVOption

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance, provided that the string is in the specified format.
    /// Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="format">One of the following specifiers that indicates the exact format
    /// to use when interpreting input: "N", "D", "B", "P", or "X".</param>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParseExact")>]
    let tryParseExact (format: string) (input: string) =
        Guid.TryParseExact(input, format) |> toVOption

    /// <summary>
    /// Returns a string representation of the value of the <see cref="T:System.Guid"/>,
    /// according to the provided format specifier.
    /// </summary>
    /// <param name="format">A single format specifier that indicates
    /// how to format the value of the <see cref="T:System.Guid"/>.
    /// The format parameter can be "N", "D", "B", "P", or "X".</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The value of the <see cref="T:System.Guid"/>, represented as a series
    /// of lowercase hexadecimal digits in the specified format.</returns>
    /// <exception cref="T:System.FormatException">
    /// The value of format is not <see langword="null"/>,
    /// an empty string (""), "N", "D", "B", "P", or "X".</exception>
    [<CompiledName("Format")>]
    let format (format: string) (guid: Guid) =
        guid.ToString(format)

    /// <summary>
    /// Gets the variant of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The variant of the <see cref="T:System.Guid"/>.</returns>
    [<CompiledName("GetVariant")>]
    let variant (guid: Guid) : Variant = guid.GetVariant()

    /// <summary>
    /// Gets the version of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The version of the <see cref="T:System.Guid"/>.</returns>
    [<CompiledName("GetVersion")>]
    let version (guid: Guid) : Version = guid.GetVersion()

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/>
    /// is of the RFC 4122 variant (<see cref="F:XNetEx.Guid.Variant.Rfc4122"/>).
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// is of the RFC 4122 variant; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsRfc4122Uuid")>]
    let internal isRfc4122 (guid: Guid) =
        variant guid = GuidVariant.Rfc4122

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/>
    /// is generated based on the current time.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// is generated based on the current time; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsTimeBased")>]
    let isTimeBased (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsTimeBased()

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/>
    /// is generated based on the input name.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// is generated based on the input name; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsNameBased")>]
    let isNameBased (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsNameBased()

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/> is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// is generated randomly; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsRandomized")>]
    let isRandomized (guid: Guid) =
        (isRfc4122 guid) && (version guid).IsRandomized()

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/> contains local ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// contains local ID data; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsLocalId")>]
    let hasLocalId (guid: Guid) =
        (isRfc4122 guid) && (version guid).ContainsLocalId()

    /// <summary>
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/> contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// contains node ID data; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsNodeId")>]
    let hasNodeId (guid: Guid) =
        (isRfc4122 guid) && (version guid).ContainsNodeId()

    /// <summary>
    /// Tries to get the timestamp represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The timestamp represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.</returns>
    [<CompiledName("TryGetTimestamp")>]
    let tryGetTime (guid: Guid) : DateTime voption =
        guid.TryGetTimestamp() |> toVOption

    /// <summary>
    /// Tries to get the clock sequence represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The clock sequence represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.</returns>
    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid: Guid) : int16 voption =
        guid.TryGetClockSequence() |> toVOption

    /// <summary>
    /// Tries to get the DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE security UUID.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE security UUID.</returns>
    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid: Guid) : struct (Domain * int) voption =
        guid.TryGetDomainAndLocalId() |> toVOption2

    /// <summary>
    /// Tries to get the node ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The node ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not time-based.</returns>
    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid: Guid) : byte[] voption =
        guid.TryGetNodeId() |> toVOption
