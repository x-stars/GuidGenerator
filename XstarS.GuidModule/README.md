# F# GUID Module

Provides RFC 4122 (UUID) compliant GUID operations for F#.
The orders of input parameters are adjusted to match F# pipeline patterns.

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

## F# GUID Module Usage

Core module: `XNetEx.FSharp.Core.Guid`.

### RFC-compliant GUID Generation

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
// 6fca3dd2-d61d-58de-9363-1574b382ea68

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
