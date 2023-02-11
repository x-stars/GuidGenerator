namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidComponentsTest() =

    [<TestMethod>]
    member _.GetVariant_Version1Guid_GetRfc4122Variant() =
        Guid.parse "6ba7b811-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.variant
        |> Assert.equalTo Guid.Variant.Rfc4122

    [<TestMethod>]
    member _.GetVersion_Version4Guid_GetGuidVersion4() =
        Guid.parse "2502f1d5-c2a9-47d3-b6d8-d7670094ace2"
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4

    [<TestMethod>]
    member _.TryGetTimestamp_Version1Guid_GetExpectedDateTime() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetTime
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc))

    [<TestMethod>]
    member _.TryGetClockSequence_Version1Guid_GetExpectedClockSequence() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetClockSeq
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo 0x00b4s

    [<TestMethod>]
    member _.TryGetDomainAndLocalId_Version2Guid_GetExpectedDomainAndLocalId() =
        Guid.newV2Org 0x00112233
        |> Guid.tryGetLocalId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (Guid.Domain.Org, 0x00112233)

    [<TestMethod>]
    member _.TryGetNodeId_Version1Guid_GetExpectedNodeId() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
