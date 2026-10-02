# Introduction

GUIDs (also known as UUIDs) are 128-bit identifiers with several standardized layouts. GuidGenerator implements versions defined by [RFC 4122](https://www.rfc-editor.org/rfc/rfc4122) and [RFC 9562](https://www.rfc-editor.org/rfc/rfc9562), and exposes APIs for generation as well as field-level operations.

## Choosing a Version

| Version | Properties | Typical use |
| --- | --- | --- |
| 1 | Timestamp-based with a node identifier | Compatibility with systems using the original time-based layout |
| 2 | DCE Security layout with a local identifier | DCE Security interoperability |
| 3 | Deterministic namespace-and-name value using MD5 | Reproducing an identifier from the same namespace and name |
| 4 | Random data | General-purpose identifiers when ordering is unnecessary |
| 5 | Deterministic namespace-and-name value using SHA-1 | Reproducing an identifier from the same namespace and name |
| 6 | Timestamp-based with reordered time fields | Time-oriented ordering while retaining a version 1-style layout |
| 7 | Unix timestamp and random data | Time-sortable identifiers for contemporary applications |
| 8 | Application-defined fields | Interoperable custom formats |

Versions 3 and 5 are useful when the same logical input must map to the same identifier. Versions 4 and 7 use random data; version 7 additionally carries a Unix timestamp. Select a version based on the format and ordering properties your application needs, and consider the information encoded by time-based formats when deciding where to expose generated values.

## Library Components

The core `XstarS.GuidGenerators` package provides:

- `GuidGenerator` implementations for generating supported versions, including factory and version-specific access.
- `Guid` extensions and component APIs for inspecting or replacing fields and building values from components.
- Custom-state builders for configuring timestamp providers, clock sequences, and node IDs for time-based generation.
- Native AOT support, including a reflection-free mode.

The companion `XstarS.GuidModule` package exposes F#-oriented functions and active patterns over the core functionality. It adjusts argument order for common F# pipeline usage.

For installation and a first program, see [Getting Started](getting-started.md). Public types and members are listed in the [API reference](xref:XNetEx.Guids.Generators.GuidGenerator).
