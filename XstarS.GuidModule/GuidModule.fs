﻿namespace XNetEx.FSharp.Core

open System
open FSharp.Core.CompilerServices
open XNetEx.Guids
open XNetEx.Guids.Generators
#if !UUIDREV_DISABLE
open System.Security.Cryptography
#endif

/// <summary>
/// An abbreviation for the CLI type <see cref="T:System.Guid"/>.
/// </summary>
type Guid = System.Guid

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
    /// An abbreviation for the type <see cref="T:XNetEx.Guids.Generators.NodeIdSource"/>.
    /// </summary>
    type NodeIdSource = Generators.NodeIdSource

    /// <summary>
    /// A byte array value pair type abbreviation that represents GUID data and its bitmask.
    /// </summary>
    type DataAndMask = (struct (byte[] * byte[]))

    /// <summary>
    /// Contains operations for working with values of type
    /// <see cref="T:XNetEx.Guids.Generators.GuidGenerator"/>.
    /// </summary>
    [<RequireQualifiedAccess>]
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Generator =

        /// <summary>
        /// An abbreviation for the type
        /// <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.
        /// </summary>
        type CustomStateBuilder = CustomStateGuidGeneratorBuilder

    /// <summary>
    /// Represents the <see cref="T:System.Guid"/> instance whose value is all zeros.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("Empty")>]
    let empty: Guid = Guid.Empty

#if !UUIDREV_DISABLE
    /// <summary>
    /// Represents the <see cref="T:System.Guid"/> instance whose value is all ones.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("MaxValue")>]
    let maxValue: Guid = Uuid.MaxValue
#endif

    /// <summary>
    /// Represents the namespace ID for a fully-qualified domain name.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("NamespaceDns")>]
    let nsDns: Guid = Namespace.Dns

    /// <summary>
    /// Represents the namespace ID for a URL.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("NamespaceUrl")>]
    let nsUrl: Guid = Namespace.Url

    /// <summary>
    /// Represents the namespace ID for an ISO OID.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("NamespaceOid")>]
    let nsOid: Guid = Namespace.Oid

    /// <summary>
    /// Represents the namespace ID for an X.500 DN.
    /// </summary>
    [<ValueAsStaticProperty>]
    [<CompiledName("NamespaceX500")>]
    let nsX500: Guid = Namespace.X500

    /// <summary>
    /// Returns a new RFC 4122 <see cref="T:System.Guid"/> instance of the specified
    /// <see cref="T:XNetEx.Guids.GuidVersion"/> with all other fields set to zero.
    /// </summary>
    /// <param name="version">The version of the new <see cref="T:System.Guid"/> instance.</param>
    /// <returns>A new RFC 4122 <see cref="T:System.Guid"/> instance of
    /// <paramref name="version"/> with all other fields set to zero.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="version"/>
    /// is not a valid <see cref="T:XNetEx.Guids.GuidVersion"/> value.</exception>
    [<CompiledName("EmptyOf")>]
    let emptyOf (version: Version) : Guid = Uuid.EmptyOf(version)

    /// <summary>
    /// Returns a new RFC 4122 <see cref="T:System.Guid"/> instance of the specified
    /// GUID version number with all other fields set to zero.
    /// </summary>
    /// <param name="version">The version of the new <see cref="T:System.Guid"/> instance.</param>
    /// <returns>A new RFC 4122 <see cref="T:System.Guid"/> instance of
    /// <paramref name="version"/> with all other fields set to zero.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="version"/> is not a valid GUID version number.</exception>
    [<CompiledName("EmptyOfVersion")>]
    let emptyOfVersion (version: byte) : Guid = Uuid.EmptyOf(enumof version)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1.</returns>
    [<CompiledName("NewVersion1")>]
    let newV1 () : Guid = Generator.Version1.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.</returns>
    [<CompiledName("NewVersion1R")>]
    let newV1R () : Guid = Generator.Version1R.NewGuid()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 4122 UUID version 1 using a volatile random node ID.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 4122 UUID version 1 using a volatile random node ID.</returns>
    [<CompiledName("NewVersion1RSequence")>]
    let newV1RSeq () : seq<Guid> =
        Generator.CreateVersion1R().AsSequence()

#if !UUIDREV_DISABLE
    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 4122 UUID version 1 using a volatile random node ID without blocking.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 4122 UUID version 1 using a volatile random node ID without blocking.</returns>
    [<CompiledName("NewVersion1RPooledSequence")>]
    let newV1RPoolSeq () : seq<Guid> =
        Generator.CreatePooled(Func<_>(Generator.CreateVersion1R)).AsSequence()
