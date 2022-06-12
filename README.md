# .NET GUID 生成器

提供 .NET 平台与 RFC 4122 UUID 标准兼容的 GUID 生成器。

## RFC 4122 UUID 标准

RFC 4122 定义了以下 5 种 UUID 版本：

* Version 1: 基于时间戳，包含 60 位时间戳和 12 位 MAC 地址
* Version 2: DCE Security 用途，包含 28 位时间戳、12 位 MAC 地址以及 32 位本地 ID
* Version 3: 基于命名空间和名称，由 MD5 散列算法计算命名空间和名称得到
* Version 4: 基于随机数（伪随机或真随机），与 .NET 的 `Guid.NewGuid()` 等价
* Version 5: 基于命名空间和名称，由 SHA1 散列算法计算命名空间和名称得到

除此之外，还有一个特殊的 Nil UUID，与 .NET 的 `Guid.Empty` 等价。

> * [RFC 4122 UUID 标准](https://www.rfc-editor.org/rfc/rfc4122)
> * [DCE Security UUID 标准](https://pubs.opengroup.org/onlinepubs/9696989899/chap5.htm)

## GUID 生成库使用

GUID 生成库的工程位于 [XstarS.GuidGenerators](XstarS.GuidGenerators)。

### 静态属性获取实例后调用

``` C#
using XstarS.GuidGenerators;

// time-based GUID generation.
var guidGenV1 = GuidGenerator.Version1;
var guidV1 = guidGenV1.NewGuid();

// name-based GUID generation.
var guidGenV5 = GuidGenerator.Version5;
var guidV5 = guidGenV5.NewGuid(GuidNamespaces.DNS, "github.com");
```

### 工厂方法获取实例后调用

``` C#
using XstarS.GuidGenerators;

// time-based GUID generation.
var guidGenV1 = GuidGenerator.OfVersion(GuidVersion.Version1);
var guidV1 = guidGenV1.NewGuid();

// name-based GUID generation.
var guidGenV5 = GuidGenerator.OfVersion(GuidVersion.Version5);
var guidV5 = guidGenV5.NewGuid(GuidNamespaces.DNS, "github.com");
```

## GUID 生成命令行工具使用

GUID 生成命令行工具的工程位于 [XstarS.GuidGen.CLI](XstarS.GuidGen.CLI)。

> 以下将命令行工具重命名为 `GuidGen.exe` 后执行命令。

### 命令行帮助信息

``` CMD
> GuidGen -?
Generate RFC 4122 compliant GUIDs.
Usage:  GuidGen[.exe] [-V1|-V4|-V1R] [-Cn]
        GuidGen[.exe] -V2 [-Cn] Domain [SiteID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     generate time-based GUIDs.
    -V2     generate DCE security GUIDs.
    -V3     generate name-based GUID by MD5 hashing.
    -V4     generate pseudo-random GUIDs (default).
    -V5     generate name-based GUID by SHA1 hashing.
    -V1R    generate time-based GUIDs (random node ID).
    -Cn     generate n GUIDs of the current version.
    Domain  specify a DCE security domain,
            which can be Person, Group or Org.
    SiteID  specify a user-defined local ID
            for DCE security domain Org (required).
    :NS     specify a well-known GUID namespace,
            which can be :DNS, :URL, :OID or :X500.
    GuidNS  specify a user-defined GUID namespace.
    Name    specify the name to generate GUID,
            or empty to read from standard input.
    -?|-H|-Help
            show the current help message.
```

### 命令行工具使用例

``` CMD
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
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
```

|           Method | GuidCount |           Mean |        StdDev | Ratio | RatioSD |
|----------------- |----------:|---------------:|--------------:|------:|--------:|
|    `GuidNewGuid` |         1 |      47.103 ns |     1.1493 ns |  1.00 |    0.00 |
|  `EmptyGenerate` |         1 |       3.732 ns |     0.0349 ns |  0.08 |    0.00 |
| `GuidV1Generate` |         1 |      45.935 ns |     0.0902 ns |  0.97 |    0.03 |
| `GuidV2Generate` |         1 |      61.437 ns |     0.1021 ns |  1.30 |    0.04 |
| `GuidV3Generate` |         1 |     290.403 ns |     1.1188 ns |  6.14 |    0.17 |
| `GuidV4Generate` |         1 |      45.968 ns |     0.1784 ns |  0.97 |    0.02 |
| `GuidV5Generate` |         1 |     292.349 ns |     1.7315 ns |  6.18 |    0.16 |
|                  |           |                |               |       |         |
|    `GuidNewGuid` |      1000 |  45,449.393 ns |   179.1553 ns |  1.00 |    0.00 |
|  `EmptyGenerate` |      1000 |   1,149.728 ns |     2.8254 ns |  0.03 |    0.00 |
| `GuidV1Generate` |      1000 |  43,300.315 ns |    84.2112 ns |  0.95 |    0.00 |
| `GuidV2Generate` |      1000 |  57,967.161 ns |    98.5313 ns |  1.28 |    0.01 |
| `GuidV3Generate` |      1000 | 282,600.088 ns | 1,675.9406 ns |  6.22 |    0.04 |
| `GuidV4Generate` |      1000 |  44,360.313 ns |   100.6797 ns |  0.98 |    0.00 |
| `GuidV5Generate` |      1000 | 287,265.934 ns | 1,995.7577 ns |  6.32 |    0.05 |
