namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidComponentTest() =

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
    member _.TryGetTimestamp_Version1Guid_GetExpectedTimestamp() =
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
        Guid.parse "6ba7b810-9dad-21d1-b402-00c04fd430c8"
        |> Guid.tryGetLocalId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (Guid.Domain.Org, 0x6ba7b810)

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.TryGetNodeId_Version6Guid_GetExpectedNodeId() =
        Guid.parse "1d19dad6-ba7b-6810-80b4-00c04fd430c8"
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
#else
    [<TestMethod>]
    member _.TryGetNodeId_Version1Guid_GetExpectedNodeId() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
#endif

    [<TestMethod>]
    member _.ReplaceVariant_EmptyGuid_GetInputVariant() =
        Guid.empty
        |> Guid.replaceVariant Guid.Variant.Rfc4122
        |> Guid.variant
        |> Assert.equalTo Guid.Variant.Rfc4122

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.ReplaceVersion_GuidMaxValue_GetInputVersion() =
        Guid.maxValue
        |> Guid.replaceVersion Guid.Version.Version7
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version7
#else
    [<TestMethod>]
    member _.ReplaceVersion_EmptyGuid_GetInputVersion() =
        Guid.empty
        |> Guid.replaceVersion Guid.Version.Version4
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4
#endif

    [<TestMethod>]
    member _.ReplaceVersionNumber_Version4Guid_GetInputVersionNumber() =
        Guid.newV4 ()
        |> Guid.replaceVersionNum 8uy
        |> Guid.version
        |> Assert.equalTo (enumof 8uy)

    [<TestMethod>]
    member _.ReplaceTimestamp_Version1Guid_GetInputDateTime() =
        Guid.parse "00000000-0000-1000-80b4-00c04fd430c8"
        |> Guid.replaceTime (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc))
        |> Guid.tryGetTime
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc))

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.ReplaceTimestampOffset_Version6Guid_GetInputDateTimeUtc() =
        Guid.parse "00000000-0000-6000-80b4-00c04fd430c8"
        |> Guid.replaceTimeOffset (DateTimeOffset(0x08BF00145DFF3810L, TimeSpan.FromHours(8)))
        |> Guid.tryGetTime
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc))
#else
    [<TestMethod>]
    member _.ReplaceTimestampOffset_Version1Guid_GetInputDateTimeUtc() =
        Guid.parse "00000000-0000-1000-80b4-00c04fd430c8"
        |> Guid.replaceTimeOffset (DateTimeOffset(0x08BF00145DFF3810L, TimeSpan.FromHours(8)))
        |> Guid.tryGetTime
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc))
#endif

    [<TestMethod>]
    member _.ReplaceClockSequence_Version1Guid_GetInputClockSequence() =
        Guid.parse "6ba7b810-9dad-11d1-8000-00c04fd430c8"
        |> Guid.replaceClockSeq 0x00b4s
        |> Guid.tryGetClockSeq
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo 0x00b4s

    [<TestMethod>]
    member _.ReplaceDomainAndLocalId_Version2Guid_GetInputDomainAndLocalId() =
        Guid.parse "00000000-9dad-21d1-b400-00c04fd430c8"
        |> Guid.replaceLocalId Guid.Domain.Org 0x6ba7b810
        |> Guid.tryGetLocalId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo (Guid.Domain.Org, 0x6ba7b810)

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.ReplaceNodeId_Version6Guid_GetInputNodeId() =
        Guid.parse "1d19dad6-ba7b-6810-80b4-000000000000"
        |> Guid.replaceNodeId [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
#else
    [<TestMethod>]
    member _.ReplaceNodeId_Version1Guid_GetInputNodeId() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-000000000000"
        |> Guid.replaceNodeId [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
#endif