#endif

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 based on the specified DCE Security domain.
    /// </summary>
    /// <param name="domain">The DCE Security domain.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// The current operating system does not support getting the local user or group ID.</exception>
    [<CompiledName("NewVersion2")>]
    let newV2 (domain: Domain) : Guid =
        Generator.Version2.NewGuid(domain)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 of the organization domain based on the specified local ID.
    /// </summary>
    /// <param name="localId">The local ID of the organization.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    [<CompiledName("NewVersion2OfOrg")>]
    let newV2Org (localId: int) : Guid =
        Generator.Version2.NewGuid(Domain.Org, localId)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 4122 UUID version 2 based on the specified DCE Security domain and local ID.
    /// </summary>
    /// <param name="domain">The DCE Security domain.</param>
    /// <param name="localId">The local ID of the domain.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 2.</returns>
    [<CompiledName("NewVersion2OfOther")>]
    let newV2Other (domain: Domain) (localId: int) : Guid =
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
    let newV3 (nsId: Guid) (name: byte[]) : Guid =
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
    let newV3S (nsId: Guid) (name: string) : Guid =
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
    let newV3Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version3.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 4.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 4122 UUID version 4.</returns>
    [<CompiledName("NewVersion4")>]
    let newV4 () : Guid = Generator.Version4.NewGuid()

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
    let newV5 (nsId: Guid) (name: byte[]) : Guid =
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
    let newV5S (nsId: Guid) (name: string) : Guid =
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
    let newV5Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version5.NewGuid(nsId, name, enc)

