# .NET GUID Generator

[English](README.md) | [简体中文](README.zh-CN.md)

Provides RFC 4122 (UUID) and RFC 9562 (UUIDREV) compliant GUID generators for .NET platform.

**Features:**

* Full support for generating GUID version 1 through 8.
* Support for getting and setting fields of GUID version 1 through 8.
* Fully compatible with Native AOT (including reflection-free mode).
* F# adapter module for GUID APIs in the framework and this project.

## RFC 4122 UUID Standard

RFC 4122 defines the following five versions of UUID:

* Version 1: The time-based version, contains a 60-bit timestamp and a 12-bit MAC address.
* Version 2: DCE Security version, contains a 28-bit timestamp, a 12-bit MAC address and a 32-bit local ID.
* Version 3: The name-based version, using MD5 hashing to compute the hash of the namespace and name.
* Version 4: The randomly or pseudo-randomly generated version, equivalent to `Guid.NewGuid()` in .NET.
* Version 5: The name-based version, using SHA-1 hashing to compute the hash of the namespace and name.

There is also a special Nil UUID whose bytes are all `0x00`s, which is equivalent to `Guid.Empty` in .NET.

> * [RFC 4122 UUID Standard](https://www.rfc-editor.org/rfc/rfc4122)
> * [DCE Security UUID Standard](https://pubs.opengroup.org/onlinepubs/9696989899/chap5.htm)

## RFC 9562 UUID Standard

RFC 9562 defines the following three versions of UUID:

* Version 6: The reordered time-based version, field-compatible with Version 1 except that the timestamp is reordered to big-endian order.
* Version 7: The Unix Epoch time-based version, contains a 48-bit timestamp and a 74-bit random number, field-compatible with ULID.
* Version 8: Reserved for custom UUID formats, fields except the variant and version are user-defined.

There is also a special Max UUID whose bytes are all `0xff`s, which is equivalent to `Guid.AllBitsSet` in .NET 9.0 or greater.

> * [RFC 9562 UUID Standard](https://www.rfc-editor.org/rfc/rfc9562)

## GUID Generator Library Usage

The GUID Generator Library project is located at [XstarS.GuidGenerators](XstarS.GuidGenerators).

### Get Generator Instance by Static Properties

``` csharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// Generate time-based GUID.
var guidV1 = GuidGenerator.Version1.NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// Generate name-based GUID.
var guidV5 = GuidGenerator.Version5.NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// Generate Unix time-based GUID.
var guidV7 = GuidGenerator.Version7.NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
```

### Get Generator Instance by the Factory Method

``` csharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// Generate time-based GUID.
var guidV1 = GuidGenerator.OfVersion(1).NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// Generate name-based GUID.
var guidV5 = GuidGenerator.OfVersion(5).NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// Generate Unix time-based GUID.
var guidV7 = GuidGenerator.OfVersion(7).NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
```

### Generate by Static Extension Methods (C# 14 or Later)

``` csharp
using System;
using XNetEx.Guids;

// Generate time-based GUID.
var guidV1 = Guid.NewVersion1();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// Generate name-based GUID.
var guidV5 = Guid.NewVersion5(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// Generate Unix time-based GUID.
var guidV7 = Guid.NewVersion7();
// 018640c6-0dc9-7189-a644-31acdba4cabc
```

### Build Custom State Generator Instance

``` csharp
using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// Build custom state time-based GUID generator.
var guidGenV1C =
    GuidGenerator.CreateCustomStateBuilder(GuidVersion.Version1)
    // Can also create by static properties:
    // CustomStateGuidGeneratorBuilder.Version1
        .UseTimestampProvider(() => DateTime.UtcNow + TimeSpan.FromHours(8))
        .UseClockSequence(0x0123)
        .UseNodeId(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 })
        .ToGuidGenerator();

// Generate custom state time-based GUID.
var guidV1C = guidGenV1C.NewGuid();
// 2a85a1d1-14c5-11f0-8123-010203040506
```

### GUID Generator State Storage

> [RFC 4122 Section 4.2.1](https://www.rfc-editor.org/rfc/rfc4122#section-4.2.1)

Optional support and requires configuration to enable:

``` csharp
using System;
using System.IO;
using XNetEx.Guids.Generators;

// Listen state storage exceptions.
GuidGenerator.StateStorageException += (sender, e) =>
{
    if ((e.OperationType != FileAccess.Read) ||
        (e.Exception is not FileNotFoundException))
    {
        Console.Error.WriteLine(e.Exception);
    }
};
// Set storage file path and load state.
var loadResult = GuidGenerator.SetStateStorageFile("state.bin");
```

### Component-based GUID Building

``` csharp
using System;
using XNetEx.Guids;

// Build time-based GUID.
var guidV6 = Uuid.EmptyOf(GuidVersion.Version6)
    .ReplaceTimestamp(new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc))
    .ReplaceClockSequence((short)0x00b4)
    .ReplaceNodeId(new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
    // 1d19dad6-ba7b-6810-80b4-00c04fd430c8

// Build Unix time-based GUID.
var guidV7 = Guid.NewGuid()
    .ReplaceVersion(GuidVersion.Version7)
    .ReplaceTimestamp(new DateTime(0x08D9F638A666EB00, DateTimeKind.Utc));
    // 017f22e2-79b0-774a-8e21-a60c1ca56e82
```

## F# GUID Module Usage

The F# GUID Module project is located at [XstarS.GuidModule](XstarS.GuidModule).

Core module: `XNetEx.FSharp.Core.Guid`.

Provides RFC 4122 (UUID) and RFC 9562 (UUIDREV) compliant GUID operations for F#.
The orders of input parameters are adjusted to match F# pipeline patterns.

### RFC-compliant GUID Generation

``` fsharp
open System
open XNetEx.FSharp.Core

// Load generator state from file.
let loadResult = Guid.loadState "state.bin"

// Generate time-based GUID.
let guidV1 = Guid.newV1 () // 3944a871-aa14-11ed-8791-a9a9a46de54f
// Generate randomized GUID.
let guidV4 = Guid.newV4 () // 0658f02d-45a4-4c25-b9d0-8ddbda3c3e08
// Generate Unix time-based GUID.
let guidV7 = Guid.newV7 () // 018640c6-0dc9-7189-a644-31acdba4cabc

// Generate name-based GUID.
let guidV3 = Guid.newV3S Guid.nsDns "github.com"
// 7f4771a0-1982-373d-928f-d31140a51652
let guidV5 = "github.com" |> Guid.newV5S Guid.nsDns
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// Build custom state time-based GUID sequence.
let guidV1CSeq =
    Guid.customStateSeq Guid.Version.Version1 {
        timeFunc (fun () -> DateTime.UtcNow + TimeSpan.FromHours(8))
        clockSeq 0x0123s
        nodeId (Array.init 6 (((+) 1) >> byte))
    }
let guidV1C = guidV1CSeq |> Seq.head
// 2a85a1d1-14c5-11f0-8123-010203040506

// Build time-based GUID.
let guid6 =
    Guid.emptyOf Guid.Version.Version6
    |> Guid.replaceTime DateTime.UtcNow
    |> Guid.replaceClockSeq 0x0123s
    |> Guid.replaceNodeId (Array.init 6 (((+) 1) >> byte))
    // 1edaa178-dec2-6054-8123-010203040506

// Build Unix time-based GUID.
let guid7 =
    Guid.newV4 ()
    |> Guid.replaceVersion Guid.Version.Version7
    |> Guid.replaceTime DateTime.UtcNow
    // 018640db-de47-7ab9-bf00-6119a1033265
```

### Common GUID Operations

``` fsharp
open XNetEx.FSharp.Core

// GUID parsing and formatting.
let guid1 = Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
let guid2 = "{6ba7b810-9dad-11d1-80b4-00c04fd430c8}" |> Guid.parseExact "B"
printfn "%s" (guid2 |> Guid.format "X")

// GUID construction and deconstruction.
let guid3 =
    Guid.ofFields
        0x00112233 0x4455s 0x6677s (0x88uy, 0x99uy)
        (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy)
let guid3Fields = guid3 |> Guid.toFields

let guid4 =
    Array.map byte
        [| 0x00; 0x11; 0x22; 0x33; 0x44; 0x55; 0x66; 0x77
           0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
    |> Guid.ofBytesUuid
let guid4Bytes = Guid.toBytes guid4
assert (guid3 = guid4)

let guid5 =
    Array.map byte
        [| 0x33; 0x22; 0x11; 0x00; 0x55; 0x44; 0x77; 0x66
           0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
    |> Guid.ofBytes
let guid5Bytes = Guid.toBytesUuid guid5
assert (guid3 = guid5)
```

## GUID Generator Command Line Tool Usage

The GUID Generator Command Line Tool project is located at [XstarS.GuidGen.CLI](XstarS.GuidGen.CLI).

> The following commands are executed after renaming the command line tool to `GuidGen.exe`.

### Command Line Tool Help Message

``` bat
> GuidGen -?
Generate RFC 4122/9562 compliant GUIDs.
Usage:  GuidGen[.exe] [-V1|-V4|-V1R] [-Cn]
        GuidGen[.exe] -V2 Domain [LocalID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -V6|-V7|-V8|-V6P|-V6R [-Cn]
        GuidGen[.exe] -V8N Hash :NS|GuidNS [Name]
        GuidGen[.exe] -RS|-Reset
        GuidGen[.exe] -V|-Version
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     Generate time-based GUID.
    -V2     Generate DCE Security GUID.
    -V3     Generate name-based GUID by MD5 hashing.
    -V4     Generate pseudo-random GUID (default).
    -V5     Generate name-based GUID by SHA1 hashing.
    -V6     Generate reordered time-based GUID.
    -V7     Generate Unix Epoch time-based GUID.
    -V8     Generate custom GUID (example impl).
    -V1R    Generate time-based GUID (random node ID).
    -V6P    Generate reordered time-based GUID
            (IEEE 802 MAC address node ID).
    -V6R    Generate reordered time-based GUID
            (non-volatile random node ID).
    -V8N    Generate custom GUID (name-based).
    -Cn     Generate n GUIDs of the current version.
    Domain  Specify a DCE Security domain,
            which can be Person, Group or Org.
    LocalID Specify a user-defined local ID
            for DCE Security domain Org (required).
    :NS     Specify a well-known GUID namespace,
            which can be :DNS, :URL, :OID or :X500.
    GuidNS  Specify a user-defined GUID namespace.
    Name    Specify the name to generate GUID,
            or empty to read from standard input.
    Hash    Specify a well-known hash algorithm,
            which can be SHA256, SHA384, SHA512,
                SHA3-256, SHA3-384, SHA3-512,
                SHAKE128 or SHAKE256.
    -RS|-Reset
            Reset the GUID generator state.
    -V|-Version
            Show the version information.
    -?|-H|-Help
            Show the current help message.
```

### Command Line Tool Usage Examples

``` bat
> GuidGen
7603eaf0-9aa8-47e6-a90b-009f9e7bbdf4
> GuidGen -V4 -C3
b3cbe197-3cca-4cd3-bcde-af605e6cac90
d0bb2cf9-ba9a-4d10-bc58-cfc7b9bd304a
3b1dc563-7c2e-4827-a1de-63f2eacd6512
> GuidGen -V5 :DNS github.com
6fca3dd2-d61d-58de-9363-1574b382ea68
> echo github.com | GuidGen -V5 :DNS
6fca3dd2-d61d-58de-9363-1574b382ea68
> GuidGen -V5 00000000-0000-0000-0000-000000000000 ""
e129f27c-5103-5c5c-844b-cdf0a15e160d
> GuidGen -V7
018640c6-0dc9-7189-a644-31acdba4cabc
```

## Performance Benchmark

``` plaintext
BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```

|             Method |  Mean            | StdDev        | Ratio |Allocated |
|------------------- |-----------------:|--------------:|------:|---------:|
|      `GuidNewGuid` |       39.4991 ns |     0.6056 ns |  1.00 |        - |
|    `EmptyGenerate` |        0.9068 ns |     0.0223 ns |  0.02 |        - |
|   `GuidV1Generate` |      105.6470 ns |     0.4313 ns |  2.68 |        - |
|   `GuidV2Generate` |      100.1834 ns |     0.0721 ns |  2.53 |        - |
|   `GuidV3Generate` |      185.6253 ns |     1.6967 ns |  4.70 |        - |
|   `GuidV4Generate` |       40.4183 ns |     0.4285 ns |  1.02 |        - |
|   `GuidV5Generate` |      175.3653 ns |     2.4101 ns |  4.44 |        - |
|   `GuidV6Generate` |      110.8020 ns |     0.0918 ns |  2.80 |        - |
|   `GuidV7Generate` |       87.1959 ns |     0.7277 ns |  2.20 |        - |
|   `GuidV8Generate` |       74.7173 ns |     0.5928 ns |  1.89 |        - |
| `MaxValueGenerate` |        0.8104 ns |     0.0097 ns |  0.02 |        - |
