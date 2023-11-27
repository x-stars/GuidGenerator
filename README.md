# .NET GUID Generator

[English](README.md) | [简体中文](README.zh-CN.md)

Provides RFC 4122 UUID compliant GUID generators for .NET platform.

## RFC 4122 UUID Standard

RFC 4122 defines the following five versions of UUID:

* Version 1: The time-based version, contains a 60-bit timestamp and a 12-bit MAC address
* Version 2: DCE Security version, contains a 28-bit timestamp, a 12-bit MAC address and a 32-bit local ID
* Version 3: The name-based version, using MD5 hashing to compute the hash of the namespace and name
* Version 4: The randomly or pseudo-randomly generated version, equivalent to `Guid.NewGuid()` in .NET
* Version 5: The name-based version, using SHA-1 hashing to compute the hash of the namespace and name

There is also a special Nil UUID whose bytes are all `0x00`s, which is equivalent to `Guid.Empty` in .NET.

> * [RFC 4122 UUID Standard](https://www.rfc-editor.org/rfc/rfc4122)
> * [DCE Security UUID Standard](https://pubs.opengroup.org/onlinepubs/9696989899/chap5.htm)

## GUID Generator Library Usage

The GUID Generator Library project is located at [XstarS.GuidGenerators](XstarS.GuidGenerators).

### Get Generator Instance by Static Properties

``` CSharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// time-based GUID generation.
var guidV1 = GuidGenerator.Version1.NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// name-based GUID generation.
var guidV5 = GuidGenerator.Version5.NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68s
```

### Get Generator Instance by the Factory Method

``` CSharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// generate time-based GUID generation.
var guidV1 = GuidGenerator.OfVersion(1).NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// generate name-based GUID.
var guidV5 = GuidGenerator.OfVersion(5).NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68s
```

### GUID Generator State Storage

> [RFC 4122 Section 4.2.1](https://www.rfc-editor.org/rfc/rfc4122#section-4.2.1)

Optional support and requires configuration to enable:

``` CSharp
using System;
using System.IO;
using XNetEx.Guids.Generators;

// listen state storage exceptions.
GuidGenerator.StateStorageException += (sender, e) =>
{
    if ((e.OperationType != FileAccess.Read) ||
        (e.Exception is not FileNotFoundException))
    {
        Console.Error.WriteLine(e.Exception);
    }
};
// set storage file path and load state.
var loadResult = GuidGenerator.SetStateStorageFile("state.bin");
```

### Component-Based GUID Building

``` CSharp
using System;
using XNetEx.Guids;

// build time-based GUID.
var guidV1 = Guid.Empty
    .ReplaceVariant(GuidVariant.Rfc4122)
    .ReplaceVersion(GuidVersion.Version1)
    .ReplaceTimestamp(new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc))
    .ReplaceClockSequence((short)0x00b4)
    .ReplaceNodeId(new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
    // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
```

## F# GUID Module Usage

The F# GUID Module project is located at [XstarS.GuidModule](XstarS.GuidModule).

Core module: `XNetEx.FSharp.Core.Guid`.

Provides RFC 4122 UUID compliant GUID operations for F#.
The orders of input parameters are adjusted to match F# pipeline patterns.

### RFC 4122 GUID Generation

``` FSharp
open System
open XNetEx.FSharp.Core

// load generator state from file.
let loadResult = Guid.loadState "state.bin"

// generate time-based GUID.
let guidV1 = Guid.newV1 () // 3944a871-aa14-11ed-8791-a9a9a46de54f
// generate randomized GUID.
let guidV4 = Guid.newV4 () // 0658f02d-45a4-4c25-b9d0-8ddbda3c3e08

// generate name-based GUID.
let guidV3 = Guid.newV3S Guid.nsDns "github.com"
// 7f4771a0-1982-373d-928f-d31140a51652
let guidV5 = "github.com" |> Guid.newV5S Guid.nsDns
// 6fca3dd2-d61d-58de-9363-1574b382ea68s

// build time-based GUID.
let guid6 = Guid.empty
            |> Guid.replaceVariant Guid.Variant.Rfc4122
            |> Guid.replaceVersion Guid.Version.Version1
            |> Guid.replaceTime DateTime.UtcNow
            |> Guid.replaceClockSeq 0x0123s
            |> Guid.replaceNodeId (Array.init 6 (((+) 1) >> byte))
            // 8dec2054-aa17-11ed-8123-010203040506
```

### Common GUID Operations

``` FSharp
open XNetEx.FSharp.Core

// GUID parsing and formatting.
let guid1 = Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
let guid2 = "{6ba7b810-9dad-11d1-80b4-00c04fd430c8}"
            |> Guid.parseExact "B"
printfn "%s" (guid2 |> Guid.format "X")

// GUID construction and deconstruction.
let guid3 = Guid.ofFields
                0x00112233 0x4455s 0x6677s (0x88uy, 0x99uy)
                (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy)
let guid3Fields = guid3 |> Guid.toFields

let guid4 = Array.map byte
                [| 0x00; 0x11; 0x22; 0x33; 0x44; 0x55; 0x66; 0x77
                   0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
            |> Guid.ofBytesUuid
let guid4Bytes = Guid.toBytes guid4
assert (guid3 = guid4)

let guid5 = Array.map byte
                [| 0x33; 0x22; 0x11; 0x00; 0x55; 0x44; 0x77; 0x66
                   0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
            |> Guid.ofBytes
let guid5Bytes = Guid.toBytesUuid guid5
assert (guid3 = guid5)
```

## GUID Generator Command Line Tool Usage

The GUID Generator Command Line Tool project is located at [XstarS.GuidGen.CLI](XstarS.GuidGen.CLI).

> The following commands are executed after renaming the command line tool to  `GuidGen.exe`.

### Command Line Tool Help Message

``` Batch
> GuidGen -?
Generate RFC 4122 compliant GUIDs.
Usage:  GuidGen[.exe] [-V1|-V4|-V1R] [-Cn]
        GuidGen[.exe] -V2 Domain [SiteID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     generate time-based GUID.
    -V2     generate DCE Security GUID.
    -V3     generate name-based GUID by MD5 hashing.
    -V4     generate pseudo-random GUID (default).
    -V5     generate name-based GUID by SHA1 hashing.
    -V1R    generate time-based GUID (random node ID).
    -Cn     generate n GUIDs of the current version.
    Domain  specify a DCE Security domain,
            which can be Person, Group or Org.
    SiteID  specify a user-defined local ID
            for DCE Security domain Org (required).
    :NS     specify a well-known GUID namespace,
            which can be :DNS, :URL, :OID or :X500.
    GuidNS  specify a user-defined GUID namespace.
    Name    specify the name to generate GUID,
            or empty to read from standard input.
    -?|-H|-Help
            show the current help message.
```

### Command Line Tool Usage Examples

``` Batch
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
```

## Performance Benchmark

``` PlainText
BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2787/23H2/2023Update/SunValley3)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```

|             Method | GuidCount | Mean            | StdDev        | Ratio | Allocated |
|------------------- |----------:|----------------:|--------------:|------:|----------:|
|      `GuidNewGuid` |         1 |      44.2642 ns |     0.9311 ns |  1.00 |         - |
|    `EmptyGenerate` |         1 |       1.0135 ns |     0.0577 ns |  0.02 |         - |
|   `GuidV1Generate` |         1 |     105.3974 ns |     0.9108 ns |  2.38 |         - |
|   `GuidV2Generate` |         1 |     100.1791 ns |     0.0363 ns |  2.26 |         - |
|   `GuidV3Generate` |         1 |     188.8818 ns |     2.8465 ns |  4.27 |         - |
|   `GuidV4Generate` |         1 |      44.2476 ns |     0.5396 ns |  1.00 |         - |
|   `GuidV5Generate` |         1 |     179.7746 ns |     2.6377 ns |  4.07 |         - |
|                    |           |                 |               |       |           |
|      `GuidNewGuid` |      1000 |  40,615.7146 ns |   873.9581 ns | 1.000 |         - |
|    `EmptyGenerate` |      1000 |     235.3933 ns |     3.1243 ns | 0.006 |         - |
|   `GuidV1Generate` |      1000 | 101,453.6940 ns | 1,077.6102 ns | 2.496 |       3 B |
|   `GuidV2Generate` |      1000 | 102,866.1385 ns | 1,349.0252 ns | 2.528 |       3 B |
|   `GuidV3Generate` |      1000 | 181,153.2007 ns | 4,158.4657 ns | 4.468 |         - |
|   `GuidV4Generate` |      1000 |  41,099.3998 ns |   942.3362 ns | 1.014 |         - |
|   `GuidV5Generate` |      1000 | 182,166.1255 ns | 2,368.8430 ns | 4.474 |         - |
