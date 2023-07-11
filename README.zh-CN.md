# .NET GUID 生成器

[English](README.md) | [简体中文](README.zh-CN.md)

为 .NET 平台提供符合 RFC 4122 UUID 标准以及 RFC4122bis UUIDREV 草稿的 GUID 生成器。

## RFC 4122 UUID 标准

RFC 4122 定义了以下 5 种 UUID 版本：

* Version 1: 基于时间戳，包含 60 位时间戳和 12 位 MAC 地址
* Version 2: DCE Security 用途，包含 28 位时间戳、12 位 MAC 地址以及 32 位本地 ID
* Version 3: 基于命名空间和名称，由 MD5 散列算法计算命名空间和名称得到
* Version 4: 基于随机数（伪随机或真随机），与 .NET 的 `Guid.NewGuid()` 等价
* Version 5: 基于命名空间和名称，由 SHA1 散列算法计算命名空间和名称得到

除此之外，还有一个特殊的 Nil UUID，其所有字节均为 `0x00`，与 .NET 的 `Guid.Empty` 等价。

> * [RFC 4122 UUID 标准](https://www.rfc-editor.org/rfc/rfc4122)
> * [DCE Security UUID 标准](https://pubs.opengroup.org/onlinepubs/9696989899/chap5.htm)

## RFC4122bis UUIDREV 草稿

RFC4122bis UUIDREV 定义了以下 3 种 UUID 版本：

* Version 6: 基于时间戳，与 Version 1 基本等价，但将时间戳调整为大端序
* Version 7: 基于 Unix 时间戳，包含 48 位时间戳和 74 位随机数，与 ULID 基本等价
* Version 8: 预留给自定义 UUID 格式，除变体和版本外的字段均由用户定义

除此之外，还有一个特殊的 Max UUID，其所有字节均为 `0xff`，在 .NET 中暂无等价实现（本项目提供）。

> * [RFC4122bis UUIDREV 草稿](https://datatracker.ietf.org/doc/html/draft-ietf-uuidrev-rfc4122bis)

## GUID 生成库使用

GUID 生成库的工程位于 [XstarS.GuidGenerators](XstarS.GuidGenerators)。

### 静态属性获取实例后调用

``` CSharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// time-based GUID generation.
var guidV1 = GuidGenerator.Version1.NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// name-based GUID generation.
var guidV5 = GuidGenerator.Version5.NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68s

// Unix time-based GUID generation.
var guidV7 = GuidGenerator.Version7.NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
```

### 工厂方法获取实例后调用

``` CSharp
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// generate time-based GUID generation.
var guidV1 = GuidGenerator.OfVersion(1).NewGuid();
// 3944a871-aa14-11ed-8791-a9a9a46de54f

// generate name-based GUID.
var guidV5 = GuidGenerator.OfVersion(5).NewGuid(GuidNamespaces.Dns, "github.com");
// 6fca3dd2-d61d-58de-9363-1574b382ea68s

// generate Unix time-based GUID.
var guidV7 = GuidGenerator.OfVersion(7).NewGuid();
// 018640c6-0dc9-7189-a644-31acdba4cabc
```

### GUID 生成器状态存储

> [RFC 4122 Section 4.2.1](https://www.rfc-editor.org/rfc/rfc4122#section-4.2.1)

可选支持，需要配置方可启用：

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

### 基于组件的 GUID 构建

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

## F# GUID 模块使用

F# GUID 模块工程位于 [XstarS.GuidModule](XstarS.GuidModule)。

核心模块：`XNetEx.FSharp.Core.Guid`。

提供一套符合 RFC 4122 标准以及 RFC4122bis UUIDREV 草稿的 GUID 相关操作，并根据 F# 管道模式对输入参数顺序进行了适当调整。

### RFC 4122 GUID 生成

``` FSharp
open System
open XNetEx.FSharp.Core

// load generator state from file.
let loadResult = Guid.loadState "state.bin"

// generate time-based GUID.
let guidV1 = Guid.newV1 () // 3944a871-aa14-11ed-8791-a9a9a46de54f
// generate randomized GUID.
let guidV4 = Guid.newV4 () // 0658f02d-45a4-4c25-b9d0-8ddbda3c3e08
// generate Unix time-based GUID.
let guidV7 = Guid.newV7 () // 018640c6-0dc9-7189-a644-31acdba4cabc

// generate name-based GUID.
let guidV3 = Guid.newV3S Guid.nsDns "github.com"
// 7f4771a0-1982-373d-928f-d31140a51652
let guidV5 = "github.com" |> Guid.newV5S Guid.nsDns
// 6fca3dd2-d61d-58de-9363-1574b382ea68s

// build time-based GUID.
let guid6 = Guid.empty
            |> Guid.replaceVariant Guid.Variant.Rfc4122
            |> Guid.replaceVersion Guid.Version.Version6
            |> Guid.replaceTime DateTime.UtcNow
            |> Guid.replaceClockSeq 0x0123s
            |> Guid.replaceNodeId (Array.init 6 (((+) 1) >> byte))
            // 1edaa178-dec2-6054-8123-010203040506

// build Unix time-based GUID.
let guid7 = Guid.newV4 ()
            |> Guid.replaceVersion Guid.Version.Version7
            |> Guid.replaceTime DateTime.UtcNow
            // 018640db-de47-7ab9-bf00-6119a1033265
```

### 常见 GUID 相关操作

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

## GUID 生成命令行工具使用

GUID 生成命令行工具的工程位于 [XstarS.GuidGen.CLI](XstarS.GuidGen.CLI)。

> 以下将命令行工具重命名为 `GuidGen.exe` 后执行命令。

### 命令行帮助信息

``` Batch
> GuidGen -?
Generate RFC 4122 revision compliant GUIDs.
Usage:  GuidGen[.exe] [-V1|-V4|-V1R] [-Cn]
        GuidGen[.exe] -V2 Domain [SiteID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -V6|-V7|-V8|-V6P|-V6R [-Cn]
        GuidGen[.exe] -V8N Hash :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     generate time-based GUID.
    -V2     generate DCE Security GUID.
    -V3     generate name-based GUID by MD5 hashing.
    -V4     generate pseudo-random GUID (default).
    -V5     generate name-based GUID by SHA1 hashing.
    -V6     generate reordered time-based GUID.
    -V7     generate Unix Epoch time-based GUID.
    -V8     generate custom GUID (example impl).
    -V1R    generate time-based GUID (random node ID).
    -V6P    generate reordered time-based GUID
            (IEEE 802 MAC address node ID).
    -V6R    generate reordered time-based GUID
            (non-volatile random node ID).
    -V8N    generate custom GUID (name-based).
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
    Hash    specify a well-known hash algorithm,
            which can be SHA256, SHA384 or SHA512.
    -?|-H|-Help
            show the current help message.
```

### 命令行工具使用例

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
> GuidGen -V7
018640c6-0dc9-7189-a644-31acdba4cabc
```

## 性能基准测试

``` PlainText
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT
  DefaultJob : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT
```

|             Method | GuidCount |           Mean |        StdDev | Ratio | Allocated |
|------------------- |----------:|---------------:|--------------:|------:|----------:|
|      `GuidNewGuid` |         1 |      50.930 ns |     0.8717 ns |  1.00 |         - |
|    `EmptyGenerate` |         1 |       3.637 ns |     0.2156 ns |  0.07 |         - |
|   `GuidV1Generate` |         1 |     103.763 ns |     1.4919 ns |  2.04 |         - |
|   `GuidV2Generate` |         1 |     103.515 ns |     1.5682 ns |  2.03 |         - |
|   `GuidV3Generate` |         1 |     200.474 ns |     4.8432 ns |  3.93 |         - |
|   `GuidV4Generate` |         1 |      52.946 ns |     1.5133 ns |  1.03 |         - |
|   `GuidV5Generate` |         1 |     197.465 ns |     6.1259 ns |  3.89 |         - |
|   `GuidV6Generate` |         1 |     118.069 ns |     1.2088 ns |  2.32 |         - |
|   `GuidV7Generate` |         1 |     101.517 ns |     1.6173 ns |  1.99 |         - |
|   `GuidV8Generate` |         1 |      88.968 ns |     2.0039 ns |  1.75 |         - |
| `MaxValueGenerate` |         1 |       3.827 ns |     0.1795 ns |  0.08 |         - |
|                    |           |                |               |       |           |
|      `GuidNewGuid` |      1000 |  49,159.827 ns |   344.4287 ns |  1.00 |         - |
|    `EmptyGenerate` |      1000 |   1,289.350 ns |    17.2862 ns |  0.03 |         - |
|   `GuidV1Generate` |      1000 | 101,202.434 ns |   542.1528 ns |  2.06 |       2 B |
|   `GuidV2Generate` |      1000 | 101,747.558 ns |   795.9027 ns |  2.07 |       5 B |
|   `GuidV3Generate` |      1000 | 193,502.769 ns | 3,048.0942 ns |  3.93 |         - |
|   `GuidV4Generate` |      1000 |  49,262.432 ns |   468.0596 ns |  1.00 |         - |
|   `GuidV5Generate` |      1000 | 189,286.804 ns | 2,916.9660 ns |  3.85 |         - |
|   `GuidV6Generate` |      1000 | 117,344.705 ns |   694.3620 ns |  2.39 |         - |
|   `GuidV7Generate` |      1000 |  97,508.760 ns | 1,155.9623 ns |  1.98 |         - |
|   `GuidV8Generate` |      1000 |  85,158.967 ns | 1,112.3969 ns |  1.73 |         - |
| `MaxValueGenerate` |      1000 |   1,323.650 ns |    50.9400 ns |  0.03 |         - |
