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

> * [RFC 4122 详细标准](http://www.webdav.org/specs/rfc4122.pdf)
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

### 直接调用静态生成方法生成 GUID

``` C#
using XstarS.GuidGenerators;

// time-based GUID generation.
var guidV1 = GuidGenerator.NewGuid(GuidVersion.Version1);

// name-based GUID generation.
var guidV5 = GuidGenerator.NewGuid(GuidVersion.Version5, GuidNamespaces.DNS, "github.com");
```

## GUID 生成命令行工具使用

GUID 生成命令行工具的工程位于 [XstarS.GuidGen.CLI](XstarS.GuidGen.CLI)。

> 以下将命令行工具重命名为 `GuidGen.exe` 后执行命令。

### 命令行帮助信息

``` CMD
> GuidGen -?
Usage:  GuidGen[.exe] [-V1|-V4] [-Cn]
        GuidGen[.exe] -V2 [-Cn] Domain [SiteID]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     generate time-based GUIDs.
    -V2     generate DCE security GUIDs.
    -V3     generate name-based GUID by MD5 hashing.
    -V4     generate pseudo-random GUIDs (default).
    -V5     generate name-based GUID by SHA1 hashing.
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
.NET SDK=6.0.203
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
```

|         Method | GuidCount |           Mean |        StdDev | Ratio | RatioSD |
|--------------- |----------:|---------------:|--------------:|------:|--------:|
|                |           |                |               |       |         |
|    GuidNewGuid |        10 |     471.158 ns |     5.0838 ns |  1.00 |    0.00 |
|  EmptyGenerate |        10 |      14.904 ns |     0.2724 ns |  0.03 |    0.00 |
| GuidV1Generate |        10 |     355.273 ns |     1.1192 ns |  0.75 |    0.01 |
| GuidV2Generate |        10 |     534.935 ns |    22.5042 ns |  1.14 |    0.05 |
| GuidV3Generate |        10 |   3,154.923 ns |    50.5871 ns |  6.70 |    0.12 |
| GuidV4Generate |        10 |     472.672 ns |     7.7831 ns |  1.00 |    0.02 |
| GuidV5Generate |        10 |   3,184.330 ns |    28.9721 ns |  6.74 |    0.08 |
|                |           |                |               |       |         |
|    GuidNewGuid |      1000 |  46,139.725 ns |   538.0497 ns |  1.00 |    0.00 |
|  EmptyGenerate |      1000 |   1,189.389 ns |    10.8059 ns |  0.03 |    0.00 |
| GuidV1Generate |      1000 |  35,907.772 ns |   516.6928 ns |  0.78 |    0.01 |
| GuidV2Generate |      1000 |  51,660.014 ns |   212.6783 ns |  1.12 |    0.01 |
| GuidV3Generate |      1000 | 310,405.899 ns | 1,696.2722 ns |  6.73 |    0.09 |
| GuidV4Generate |      1000 |  46,453.650 ns |   656.3521 ns |  1.01 |    0.02 |
| GuidV5Generate |      1000 | 313,504.720 ns | 2,278.5551 ns |  6.79 |    0.07 |
