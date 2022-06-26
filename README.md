# .NET GUID 生成器

为 .NET 平台提供符合 RFC 4122 UUID 标准的 GUID 生成器。

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
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// time-based GUID generation.
var guidGenV1 = GuidGenerator.Version1;
var guidV1 = guidGenV1.NewGuid();

// name-based GUID generation.
var guidGenV5 = GuidGenerator.Version5;
var guidV5 = guidGenV5.NewGuid(GuidNamespaces.Dns, "github.com");
```

### 工厂方法获取实例后调用

``` C#
using XNetEx.Guids;
using XNetEx.Guids.Generators;

// time-based GUID generation.
var guidGenV1 = GuidGenerator.OfVersion(GuidVersion.Version1);
var guidV1 = guidGenV1.NewGuid();

// name-based GUID generation.
var guidGenV5 = GuidGenerator.OfVersion(GuidVersion.Version5);
var guidV5 = guidGenV5.NewGuid(GuidNamespaces.Dns, "github.com");
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
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  DefaultJob : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
```

|           Method | GuidCount |           Mean |        StdDev | Ratio | RatioSD |
|----------------- |----------:|---------------:|--------------:|------:|--------:|
|    `GuidNewGuid` |         1 |      45.730 ns |     0.6073 ns |  1.00 |    0.00 |
|  `EmptyGenerate` |         1 |       3.303 ns |     0.0242 ns |  0.07 |    0.00 |
| `GuidV1Generate` |         1 |      52.571 ns |     0.6960 ns |  1.15 |    0.02 |
| `GuidV2Generate` |         1 |      70.437 ns |     2.0033 ns |  1.55 |    0.05 |
| `GuidV3Generate` |         1 |     292.193 ns |     1.8137 ns |  6.39 |    0.10 |
| `GuidV4Generate` |         1 |      17.686 ns |     0.0571 ns |  0.39 |    0.01 |
| `GuidV5Generate` |         1 |     298.274 ns |     5.0662 ns |  6.53 |    0.16 |
|                  |           |                |               |       |         |
|    `GuidNewGuid` |      1000 |  45,503.427 ns |    88.4931 ns |  1.00 |    0.00 |
|  `EmptyGenerate` |      1000 |   1,154.960 ns |     2.1231 ns |  0.03 |    0.00 |
| `GuidV1Generate` |      1000 |  50,888.715 ns |   735.9581 ns |  1.12 |    0.02 |
| `GuidV2Generate` |      1000 |  67,488.634 ns | 2,209.6733 ns |  1.49 |    0.06 |
| `GuidV3Generate` |      1000 | 290,308.876 ns | 2,142.5288 ns |  6.38 |    0.04 |
| `GuidV4Generate` |      1000 |  15,928.203 ns |    70.8751 ns |  0.35 |    0.00 |
| `GuidV5Generate` |      1000 | 288,628.431 ns | 1,988.7368 ns |  6.34 |    0.04 |
