# AI Agent Guide for GuidGenerator

## Overview

**GuidGenerator** is a comprehensive .NET library implementing [RFC 4122 (UUID) and RFC 9562 (UUIDREV)](README.md) compliant GUID generators for versions 1–8.
The project spans C# (core library, CLI, service, native interop) and F# (functional adapters).

| **Component**                  | **Language** | **Role**                                         |
| ------------------------------ | ------------ | ------------------------------------------------ |
| `XstarS.GuidGenerators`        | C#           | Core library (NuGet: XstarS.GuidGenerators)      |
| `XstarS.GuidGen.CLI`           | C#           | Command-line tool with Native AOT support        |
| `XstarS.GuidGen.Service`       | C#           | ASP.NET Core REST API (net8.0+)                  |
| `XstarS.GuidGen.NativeLib`     | C#           | Native C library bindings (net8.0+)              |
| `XstarS.GuidModule`            | F#           | F#-idiomatic wrappers (NuGet: XstarS.GuidModule) |
| `XstarS.GuidGenLib.UnitTests`  | C#           | MSTest suite for core library                    |
| `XstarS.GuidModule.UnitTests`  | F#           | Tests for F# module                              |
| `XstarS.GuidGenLib.Benchmarks` | C#           | BenchmarkDotNet performance tests                |

## Build & Test Commands

```bash
# Build (Debug is default)
dotnet build

# Build Release
dotnet build -c Release

# Run all tests (includes code coverage)
dotnet test --collect:"XPlat Code Coverage"

# Run tests only (skip code coverage)
dotnet test -c Release

# Run specific test project
dotnet test XstarS.GuidGenLib.UnitTests

# Run benchmarks
dotnet run -p XstarS.GuidGenLib.Benchmarks -c Release

# Publish CLI (Windows x64)
cd XstarS.GuidGen.CLI
dotnet publish -c Release -f net10.0 -r win-x64 --sc

# Publish CLI (Linux x64)
dotnet publish -c Release -f net10.0 -r linux-x64 --sc
```

## Architecture & Key Patterns

### Generator Pattern

- **Base class**: `GuidGenerator` (abstract)
- **Factory access**: `GuidGenerator.OfVersion(int)` or `GuidGenerator.Version1` (static properties)
- **Implementations**: `TimeBasedGuidGenerator`, `NameBasedGuidGenerator`, `RandomGuidGenerator`, `NameBasedGuidGenerator`, `UnixTimeBasedGuidGenerator`, `DceSecurityGuidGenerator`, `CustomGuidGenerator`
- **Extension methods**: Core API exposed via `Guid.*` extension methods (e.g., `Guid.NewVersion1()`, `Guid.GetVersion()`)

### Components Pattern

- Low-level field extraction: `IGuidComponent`/`IGuidComponents` interfaces
- Classes: `TimeBasedGuidComponents`, `NameBasedGuidComponents`, `RandomizedGuidComponents`, etc.
- Located in: `XstarS.GuidGenerators/Components/`

### Conditional Features

- RFC 9562 (versions 6–8) controlled by `UUIDREV_DISABLE` symbol (default: enabled)
- Framework-specific code: `#if` guards for NET461, NETSTANDARD2_0, NETCOREAPP2_1_OR_GREATER, etc.

## Naming Conventions

### Files & Classes

- Generator classes: `*GuidGenerator.cs` (e.g., `TimeBasedGuidGenerator.cs`)
- Component classes: `*GuidComponents.cs` (e.g., `RandomizedGuidComponents.cs`)
- Feature extensions split by concern: `GuidExtensions.cs`, `GuidExtensions.Replace.cs`, `GuidExtensions.OtherData.cs`
- Test files use partial classes: `GuidComponentTest.cs`, `GuidComponentTest.Timestamp.cs`, `GuidComponentTest.NodeId.cs`

### Namespaces

- Core: `XNetEx.Guids`, `XNetEx.Guids.Generators`
- F# module: `XNetEx.FSharp.Core`
- CLI: `XstarS.GuidGen`, `XstarS.GuidGen.Commands`

### Code Style

- C#: Latest LangVersion, nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Unsafe blocks allowed for performance-critical byte manipulation
- XML documentation: Use `<summary>`, `<param>`, `<returns>` (see `GuidVersion.cs` for examples)
- F#: Functional style with active patterns (see `GuidPatterns.fs`)

## Testing Strategy

### MSTest Framework

- Test classes: `[TestClass]` attribute
- Test methods: `[TestMethod]` attribute
- Assertions: `Assert.AreEqual()`, `Assert.IsTrue()`, `Assert.IsNull()`, etc.
- Partial classes group related tests by concern (e.g., `.Timestamp`, `.NodeId`, `.RandomData`)

### Coverage

- Multi-framework testing: net461, net472, net6.0, net8.0, net10.0
- Platform-specific tests: Conditional compilation for Windows/Unix differences
- Edge cases: Nil, Max, variant, version detection, field extraction

### Adding Tests

1. Add test method to existing `*Test.cs` file or create new one
2. Use `[TestMethod]` attribute; name pattern: `Test<Feature><Scenario>`
3. If test set grows, create partial class (e.g., `GuidComponentTest.NewFeature.cs`)
4. Run: `dotnet test XstarS.GuidGenLib.UnitTests`

