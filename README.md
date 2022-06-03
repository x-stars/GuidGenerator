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

### 工厂方法创建后使用

``` C#
using XstarS.GuidGenerators;

// time-based GUID generation.
var guidGenV1 = GuidGenerator.OfVersion(GuidVersion.Version1);
var guidV1 = guidGenV1.NewGuid();

// name-based GUID generation.
var guidGenV5 = GuidGenerator.OfVersion(GuidVersion.Version5);
var guidV5 = guidGenV5.NewGuid(GuidNamespaces.DNS, "github.com");
```

### 直接调用静态生成方法

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
> GuidGen.exe -?
Usage:  GuidGen[.exe] [-V1|-V2|-V4] [-Cn]
        GuidGen[.exe] -V3|-V5 :NS|GuidNS [Name]
        GuidGen[.exe] -?|-H|-Help
Parameters:
    -V1     generate time-based GUIDs.
    -V2     generate DCE security GUIDs.
    -V3     generate name-based GUID by MD5 hashing.
    -V4     generate pesudo-random GUIDs (default).
    -V5     generate name-based GUID by SHA1 hashing.
    -Cn     generate n GUIDs of the current version.
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
> GuidGen.exe
7603eaf0-9aa8-47e6-a90b-009f9e7bbdf4
> GuidGen.exe -V4 -C3
b3cbe197-3cca-4cd3-bcde-af605e6cac90
d0bb2cf9-ba9a-4d10-bc58-cfc7b9bd304a
3b1dc563-7c2e-4827-a1de-63f2eacd6512
> GuidGen.exe -V5 :DNS github.com
6fca3dd2-d61d-58de-9363-1574b382ea68
> echo github.com | GuidGen.exe -V5 :DNS
6fca3dd2-d61d-58de-9363-1574b382ea68
> GuidGen.exe -V5 00000000-0000-0000-0000-000000000000 ""
e129f27c-5103-5c5c-844b-cdf0a15e160d
```