#if !UUIDREV_DISABLE
    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6.</returns>
    [<CompiledName("NewVersion6")>]
    let newV6 () : Guid = Generator.Version6.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6
    /// using a physical (IEEE 802 MAC) address node ID.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6
    /// using a physical (IEEE 802 MAC) address node ID.</returns>
    [<CompiledName("NewVersion6P")>]
    let newV6P () : Guid = Generator.Version6P.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 6
    /// using a non-volatile random node ID.</returns>
    [<CompiledName("NewVersion6R")>]
    let newV6R () : Guid = Generator.Version6R.NewGuid()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 9562 UUID version 6 using a volatile random node ID.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 9562 UUID version 6 using a volatile random node ID.</returns>
    [<CompiledName("NewVersion6RSequence")>]
    let newV6RSeq () : seq<Guid> =
        Generator.CreateVersion6R().AsSequence()

    /// <summary>
    /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 9562 UUID version 6 using a volatile random node ID without blocking.
    /// </summary>
    /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/> instances
    /// of RFC 9562 UUID version 6 using a volatile random node ID without blocking.</returns>
    [<CompiledName("NewVersion6RPooledSequence")>]
    let newV6RPoolSeq () : seq<Guid> =
        Generator.CreatePooled(Func<_>(Generator.CreateVersion6R)).AsSequence()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 7.
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 7.</returns>
    [<CompiledName("NewVersion7")>]
    let newV7 () : Guid = Generator.Version7.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// example implementation (RFC 9562 Appendix B.1).
    /// </summary>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// example implementation (RFC 9562 Appendix B.1).</returns>
    [<CompiledName("NewVersion8")>]
    let newV8 () : Guid = Generator.Version8.NewGuid()

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the specified hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="hashing">The hash algorithm.</param>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using <paramref name="hashing"/>.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="hashing"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
    [<CompiledName("NewVersion8N")>]
    let newV8N (hashing: HashAlgorithm)
               (nsId: Guid) (name: byte[]) : Guid =
        Generator.GetVersion8NOf(hashing).NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the specified hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="hashing">The hash algorithm.</param>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using <paramref name="hashing"/>.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="hashing"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
    [<CompiledName("NewVersion8NByString")>]
    let newV8NS (hashing: HashAlgorithm)
                (nsId: Guid) (name: string) : Guid =
        Generator.GetVersion8NOf(hashing).NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the specified hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="hashing">The hash algorithm.</param>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using <paramref name="hashing"/>.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="hashing"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
    [<CompiledName("NewVersion8NByEncoding")>]
    let newV8NEnc (hashing: HashAlgorithm)
                  (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.GetVersion8NOf(hashing).NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha256")>]
    let newV8NSha256 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha256ByString")>]
    let newV8NSha256S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha256ByEncoding")>]
    let newV8NSha256Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha256.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha384")>]
    let newV8NSha384 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha384.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha384ByString")>]
    let newV8NSha384S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha384.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha384ByEncoding")>]
    let newV8NSha384Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha384.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha512")>]
    let newV8NSha512 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha512.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha512ByString")>]
    let newV8NSha512S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha512.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    [<CompiledName("NewVersion8NSha512ByEncoding")>]
    let newV8NSha512Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha512.NewGuid(nsId, name, enc)

#if NET8_0_OR_GREATER
    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D256")>]
    let newV8NSha3D256 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha3D256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D256ByString")>]
    let newV8NSha3D256S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha3D256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D256ByEncoding")>]
    let newV8NSha3D256Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha3D256.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-384 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D384")>]
    let newV8NSha3D384 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha3D384.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-384 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D384ByString")>]
    let newV8NSha3D384S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha3D384.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-384 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-384 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-384 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D384ByEncoding")>]
    let newV8NSha3D384Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha3D384.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-512 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D512")>]
    let newV8NSha3D512 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NSha3D512.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-512 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D512ByString")>]
    let newV8NSha3D512S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NSha3D512.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHA3-512 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHA3-512 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHA3-512 hash algorithm.</exception>
    [<CompiledName("NewVersion8NSha3D512ByEncoding")>]
    let newV8NSha3D512Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NSha3D512.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE128 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE128 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE128 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake128")>]
    let newV8NShake128 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NShake128.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE128 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE128 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE128 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake128ByString")>]
    let newV8NShake128S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NShake128.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE128 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE128 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE128 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake128ByEncoding")>]
    let newV8NShake128Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NShake128.NewGuid(nsId, name, enc)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name byte array.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake256")>]
    let newV8NShake256 (nsId: Guid) (name: byte[]) : Guid =
        Generator.Version8NShake256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake256ByString")>]
    let newV8NShake256S (nsId: Guid) (name: string) : Guid =
        Generator.Version8NShake256.NewGuid(nsId, name)

    /// <summary>
    /// Generates a new <see cref="T:System.Guid"/> instance
    /// of RFC 9562 UUID version 8 using the SHAKE256 hash algorithm
    /// based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace <see cref="T:System.Guid"/>.</param>
    /// <param name="enc">The <see cref="T:System.Text.Encoding"/> of the name string.</param>
    /// <param name="name">The name string.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance of RFC 9562 UUID version 8
    /// using the SHAKE256 hash algorithm.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">
    /// This platform does not support the SHAKE256 hash algorithm.</exception>
    [<CompiledName("NewVersion8NShake256ByEncoding")>]
    let newV8NShake256Enc (nsId: Guid) (enc: Text.Encoding) (name: string) : Guid =
        Generator.Version8NShake256.NewGuid(nsId, name, enc)
#endif
#endif

    /// <summary>
    /// Loads the GUID generator state from the specified storage file
    /// and returns a value that indicates whether the loading operation is successful.
    /// </summary>
    /// <param name="fileName">The path of the state storage file,
    /// or <see langword="null"/> to disable the state storage.</param>
    /// <returns><see langword="true"/> if the state storage loading operation
    /// is successful; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("LoadGeneratorState")>]
    let loadState (fileName: string) : bool =
        Generator.SetStateStorageFile(fileName)

    /// <summary>
    /// Resets the GUID generator state that can be saving to the state storage file.
    /// </summary>
    [<CompiledName("ResetGeneratorState")>]
    let resetState () : unit = Generator.ResetState()

    /// <summary>
    /// Connects a listener function to the event that occurs
    /// when a GUID generator state storage I/O operation throws an exception.
    /// </summary>
    /// <param name="callback">The function to call when the event is triggered.</param>
    [<CompiledName("OnStateStorageException")>]
    let onStateExn (callback: StateStorageExceptionEventArgs -> unit) : unit =
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
    let ofFields (a: int) (b: int16) (c: int16) (d: byte, e: byte)
                 (f: byte, g: byte, h: byte, i: byte, j: byte, k: byte) : Guid =
        Guid(a, b, c, d, e, f, g, h, i, j, k)

#if NET7_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance
    /// by using the specified 128-bit unsigned integer.
    /// </summary>
    /// <param name="value">A 128-bit unsigned integer
    /// containing the value of the GUID.</param>
    /// <returns>A new <see cref="Guid"/> instance
    /// of the specified 128-bit unsigned integer.</returns>
    [<CLSCompliant(false)>]
    [<CompiledName("OfUInt128")>]
    let ofUInt128 (value: UInt128) : Guid = Uuid.FromUInt128(value)
#endif

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
    let ofBytes (bytes: byte[]) : Guid = Guid(bytes)

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
    let ofBytesUuid (bytes: byte[]) : Guid = Uuid.FromByteArray(bytes)

    /// <summary>
    /// Returns fields of integers and bytes of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>An 11-element value tuple
    /// that contains fields of the <see cref="T:System.Guid"/>.</returns>
    [<CompiledName("ToFields")>]
    let toFields (guid: Guid)
        : struct (int * int16 * int16 * struct (byte * byte) *
                  struct (byte * byte * byte * byte * byte * byte)) =
        let (a, b, c, d, e, f, g, h, i, j, k) = guid.Deconstruct()
        struct (a, b, c, struct (d, e), struct (f, g, h, i, j, k))

#if NET7_0_OR_GREATER
    /// <summary>
    /// Returns a 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <returns>A 128-bit unsigned integer
    /// that contains the value of the <see cref="Guid"/>.</returns>
    [<CLSCompliant(false)>]
    [<CompiledName("ToUInt128")>]
    let toUInt128 (guid: Guid) : UInt128 = guid.ToUInt128()
#endif

    /// <summary>
    /// Returns a 16-element byte array that contains fields
    /// in little-endian order of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields
    /// of the <see cref="T:System.Guid"/> in little-endian order.</returns>
    [<CompiledName("ToByteArray")>]
    let toBytes (guid: Guid) : byte[] = guid.ToByteArray()

    /// <summary>
    /// Returns a 16-element byte array that contains fields
    /// in big-endian order of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A 16-element byte array that contains fields
    /// of the <see cref="T:System.Guid"/> in big-endian order.</returns>
    [<CompiledName("ToUuidByteArray")>]
    let toBytesUuid (guid: Guid) : byte[] = guid.ToUuidByteArray()

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
    let parse (input: string) : Guid = Guid.Parse(input)

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
    let parseExact (format: string) (input: string) : Guid =
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
    let parseUrn (input: string) : Guid = Uuid.ParseUrn(input)

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance. Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParse")>]
    let tryParse (input: string) : Guid voption =
        Guid.TryParse(input) |> TryResult.toValueOption

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
    let tryParseExact (format: string) (input: string) : Guid voption =
        Guid.TryParseExact(input, format) |> TryResult.toValueOption

    /// <summary>
    /// Tries to convert the string representation of a GUID to the equivalent
    /// <see cref="T:System.Guid"/> instance, provided that the string is in the URN format.
    /// Returns <c>ValueNone</c> if the parse operation was failed.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>A <see cref="T:System.Guid"/> instance that contains the value that was parsed,
    /// or <c>ValueNone</c> if the parse operation was failed.</returns>
    [<CompiledName("TryParseUrn")>]
    let tryParseUrn (input: string) : Guid voption =
        Uuid.TryParseUrn(input) |> TryResult.toValueOption

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
    let format (format: string) (guid: Guid) : string =
        guid.ToString(format)

    /// <summary>
    /// Returns a URN string representation of the value of the <see cref="T:System.Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The value of the <see cref="T:System.Guid"/>, represented as a series
    /// of lowercase hexadecimal digits in the URN format.</returns>
    [<CompiledName("FormatUrn")>]
    let formatUrn (guid: Guid) : string = guid.ToUrnString()

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
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/> is of the RFC 4122 variant.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// is of the RFC 4122 variant; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsRfc4122Uuid")>]
    let internal isRfc4122 (guid: Guid) : bool =
        variant guid = Variant.Rfc4122

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/>
    /// is generated based on the current time.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// is generated based on the current time; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsTimeBased")>]
    let isTimeBased (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).IsTimeBased()

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/>
    /// is generated based on the input name.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// is generated based on the input name; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsNameBased")>]
    let isNameBased (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).IsNameBased()

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/> is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// is generated randomly; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsRandomized")>]
    let isRandomized (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).IsRandomized()

#if !UUIDREV_DISABLE
    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/>
    /// is generated based on custom data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// is generated based on custom data; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("IsCustomized")>]
    let isCustomized (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).IsCustomized()
#endif

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/> contains a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// contains a clock sequence; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsClockSequence")>]
    let hasClockSeq (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).ContainsClockSequence()

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/> contains local ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// contains local ID data; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsLocalId")>]
    let hasLocalId (guid: Guid) : bool =
        (isRfc4122 guid) && (version guid).ContainsLocalId()

    /// <summary>
    /// Gets a value that indicates whether the <see cref="T:System.Guid"/> contains node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="T:System.Guid"/>
    /// contains node ID data; otherwise, <see langword="false"/>.</returns>
    [<CompiledName("ContainsNodeId")>]
    let hasNodeId (guid: Guid) : bool =
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
        guid.TryGetTimestamp() |> TryResult.toValueOption

    /// <summary>
    /// Tries to get the clock sequence represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain a clock sequence.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The clock sequence represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain a clock sequence.</returns>
    [<CompiledName("TryGetClockSequence")>]
    let tryGetClockSeq (guid: Guid) : int16 voption =
        guid.TryGetClockSequence() |> TryResult.toValueOption

    /// <summary>
    /// Tries to get the DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE Security UUID.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The DCE Security domain and local ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not a DCE Security UUID.</returns>
    [<CompiledName("TryGetDomainAndLocalId")>]
    let tryGetLocalId (guid: Guid) : struct (Domain * int) voption =
        guid.TryGetDomainAndLocalId() |> TryResult.toValueOption2

    /// <summary>
    /// Tries to get the node ID represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain node ID data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The node ID represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain node ID data.</returns>
    [<CompiledName("TryGetNodeId")>]
    let tryGetNodeId (guid: Guid) : byte[] voption =
        guid.TryGetNodeId() |> TryResult.toValueOption

    /// <summary>
    /// Tries to get the hash data represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not name-based.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The hash data and its bitmask represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not name-based.</returns>
    [<CompiledName("TryGetHashData")>]
    let tryGetHashData (guid: Guid) : DataAndMask voption =
        guid.TryGetHashData() |> TryResult.toValueOption2

    /// <summary>
    /// Tries to get the random data represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The random data and its bitmask represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> is not generated randomly.</returns>
    [<CompiledName("TryGetRandomData")>]
    let tryGetRandomData (guid: Guid) : DataAndMask voption =
        guid.TryGetRandomData() |> TryResult.toValueOption2

#if !UUIDREV_DISABLE
    /// <summary>
    /// Tries to get the custom data represented by the <see cref="T:System.Guid"/>.
    /// Returns <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain custom data.
    /// </summary>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>The custom data and its bitmask represented by the <see cref="T:System.Guid"/>,
    /// or <c>ValueNone</c> if the <see cref="T:System.Guid"/> does not contain custom data.</returns>
    [<CompiledName("TryGetCustomData")>]
    let tryGetCustomData (guid: Guid) : DataAndMask voption =
        guid.TryGetCustomData() |> TryResult.toValueOption2
#endif

    /// <summary>
    /// Replaces the version of the current <see cref="T:System.Guid"/>
    /// with the specified <see cref="T:XNetEx.Guids.GuidVariant"/>.
    /// </summary>
    /// <param name="variant">The variant to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to this <see cref="T:System.Guid"/> except that
    /// the version replaced with <paramref name="variant"/>.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="variant"/>
    /// is not a valid <see cref="T:XNetEx.Guids.GuidVariant"/> value.</exception>
    [<CompiledName("ReplaceVariant")>]
    let replaceVariant (variant: Variant) (guid: Guid) : Guid =
        guid.ReplaceVariant(variant)

    /// <summary>
    /// Replaces the version of the current <see cref="T:System.Guid"/>
    /// with the specified <see cref="T:XNetEx.Guids.GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the version replaced with <paramref name="version"/>.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="version"/>
    /// is not a valid <see cref="T:XNetEx.Guids.GuidVersion"/> value.</exception>
    [<CompiledName("ReplaceVersion")>]
    let replaceVersion (version: Version) (guid: Guid) : Guid =
        guid.ReplaceVersion(version)

    /// <summary>
    /// Replaces the version of the current <see cref="T:System.Guid"/>
    /// with the specified GUID version number.
    /// </summary>
    /// <param name="version">The version to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the version replaced with <paramref name="version"/>.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="version"/> is not a valid GUID version number.</exception>
    [<CompiledName("ReplaceVersionNumber")>]
    let replaceVersionNum (version: byte) (guid: Guid) : Guid =
        guid.ReplaceVersion(version)

    /// <summary>
    /// Replaces the timestamp of the current <see cref="T:System.Guid"/> with the specified
    /// <see cref="T:System.DateTime"/> if the <see cref="T:System.Guid"/> is time-based.
    /// </summary>
    /// <param name="time">The timestamp to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the timestamp replaced with <paramref name="time"/>, or the original value
    /// if the <see cref="T:System.Guid"/> is not time-based.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="time"/>
    /// is out of the range that <paramref name="guid"/> can represent.</exception>
    [<CompiledName("ReplaceTimestamp")>]
    let replaceTime (time: DateTime) (guid: Guid) : Guid =
        guid.ReplaceTimestamp(time)

    /// <summary>
    /// Replaces the timestamp of the current <see cref="T:System.Guid"/> with the specified
    /// <see cref="T:System.DateTimeOffset"/> if the <see cref="T:System.Guid"/> is time-based.
    /// </summary>
    /// <param name="time">The timestamp to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the timestamp replaced with <paramref name="time"/>, or the original value
    /// if the <see cref="T:System.Guid"/> is not time-based.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="time"/>
    /// is out of the range that <paramref name="guid"/> can represent.</exception>
    [<CompiledName("ReplaceTimestampByOffset")>]
    let replaceTimeOffset (time: DateTimeOffset) (guid: Guid) : Guid =
        guid.ReplaceTimestamp(time)

    /// <summary>
    /// Replaces the clock sequence of the current <see cref="T:System.Guid"/> with the specified
    /// 16-bit signed integer if the <see cref="T:System.Guid"/> contains a clock sequence.
    /// </summary>
    /// <param name="clockSeq">The clock sequence to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the clock sequence replaced with <paramref name="clockSeq"/>, or the original value
    /// if the <see cref="T:System.Guid"/> does not contain a clock sequence.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="clockSeq"/>
    /// is out of the range that <paramref name="guid"/> can represent.</exception>
    [<CompiledName("ReplaceClockSequence")>]
    let replaceClockSeq (clockSeq: int16) (guid: Guid) : Guid =
        guid.ReplaceClockSequence(clockSeq)

    /// <summary>
    /// Replaces the DCE Security domain and local ID of the current <see cref="T:System.Guid"/>
    /// with the specified <see cref="T:XNetEx.Guids.DceSecurityDomain"/> and 32-bit signed integer
    /// if the <see cref="T:System.Guid"/> is a DCE Security UUID.
    /// </summary>
    /// <param name="domain">The DCE Security domain to use as replacement.</param>
    /// <param name="localId">The local ID to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the DCE Security domain replaced with <paramref name="domain"/>
    /// and the local ID replaced with and <paramref name="localId"/>, or the original value
    /// if the <see cref="T:System.Guid"/> is not a DCE Security UUID.</returns>
    [<CompiledName("ReplaceDomainAndLocalId")>]
    let replaceLocalId (domain: Domain) (localId: int) (guid: Guid) : Guid =
        guid.ReplaceDomainAndLocalId(domain, localId)

    /// <summary>
    /// Replaces the node ID of the current <see cref="T:System.Guid"/> with the specified
    /// byte array if the <see cref="T:System.Guid"/> contains node ID data.
    /// </summary>
    /// <param name="nodeId">The node ID to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the node ID replaced with <paramref name="nodeId"/>, or the original value
    /// if the <see cref="T:System.Guid"/> does not contain node ID data.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nodeId"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="nodeId"/> is not 6 bytes long.</exception>
    [<CompiledName("ReplaceNodeId")>]
    let replaceNodeId (nodeId: byte[]) (guid: Guid) : Guid =
        guid.ReplaceNodeId(nodeId)

    /// <summary>
    /// Replaces the hash data of the current <see cref="T:System.Guid"/> with the specified
    /// byte array if the <see cref="T:System.Guid"/> is name-based.
    /// </summary>
    /// <param name="hashData">The hash data to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the hash data replaced with <paramref name="hashData"/>, or the original value
    /// if the <see cref="T:System.Guid"/> is not name-based.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="hashData"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="hashData"/> is not 16 bytes long.</exception>
    [<CompiledName("ReplaceHashData")>]
    let replaceHashData (hashData: byte[]) (guid: Guid) : Guid =
        guid.ReplaceHashData(hashData)

    /// <summary>
    /// Replaces the random data of the current <see cref="T:System.Guid"/> with the specified
    /// byte array if the <see cref="T:System.Guid"/> is generated randomly.
    /// </summary>
    /// <param name="randomData">The random data to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the random data replaced with <paramref name="randomData"/>, or the original value
    /// if the <see cref="T:System.Guid"/> is not generated randomly.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="randomData"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="randomData"/> is not 16 bytes long.</exception>
    [<CompiledName("ReplaceRandomData")>]
    let replaceRandomData (randomData: byte[]) (guid: Guid) : Guid =
        guid.ReplaceRandomData(randomData)

#if !UUIDREV_DISABLE
    /// <summary>
    /// Replaces the custom data of the current <see cref="T:System.Guid"/> with the specified
    /// byte array if the <see cref="T:System.Guid"/> contains custom data.
    /// </summary>
    /// <param name="customData">The custom data to use as replacement.</param>
    /// <param name="guid">The <see cref="T:System.Guid"/>.</param>
    /// <returns>A new <see cref="T:System.Guid"/> instance that is
    /// equivalent to the <see cref="T:System.Guid"/> except that
    /// the custom data replaced with <paramref name="customData"/>, or the original value
    /// if the <see cref="T:System.Guid"/> does not contain custom data.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="customData"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="customData"/> is not 16 bytes long.</exception>
    [<CompiledName("ReplaceCustomData")>]
    let replaceCustomData (customData: byte[]) (guid: Guid) : Guid =
        guid.ReplaceCustomData(customData)
#endif

    /// <summary>
    /// Contains methods to build custom state <see cref="T:System.Guid"/> sequences
    /// using the F# computation expression syntax.
    /// </summary>
    [<Struct; NoEquality; NoComparison>]
    type CustomStateSeqBuilder

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:XNetEx.FSharp.Core.GuidModule.CustomStateSeqBuilder"/> structure
        /// of the specified <see cref="T:XNetEx.Guids.GuidVersion"/>.
        /// </summary>
        /// <param name="version">The version of the <see cref="T:System.Guid"/> sequence.</param>
        private (version: Version) =

        /// <summary>
        /// Creates a new <see cref="T:XNetEx.FSharp.Core.GuidModule.CustomStateSeqBuilder"/>
        /// instance of the specified <see cref="T:XNetEx.Guids.GuidVersion"/>.
        /// </summary>
        /// <param name="version">The version of the <see cref="T:System.Guid"/> sequence.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>
        /// instance of <paramref name="version"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="version"/> does not support using custom states.</exception>
        static member internal Create(version: Version) : CustomStateSeqBuilder =
            let validation = Generator.CustomStateBuilder.Create(version)
            CustomStateSeqBuilder(version)

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>
        /// instance of the current <see cref="T:XNetEx.Guids.GuidVersion"/>.
        /// </summary>
        /// <returns>A <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>
        /// instance of the current <see cref="T:XNetEx.Guids.GuidVersion"/>.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// This instance is not initialized correctly.</exception>
        member _.Zero() : Generator.CustomStateBuilder =
            Generator.CustomStateBuilder.Create(version)

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>
        /// instance of the current <see cref="T:XNetEx.Guids.GuidVersion"/>.
        /// </summary>
        /// <param name="unused">This parameter is not used.</param>
        /// <returns>A <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>
        /// instance of the current <see cref="T:XNetEx.Guids.GuidVersion"/>.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// This instance is not initialized correctly.</exception>
        member _.Yield(unused: unit) : Generator.CustomStateBuilder =
            Generator.CustomStateBuilder.Create(version)

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns the result <see cref="T:System.Guid"/> sequence of the specified
        /// <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <returns>The result <see cref="T:System.Guid"/> sequence of the specified
        /// <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="builder"/> is not initialized correctly.</exception>
        member _.Run(builder: Generator.CustomStateBuilder) : seq<Guid> =
            builder.ToGuidGenerator().AsSequence()

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified custom timestamp provider function.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="timeFunc">The custom timestamp provider function to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="timeFunc"/>.</returns>
        [<CustomOperation("timeFunc")>]
        member _.UseTimestampFunc(builder: Generator.CustomStateBuilder, timeFunc: unit -> DateTime)
            : Generator.CustomStateBuilder =
            builder.UseTimestampProvider(Func<_>(timeFunc))

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified custom timestamp provider function.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="timeFunc">The custom timestamp provider function to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="timeFunc"/>.</returns>
        [<CustomOperation("timeFunc")>]
        member _.UseTimestampFunc(builder: Generator.CustomStateBuilder, timeFunc: unit -> DateTimeOffset)
            : Generator.CustomStateBuilder =
            builder.UseTimestampProvider(Func<_>(timeFunc))

#if NET8_0_OR_GREATER
        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified custom <see cref="T:System.TimeProvider"/>.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="timeProvider">The custom <see cref="T:System.TimeProvider"/> to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="timeProvider"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="timeProvider"/>is <see langword="null"/>.</exception>
        [<CustomOperation("timeProvider")>]
        member _.UseTimeProvider(builder: Generator.CustomStateBuilder, timeProvider: TimeProvider)
            : Generator.CustomStateBuilder =
            builder.UseTimeProvider(timeProvider)
#endif

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified initial clock sequence.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="initClockSeq">The custom initial clock sequence to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="initClockSeq"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="initClockSeq"/> is negative or greater than 0x3FFF.</exception>
        [<CustomOperation("clockSeq")>]
        member _.UseClockSequence(builder: Generator.CustomStateBuilder, initClockSeq: int16)
            : Generator.CustomStateBuilder =
            builder.UseClockSequence(initClockSeq)

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified
        /// <see cref="T:XNetEx.Guids.Generators.NodeIdSource"/>.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="nodeIdSource">
        /// The <see cref="T:XNetEx.Guids.Generators.NodeIdSource"/> to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="nodeIdSource"/>.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="nodeIdSource"/>is not a valid enum value.</exception>
        [<CustomOperation("nodeIdSource")>]
        member _.UseNodeIdSource(builder: Generator.CustomStateBuilder, nodeIdSource: NodeIdSource)
            : Generator.CustomStateBuilder =
            builder.UseNodeIdSource(nodeIdSource)

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified custom node ID provider function.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="nodeIdFunc">The custom node ID provider function to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="nodeIdFunc"/>.</returns>
        [<CustomOperation("nodeIdFunc")>]
        member _.UseNodeIdFunc(builder: Generator.CustomStateBuilder, nodeIdFunc: unit -> byte[])
            : Generator.CustomStateBuilder =
            builder.UseNodeIdProvider(Func<_>(nodeIdFunc))

        /// <summary>
        /// A method used to support the F# computation expression syntax.
        /// Returns a new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using the specified custom node ID.
        /// </summary>
        /// <param name="builder">
        /// The current <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/>.</param>
        /// <param name="nodeId">The custom node ID to use.</param>
        /// <returns>A new <see cref="T:XNetEx.Guids.Generators.CustomStateGuidGeneratorBuilder"/> instance
        /// of the current state and using <paramref name="nodeId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="nodeId"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="nodeId"/> is not 6 bytes long.</exception>
        [<CustomOperation("nodeId")>]
        member _.UseNodeId(builder: Generator.CustomStateBuilder, nodeId: byte[])
            : Generator.CustomStateBuilder =
            builder.UseNodeId(nodeId)

    /// <summary>
    /// Builds an unlimited custom state <see cref="T:System.Guid"/> sequence of the specified
    /// <see cref="T:XNetEx.Guids.GuidVersion"/> using the F# computation expression syntax.
    /// </summary>
    /// <param name="version">The version of the <see cref="T:System.Guid"/> sequence.</param>
    /// <returns>A new <see cref="T:XNetEx.FSharp.Core.GuidModule.CustomStateSeqBuilder"/>
    /// instance of <paramref name="version"/>.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="version"/> does not support using custom states.</exception>
    [<CompiledName("CreateCustomStateSeqBuilder")>]
    let customStateSeq (version: Version) : CustomStateSeqBuilder =
        CustomStateSeqBuilder.Create(version)

    /// <summary>
    /// Builds an unlimited custom state <see cref="T:System.Guid"/> sequence of the specified
    /// GUID version number using the F# computation expression syntax.
    /// </summary>
    /// <param name="version">The version of the <see cref="T:System.Guid"/> sequence.</param>
    /// <returns>A new <see cref="T:XNetEx.FSharp.Core.GuidModule.CustomStateSeqBuilder"/>
    /// instance of <paramref name="version"/>.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="version"/> does not support using custom states.</exception>
    [<CompiledName("CreateCustomStateSeqBuilderOfVersion")>]
    let customStateSeqOfVersion (version: byte) : CustomStateSeqBuilder =
        CustomStateSeqBuilder.Create(enumof version)