## Multi-Targeting & Build Profiles

### Framework Support

- **XstarS.GuidGenerators**: net461, net472, net6.0, net8.0, net10.0, netstandard2.0, netstandard2.1 (maximum compatibility)
- **CLI/Service/NativeLib**: net8.0, net10.0 (modern only)
- **XstarS.GuidModule**: net8.0, netstandard2.0, netstandard2.1 (F#)

### Build Configuration Files (`BuildItems/`)

- `AssemblyInfo.Build.props`: Version (currently 2.13.0), signing, metadata
- `NativeAOT.Build.props`: Native AOT compatibility flags
- `NoReflection.Build.props`: Reflection-free mode for trimming
- `Trimming.Build.props`: Trimming-safe code
- `NuGetPack.Build.props`: NuGet package metadata
- `RefUnsafe.Build.props`: Ref/unsafe handling

### Code Sharing

- `SharedItems/Common/`: Platform-agnostic code included in all projects
- `SharedItems/FSharp/`: F#-specific utilities
- `SharedItems/SkipLocalsInit/`: Performance optimization markers
- `SharedItems/NoRuntimeMarshal/`: Platform marshaling optimization

**Include in .csproj:**

```xml
<ItemGroup>
  <Compile Include="..\SharedItems\Common\**\*.cs" />
  <Compile Include="..\SharedItems\SkipLocalsInit\**\*.cs" />
</ItemGroup>
```

## Common Development Tasks

### Adding a New GUID Version or Feature

1. **In `XstarS.GuidGenerators`:**
   - Add enum member to `GuidVersion.cs` if versioning changed
   - Create `*GuidGenerator.cs` implementing `GuidGenerator`
   - Create corresponding `*GuidComponents.cs` class if field extraction needed
   - Add extension methods to `GuidExtensions.cs` for public API
   - Add provider or helper if needed (in `Helpers/`, `Providers/`)

2. **In `XstarS.GuidModule`** (if F# API useful):
   - Add active pattern(s) to `GuidPatterns.fs`
   - Add wrapper functions in appropriate module file

3. **Testing:**
   - Create `*Test.cs` in `XstarS.GuidGenLib.UnitTests`
   - Add corresponding test in `XstarS.GuidModule.UnitTests`
   - Run: `dotnet test`

4. **Update RFC feature gates:**
   - Use `#if !UUIDREV_DISABLE` for RFC 9562-specific code (versions 6–8)

### Modifying Core GUID Manipulation

- Unsafe code allowed: Use for performance-critical byte operations
- Maintain multi-framework compatibility: Test changes with `dotnet test`
- Update XML documentation

### CLI/Service Updates

- CLI code embeds full GuidGenerators (compilation, not reference); changes to GuidGenerators rebuild CLI
- Service uses project reference; rebuild service after GuidGenerators changes
- Native AOT consideration: Test with `dotnet publish -r win-x64 --sc`

## Dependency Graph

``` tree
XstarS.GuidGenerators (core)
├── XstarS.GuidGen.CLI (embeds via compilation)
├── XstarS.GuidGen.Service (project reference)
├── XstarS.GuidGen.NativeLib (project reference)
├── XstarS.GuidModule (F# wrapper, project reference)
└── Tests (XstarS.GuidGenLib.UnitTests, XstarS.GuidModule.UnitTests)
```

## Current File Context

If working in `XstarS.GuidModule/` (F# code):

- `GuidPatterns.fs`: Active patterns for pattern matching on `Guid` values
  - Patterns: `|GuidFields|`, `|GuidVariant|`, `|GuidVersion|`, `|TimeBasedGuid|_|`, `|NameBasedGuid|_|`, `|RandomizedGuid|_|`, `|CustomizedGuid|_|`
  - These wrap underlying C# extension methods for idiomatic F# experience

## Quick Reference: File Locations

| **Type**    | **Location**                                 | **Pattern**          |
| ----------- | -------------------------------------------- | -------------------- |
| Generators  | `XstarS.GuidGenerators/Generators/`          | `*GuidGenerator.cs`  |
| Components  | `XstarS.GuidGenerators/Components/`          | `*GuidComponents.cs` |
| Extensions  | `XstarS.GuidGenerators/`                     | `GuidExtensions*.cs` |
| Helpers     | `XstarS.GuidGenerators/Helpers/`             | `*.cs`               |
| Providers   | `XstarS.GuidGenerators/Providers/`           | `*Provider.cs`       |
| C# Tests    | `XstarS.GuidGenLib.UnitTests/`               | `*Test.cs`           |
| F# Tests    | `XstarS.GuidModule.UnitTests/`               | `*Test.fs`           |
| Benchmarks  | `XstarS.GuidGenLib.Benchmarks/`              | `*Benchmark.cs`      |
| Shared Code | `SharedItems/Common/`, `SharedItems/FSharp/` | `*.cs`, `*.fs`       |

## Pre-commit Checks

Before committing:

1. Run `dotnet build -c Release`
2. Run `dotnet test` (verify all pass)
3. Verify target frameworks still build: `dotnet build -f net461` (for library projects)
4. XML documentation complete on public APIs
5. No reflection in code destined for CLI/Service/NativeLib (use `// ILC:no-reflection` if unavoidable)

---

For detailed API usage, see [README.md](README.md) and individual project READMEs.
