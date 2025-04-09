# .NET GUID Generator

Provides RFC 4122 (UUID) compliant GUID generators for .NET platform.

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

## GUID Generator Library Usage

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
```

### Build Custom State Generator Instance

``` csharp
using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// Build custom state time-based GUID generator.
var guidGenV1C =
    GuidGenerator.CreateCustomStateBuilder(GuidVersion.Version1)
    // Can also create by static property:
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
var guidV1 = Guid.Empty
    .ReplaceVariant(GuidVariant.Rfc4122)
    .ReplaceVersion(GuidVersion.Version1)
    .ReplaceTimestamp(new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc))
    .ReplaceClockSequence((short)0x00b4)
    .ReplaceNodeId(new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
    // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
```
