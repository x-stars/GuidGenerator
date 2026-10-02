# Getting Started

## Requirements

The core library targets .NET Framework 4.6.1 and 4.7.2, .NET 6, 8, and 10, and .NET Standard 2.0 and 2.1. Choose a target supported by your application. The API reference on this site is generated from the Release `net10.0` build.

## Install the Package

Add the core package to a project:

```sh
dotnet add package XstarS.GuidGenerators
```

For F#-oriented APIs, install the companion `XstarS.GuidModule` package instead; it depends on the core library.

## Generate and Inspect GUIDs

This example creates a random GUID, a deterministic name-based GUID, and a time-sortable version 7 GUID. It then inspects the version and timestamp of the version 7 value:

```csharp
using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

Guid randomGuid = GuidGenerator.Version4.NewGuid();
Guid nameBasedGuid = GuidGenerator.Version5.NewGuid(
    GuidNamespaces.Dns,
    "example.com");
Guid unixTimeGuid = GuidGenerator.Version7.NewGuid();

Console.WriteLine(randomGuid);
Console.WriteLine(nameBasedGuid);
Console.WriteLine($"Version: {unixTimeGuid.GetVersion()}");

if (unixTimeGuid.TryGetTimestamp(out DateTime timestamp))
{
    Console.WriteLine($"Timestamp: {timestamp:O}");
}
```

The static properties such as `Version4` and `Version7` are convenient when the version is known at compile time. Use the factory when selecting a version dynamically:

```csharp
byte version = 7;
Guid guid = GuidGenerator.OfVersion(version).NewGuid();
```

Versions 3 and 5 require both a namespace and a name. Reusing the same namespace and name produces the same GUID. Predefined namespace identifiers include `Dns`, `Url`, `Oid`, and `X500`.

## Continue Exploring

- Read [Choosing a Version](introduction.md#choosing-a-version) for the differences between GUID layouts.
- Browse the [API reference](xref:XNetEx.Guids.Generators.GuidGenerator) for generator overloads, field operations, and XML documentation.
- See the repository [README](https://github.com/x-stars/GuidGenerator/blob/main/README.md) for additional C# and F# examples.
