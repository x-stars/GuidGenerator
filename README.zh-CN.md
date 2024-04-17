# .NET GUID 生成器

[English](README.md) | [简体中文](README.zh-CN.md)

为 .NET 平台提供符合 RFC 4122 (UUID) 标准的 GUID 生成器。

## RFC 4122 UUID 标准

RFC 4122 定义了以下 5 种 UUID 版本：

* Version 1: 基于时间戳，包含 60 位时间戳和 12 位 MAC 地址。
* Version 2: DCE Security 用途，包含 28 位时间戳、12 位 MAC 地址以及 32 位本地 ID。
* Version 3: 基于命名空间和名称，由 MD5 散列算法计算命名空间和名称得到。
* Version 4: 基于随机数（伪随机或真随机），与 .NET 的 `Guid.NewGuid()` 等价。
* Version 5: 基于命名空间和名称，由 SHA1 散列算法计算命名空间和名称得到。

除此之外，还有一个特殊的 Nil UUID，其所有字节均为 `0x00`，与 .NET 的 `Guid.Empty` 等价。

> * [RFC 4122 UUID 标准](https://www.rfc-editor.org/rfc/rfc4122)
> * [DCE Security UUID 标准](https://pubs.opengroup.org/onlinepubs/9696989899/chap5.htm)

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
// 6fca3dd2-d61d-58de-9363-1574b382ea68
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
// 6fca3dd2-d61d-58de-9363-1574b382ea68
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
var guidV1 = Guid.Empty
    .ReplaceVariant(GuidVariant.Rfc4122)
    .ReplaceVersion(GuidVersion.Version1)
    .ReplaceTimestamp(new DateTime(0x08BEFFD14FDBF810, DateTimeKind.Utc))
    .ReplaceClockSequence((short)0x00b4)
    .ReplaceNodeId(new byte[] { 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 });
    // 6ba7b810-9dad-11d1-80b4-00c04fd430c8
```

## F# GUID 模块使用

F# GUID 模块工程位于 [XstarS.GuidModule](XstarS.GuidModule)。

核心模块：`XNetEx.FSharp.Core.Guid`。

提供一套符合 RFC 4122 (UUID) 标准的 GUID 相关操作，并根据 F# 管道模式对输入参数顺序进行了适当调整。

### RFC 标准 GUID 生成

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
Generate RFC 4122 compliant GUIDs.
Usage:  GuidGen[.exe] [-V1|-V4|-V1R] [-Cn]
        GuidGen[.exe] -V2 Domain [SiteID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     Generate time-based GUID.
    -V2     Generate DCE Security GUID.
    -V3     Generate name-based GUID by MD5 hashing.
    -V4     Generate pseudo-random GUID (default).
    -V5     Generate name-based GUID by SHA1 hashing.
    -V1R    Generate time-based GUID (random node ID).
    -Cn     Generate n GUIDs of the current version.
    Domain  Specify a DCE Security domain,
            which can be Person, Group or Org.
    SiteID  Specify a user-defined local ID
            for DCE Security domain Org (required).
    :NS     Specify a well-known GUID namespace,
            which can be :DNS, :URL, :OID or :X500.
    GuidNS  Specify a user-defined GUID namespace.
    Name    Specify the name to generate GUID,
            or empty to read from standard input.
    -?|-H|-Help
            Show the current help message.
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
```

## 性能基准测试

``` PlainText
BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```

|             Method | GuidCount | Mean            | StdDev        | Ratio |Allocated |
|------------------- |----------:|----------------:|--------------:|------:|---------:|
|      `GuidNewGuid` |         1 |      39.4991 ns |     0.6056 ns |  1.00 |        - |
|    `EmptyGenerate` |         1 |       0.9068 ns |     0.0223 ns |  0.02 |        - |
|   `GuidV1Generate` |         1 |     105.6470 ns |     0.4313 ns |  2.68 |        - |
|   `GuidV2Generate` |         1 |     100.1834 ns |     0.0721 ns |  2.53 |        - |
|   `GuidV3Generate` |         1 |     185.6253 ns |     1.6967 ns |  4.70 |        - |
|   `GuidV4Generate` |         1 |      40.4183 ns |     0.4285 ns |  1.02 |        - |
|   `GuidV5Generate` |         1 |     175.3653 ns |     2.4101 ns |  4.44 |        - |
|                    |           |                 |               |       |          |
|      `GuidNewGuid` |      1000 |  37,335.7751 ns |   399.4522 ns | 1.000 |        - |
|    `EmptyGenerate` |      1000 |     232.4035 ns |     1.6808 ns | 0.006 |        - |
|   `GuidV1Generate` |      1000 | 105,798.8867 ns |   683.9275 ns | 2.834 |      2 B |
|   `GuidV2Generate` |      1000 | 106,502.6782 ns |   966.0136 ns | 2.853 |      3 B |
|   `GuidV3Generate` |      1000 | 179,090.1237 ns |   993.3070 ns | 4.797 |        - |
|   `GuidV4Generate` |      1000 |  37,113.4318 ns |   231.3545 ns | 0.994 |        - |
|   `GuidV5Generate` |      1000 | 174,083.2118 ns | 1,143.8121 ns | 4.665 |        - |
