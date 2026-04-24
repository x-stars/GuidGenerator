# Code Style Guide

This document describes the code style conventions used in the GuidGenerator project.

## Why Not EditorConfig?

EditorConfig is excellent for formatting rules (indentation, spacing, line endings),
but it cannot enforce naming conventions, `this` keyword usage, type qualification, and XML documentation structure.
These require code analysis tools or manual enforcement.

See [.editorconfig](.editorconfig) for formatting rules.

---

## C# Code Style

### Class Declaration Modifiers

- Use explicit access modifiers (`public`/`internal`)
- Prefer `sealed` for non-inheritable classes
- Use `partial` for classes split across multiple files

```csharp
// Public API classes
public abstract partial class GuidGenerator : IGuidGenerator, IGuidGeneratorInfo
public static partial class GuidExtensions

// Internal implementation classes
internal sealed class RandomGuidGenerator : GuidGenerator, IGuidGenerator
internal sealed class GuidGeneratorPool : IGuidGenerator, IDisposable
```

### Partial Class Usage

Split large classes by concern into multiple files:

``` plaintext
GuidGenerator.cs              - Main class definition
GuidGenerator.Versions.cs      - Version-specific logic
GuidGenerator.StateStorage.cs  - State management
GuidGenerator.VHashings.cs     - Hashing variants
```

### Private Fields

PascalCase (preferred) or `_camelCase` (legacy):

```csharp
// Preferred: PascalCase
private readonly DateTime EpochDateTime;
private readonly TimestampProvider TimestampProvider;

// Legacy: _camelCase (used in some helper classes)
private readonly string _name;
```

### this Parameter Usage

Always use `this.` prefix for instance members:

```csharp
public virtual bool RequiresInput => this.Version.IsNameBased();

public void Dispose()
{
    this.Dispose(disposing: true);
    GC.SuppressFinalize(this);
}
```

### Type Name Qualification

Use full namespace for type references:

```csharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

public abstract GuidVersion Version { get; }
public virtual GuidVariant Variant => GuidVariant.Rfc4122;
```

### Constant Naming

PascalCase for public constants:

```csharp
public static class HashAlgorithmNames
{
    public const string MD5 = "MD5";
    public const string SHA1 = "SHA1";
    public const string SHAKE128 = "SHAKE128";
}
```

### Generic Constraints

Use `where` clause for type constraints:

```csharp
public static T Identity<T>(this T value) where T : class? => value;
public static unsafe T Read<T>(ref byte source) where T : unmanaged => ...;
```

### Null Check Patterns

Use modern pattern matching (`is not null`):

```csharp
if (this.TrySetVariant(ref guid, variant) is not null)
{
    // Handle error
}

this.TimestampProvider = (timestampProvider is not null) ?
    timestampProvider : DefaultTimestampProvider;
```

### Exception Throwing

Use `nameof()` for parameter names, concise messages:

```csharp
throw new ArgumentOutOfRangeException(nameof(version));
throw new NotSupportedException("Message describing the error");
throw new ArgumentNullException(nameof(name));
```

### Property Patterns

Use expression-bodied properties for simple getters:

```csharp
public virtual GuidVariant Variant => GuidVariant.Rfc4122;
public virtual bool RequiresInput => this.Version.IsNameBased();
```

### Interface Implementation

Explicit `public` modifier:

```csharp
public abstract partial class GuidGenerator : IGuidGenerator, IGuidGeneratorInfo
{
    public abstract GuidVersion Version { get; }
    public abstract Guid NewGuid();
}
```

### XML Documentation

Required for all public APIs:

```csharp
/// <summary>
/// Represents the version of a <see cref="Guid"/>.
/// </summary>
public enum GuidVersion : byte
{
    /// <summary>
    /// Represents the version of the nil UUID (<see cref="Guid.Empty"/>).
    /// </summary>
    Empty = 0,
}

/// <summary>
/// Generates a new <see cref="Guid"/> instance.
/// </summary>
/// <returns>A new <see cref="Guid"/> instance.</returns>
/// <exception cref="ObjectDisposedException">
/// This instance has already been disposed.</exception>
public abstract Guid NewGuid();
```

Required elements: `<summary>`, `<param>`, `<returns>`, `<exception>`.

### Namespace Structure

Use layered namespaces:

```csharp
namespace XNetEx.Guids;            // Core types
namespace XNetEx.Guids.Generators;   // Generator implementations
namespace XNetEx.Guids.Components;   // Low-level field manipulation
namespace XNetEx.Guids.Helpers;      // Utility classes
```

### File Organization

Group related code by file naming patterns:

| Pattern       | Example                      | Purpose                  |
| ------------- | ---------------------------- | ------------------------ |
| Main class    | `GuidGenerator.cs`           | Primary class definition |
| Feature split | `GuidGenerator.Versions.cs`  | Version-specific code    |
| Concern split | `GuidExtensions.Replace.cs`  | Feature-based extension  |
| Components    | `TimeBasedGuidComponents.cs` | Field extraction logic   |
| Helpers       | `HashAlgorithmNames.cs`      | Utility constants        |
| Providers     | `TimestampProvider.cs`       | Dependency injection     |

### Unsafe Code

Minimize unsafe code, document when used:

```csharp
public static unsafe void Deconstruct(this Guid guid, ...)
{
    var pGuid = (byte*)&guid;
    a = *(int*)&pGuid[0];
    // Use only for performance-critical byte manipulation
}
```

### Conditional Compilation

Use meaningful symbols with clear scope:

```csharp
// RFC 9562 features (versions 6-8)
#if !UUIDREV_DISABLE
    // Version 6, 7, 8 code
#endif

// Framework-specific code
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    // Modern API code
#endif
```

---

## F# Code Style

### Active Patterns

Use for idiomatic pattern matching:

```fsharp
[<AutoOpen>]
module GuidPatterns =

    let (|GuidFields|) (guid: Guid) : struct (...) = ...
    let (|GuidVersion|) (guid: Guid) : Guid.Version = ...
    let (|TimeBasedGuid|_|) (guid: Guid) : DateTime voption = ...
```

### Module Attributes

```fsharp
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =
    // Module members
```

### Compiler Attributes

```fsharp
[<CompiledName("GuidFieldsPattern")>]
[<ValueAsStaticProperty>]
[<AutoOpen>]
```

---

## Analysis Tools

### Recommended Tools

1. **Microsoft.CodeAnalysis.NetAnalyzers** (included) - Built into the project
2. **StyleCopAnalyzers** (optional) - Additional documentation rules

### IDE Settings

- **Visual Studio**: Tools → Options → Text Editor → C# → Code Style
- **VS Code**: Install "C# Dev Kit" and configure "editor.codeActionsOnSave"

---

## Enforcement

### Build-Time Checks

- `<EnableNETAnalyzers>true</EnableNETAnalyzers>` in .csproj
- Treat warnings as errors for style issues (optional)

### Pre-commit Checklist

- [ ] Run analyzer: `dotnet build` (check warnings)
- [ ] Verify XML docs on public APIs
- [ ] Check private field naming
- [ ] Ensure `this.` prefix used consistently

---

## See Also

- [.editorconfig](.editorconfig) - Formatting rules
- [AGENTS.md](AGENTS.md) - AI agent guidance
- [README.md](README.md) - Project overview
