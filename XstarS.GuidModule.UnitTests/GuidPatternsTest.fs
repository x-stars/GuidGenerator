namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidPatternsTest() =

    [<TestMethod>]
    member _.GuidFieldsPattern_EmptyGuid_GetAllFieldsZero() =
        let (GuidFields (a, b, c, (d, e), (f, g, h, i, j, k))) = Guid.empty
        (a, b, c, (d, e), (f, g, h, i, j, k)) |> Assert.equalTo
            (0, 0s, 0s, (0uy, 0uy), (0uy, 0uy, 0uy, 0uy, 0uy, 0uy))

    [<TestMethod>]
    member _.GuidVariantPattern_Version1Guid_GetRfc4122Variant() =
        let (GuidVariant variant) = Guid.parse "6ba7b811-9dad-11d1-80b4-00c04fd430c8"
        Assert.equalTo Guid.Variant.Rfc4122 variant

    [<TestMethod>]
    member _.GuidVersionPattern_Version4Guid_GetGuidVersion4() =
        let (GuidVersion version) = Guid.parse "2502f1d5-c2a9-47d3-b6d8-d7670094ace2"
        Assert.equalTo Guid.Version.Version4 version

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.TimeBasedGuidPattern_Version6Guid_GetExpectedTimestamp() =
        match Guid.parse "1d19dad6-ba7b-6810-80b4-00c04fd430c8" with
        | TimeBasedGuid time ->
            Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc)) time
        | _ -> Assert.failWith "Unexpected pattern matched."
#else
    [<TestMethod>]
    member _.TimeBasedGuidPattern_Version1Guid_GetExpectedTimestamp() =
        match Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8" with
        | TimeBasedGuid time ->
            Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc)) time
        | _ -> Assert.failWith "Unexpected pattern matched."
#endif

    [<TestMethod>]
    member _.NameBasedGuidPattern_Version5Guid_GetExpectedHashDataAndMask() =
        match Guid.parse "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24" with
        | NameBasedGuid (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x76; 0x8a; 0x7b; 0x1b; 0xae; 0x51; 0x0c; 0x0a
                   0x3c; 0x9d; 0xa8; 0x5a; 0x34; 0x3f; 0x2c; 0x24 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.RandomizedGuidPattern_Version7Guid_GetExpectedRandomDataAndMask() =
        match Guid.parse "017f22e2-79b0-7cc3-98c4-dc0c0c07398f" with
        | RandomizedGuid (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0c; 0xc3
                   0x18; 0xc4; 0xdc; 0x0c; 0x0c; 0x07; 0x39; 0x8f |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."
#else
    [<TestMethod>]
    member _.RandomizedGuidPattern_Version4Guid_GetExpectedRandomDataAndMask() =
        match Guid.parse "2502f1d5-c2a9-47d3-b6d8-d7670094ace2" with
        | RandomizedGuid (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x25; 0x02; 0xf1; 0xd5; 0xc2; 0xa9; 0x07; 0xd3
                   0x36; 0xd8; 0xd7; 0x67; 0x00; 0x94; 0xac; 0xe2 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."
#endif

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.CustomizedGuidPattern_Version8Guid_GetExpectedCustomDataAndMask() =
        match Guid.parse "05db6c94-bba6-8702-88aa-548f4d6cd700" with
        | CustomizedGuid (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x05; 0xdb; 0x6c; 0x94; 0xbb; 0xa6; 0x07; 0x02
                   0x08; 0xaa; 0x54; 0x8f; 0x4d; 0x6c; 0xd7; 0x00 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."
#endif

    [<TestMethod>]
    member _.GuidClockSequencePattern_Version1Guid_GetExpectedClockSequence() =
        match Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8" with
        | GuidClockSeq clkSeq ->
            Assert.equalTo 0x00b4s clkSeq
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidDoaminAndLocalIdPattern_Version6Guid_GetExpectedDoaminAndLocalId() =
        match Guid.parse "6ba7b810-9dad-21d1-b402-00c04fd430c8" with
        | GuidLocalId (domain, localId) ->
            Assert.equalTo Guid.Domain.Org domain
            Assert.equalTo 0x6ba7b810 localId
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidNodeIdPattern_Version1Guid_GetExpectedNodeId() =
        match Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8" with
        | GuidNodeId nodeId ->
            Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |] nodeId
        | _ -> Assert.failWith "Unexpected pattern matched."
