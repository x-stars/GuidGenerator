namespace XNetEx.FSharp.Core

open System
open XNetEx.Guids
open XNetEx.Guids.Generators

/// <summary>
/// Contains operations for working with values of type <see cref="T:System.Guid"/>.
/// </summary>
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =

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
    let empty = Guid.Empty

    /// <summary>
    /// Represents the <see cref="T:System.Guid"/> instance whose value is all ones.
    /// </summary>
    [<CompiledName("MaxValue")>]
    let maxValue = Uuid.MaxValue

    /// <summary>
    /// Represents the namespace ID of a fully-qualified domain name.
    /// </summary>
    [<CompiledName("NamespaceDns")>]
    let nsDns = Namespace.Dns

    /// <summary>
    /// Represents the namespace ID of a URL.
    /// </summary>
    [<CompiledName("NamespaceUrl")>]
    let nsUrl = Namespace.Url

    /// <summary>
    /// Represents the namespace ID of an ISO OID.
    /// </summary>
    [<CompiledName("NamespaceOid")>]
    let nsOid = Namespace.Oid

    /// <summary>
    /// Represents the namespace ID of an X.500 DN.
    /// </summary>
    [<CompiledName("NamespaceX500")>]
    let nsX500 = Namespace.X500

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.</returns>
    [<CompiledName("NewVersion1")>]
    let newV1 () = Generator.Version1.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.</returns>
    [<CompiledName("NewVersion1R")>]
    let newV1R () = Generator.Version1R.NewGuid()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID version 1 using a volatile random node ID.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID version 1 using a volatile random node ID.</returns>
    [<CompiledName("NewVersion1RSequence")>]
    let newV1RSeq () =
        let guidGen = Generator.CreateVersion1R()
        seq { while true do yield guidGen.NewGuid() }

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 based on the specified DCE Security domain.
    /// </summary>
    /// <param name="domain">The DCE Security domain.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// The current operating system does not support getting the local user or group ID.</exception>
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
        Generator.Version2.NewGuid(Domain.Org, localId)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 based on the specified DCE Security domain and local ID.
    /// </summary>
    /// <param name="domain">The DCE Security domain.</param>
    /// <param name="localId">The local ID of the domain.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    [<CompiledName("NewVersion2OfOther")>]
    let newV2Other (domain: Domain) (localId: int) =
        Generator.Version2.NewGuid(domain, localId)

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
    /// <param name="name">The name string.</param>
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
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 3.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion3ByEncoding")>]
    let newV3Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version3.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 4.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 4.</returns>
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
    /// <param name="name">The name string.</param>
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
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 5.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion5ByEncoding")>]
    let newV5Enc (nsId: Guid) (enc: Text.Encoding) (name: string) =
        Generator.Version5.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 6.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 6.</returns>
    [<CompiledName("NewVersion6")>]
    let newV6 () = Generator.Version6.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 6
    /// using a physical (IEEE 802 MAC) address node ID.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 6
    /// using a physical (IEEE 802 MAC) address node ID.</returns>
    [<CompiledName("NewVersion6P")>]
    let newV6P () = Generator.Version6P.NewGuid()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID revision version 6 using a volatile random node ID.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/>
    /// instances of RFC 4122 UUID revision version 6 using a volatile random node ID.</returns>
    [<CompiledName("NewVersion6Sequence")>]
    let newV6Seq () =
        let guidGen = Generator.CreateVersion6()
        seq { while true do yield guidGen.NewGuid() }

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 7.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 7.</returns>
    [<CompiledName("NewVersion7")>]
    let newV7 () = Generator.Version7.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 8
    /// example implementation (UUIDREV Appendix A.5).
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID revision version 8
    /// example implementation (UUIDREV Appendix A.5).</returns>
    [<CompiledName("NewVersion8")>]
    let newV8 () = Generator.Version8.NewGuid()

    /// <summary>
    /// Loads the GUID generator state from the specified storage file
    /// and returns a value that indicates whether the loading operation is successful.
    /// </summary>
    /// <param name="fileName">The path of the state storage file,
    /// or <see langword="null"/> to disable the state storage.</param>
    /// <returns><see langword="true"/> if the state storage loading operation
    /// is successful; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("LoadGeneratorState")>]
    let loadState (fileName: string) =
        Generator.SetStateStorageFile(fileName)

    /// <summary>
    /// Connects a listener function to the event that occurs
    /// when a GUID generator state storage I/O operation throws an exception.
    /// </summary>
    /// <param name="callback">The function to call when the event is triggered.</param>
    [<CompiledName("OnStateStorageException")>]
    let onStateExn (callback: StateStorageExceptionEventArgs -> unit) =
        Generator.StateStorageException.Add(callback)

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
    /// <param name="bytes">A 16-element byte array
    /// containing fields of the GUID in little-endian order.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="bytes"/> not 16 bytes long.</exception>
    [<CompiledName("OfByteArray")>]
    let ofBytes (bytes: byte[]) = Guid(bytes)

    /// <summary>
    /// Creates a new <see cref="T:System.Guid"/> instance
    /// by using the specified byte array of fields in big-endian order.
    /// </summary>
    /// <param name="bytes">A 16-element byte array
    /// containing fields of the GUID in big-endian order.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of the specified byte array.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bytes"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="bytes"/> not 16 bytes long.</exception>
    [<CompiledName("OfUuidByteArray")>]
    let ofBytesUuid (bytes: byte[]) =
        Uuid.FromByteArray(bytes)

    /// <summary>
    /// Returns fields of integers and bytes of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An 11-element tuple that contains fields of the <see cref="T:System.Guid"/>.</returns>
    [<CompiledName("ToFields")>]
    let toFields (guid: Guid) =
        let (a, b, c, d, e, f, g, h, i, j, k) = guid.Deconstruct()
        struct (a, b, c, struct (d, e), struct (f, g, h, i, j, k))

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
    /// in big-endian order of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields
    /// of the <see cref="T:System.Guid"/> in big-endian order.</returns>
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
    /// Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid"/> instance,
    /// provided that the string is in the URN format.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input"/> is not in the URN format.</exception>
    [<CompiledName("ParseUrn")>]
    let parseUrn (input: string) = Uuid.ParseUrn(input)

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance. Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParse")>]
    let tryParse (input: string) =
        Guid.TryParse(input) |> TryResult.toVOption

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
        Guid.TryParseExact(input, format) |> TryResult.toVOption

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance, provided that the string is in the URN format.
    /// Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParseUrn")>]
    let tryParseUrn (input: string) =
        Uuid.TryParseUrn(input) |> TryResult.toVOption

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
    /// Returns a URN string representation of the value of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The value of the <see cref="T:System.Guid"/>, represented as a series
    /// of lowercase hexadecimal digits in the URN format.</returns>
    [<CompiledName("FormatUrn")>]
    let formatUrn (guid: Guid) = guid.ToUrnString()

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
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/> is of the RFC 4122 variant.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// is of the RFC 4122 variant; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsRfc4122Uuid")>]
    let internal isRfc4122 (guid: Guid) =
        variant guid = Variant.Rfc4122

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
    /// Gets a value that indicates whether a <see cref="T:System.Guid"/> contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if a <see cref="T:System.Guid"/>
    /// contains a clock sequence; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsClockSequence")>]
    let hasClockSeq (guid: Guid) =
        (isRfc4122 guid) && (version guid).ContainsClockSequence()

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
        guid.TryGetTimestamp() |> TryResult.toVOption

    /// <summary>
    /// Tries to get the clock sequence represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The clock sequence represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain a clock sequence.</returns>
    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid: Guid) : int16 voption =
        guid.TryGetClockSequence() |> TryResult.toVOption

    /// <summary>
    /// Tries to get the DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE Security UUID.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE Security UUID.</returns>
    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid: Guid) : struct (Domain * int) voption =
        guid.TryGetDomainAndLocalId() |> TryResult.toVOption2

    /// <summary>
    /// Tries to get the node ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The node ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain node ID data.</returns>
    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid: Guid) : byte[] voption =
        guid.TryGetNodeId() |> TryResult.toVOption
