# .NET GUID Generator

Provides RFC 4122 UUID and RFC4122bis UUIDREV (draft) compliant GUID generators for .NET platform.

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

## RFC4122bis UUIDREV Draft

RFC4122bis UUIDREV defines the following three versions of UUID:

* Version 6: The reordered time-based version, field-compatible with Version 1 except that the timestamp is reordered to big-endian order
* Version 7: The Unix Epoch time-based version, contains a 48-bit timestamp and a 74-bit random number, field-compatible with ULID
* Version 8: Reserved for custom UUID formats, fields except the variant and version are user-defined

There is also a special Max UUID whose bytes are all `0xff`s, which has no equivalent implementation in .NET (provided in this project).

> * [RFC4122bis UUIDREV Draft](https://datatracker.ietf.org/doc/html/draft-ietf-uuidrev-rfc4122bis)

## GUID Generator Library Usage

### Get Generator Instance by Static Properties

``` CSharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// time-based GUID generation.
var guidV1 = GuidGenerator.Version1.NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// name-based GUID generation.
var guidV5 = GuidGenerator.Version5.NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// Unix time-based GUID generation.
var guidV7 = GuidGenerator.Version7.NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
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
// 6fca3dd2-d61d-58de-9363-1574b382ea68

// generate Unix time-based GUID.
var guidV7 = GuidGenerator.OfVersion(7).NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
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
var guidV6 = Guid.Empty
    .ReplaceVariant(GuidVariant.Rfc4122)
    .ReplaceVersion(GuidVersion.Version6)
    .ReplaceTimestamp(new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc))
    .ReplaceClockSequence((short)0x00b4)
    .ReplaceNodeId(new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
    // 1d19dad6-ba7b-6810-80b4-00c04fd430c8

// build Unix time-based GUID.
var guidV7 = Guid.NewGuid()
    .ReplaceVersion(GuidVersion.Version7)
    .ReplaceTimestamp(new DateTime(0x08D9F638A666EB00, DateTimeKind.Utc));
    // 017f22e2-79b0-774a-8e21-a60c1ca56e82
```
