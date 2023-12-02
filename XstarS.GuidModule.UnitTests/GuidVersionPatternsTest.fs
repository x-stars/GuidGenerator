namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidVersionPatternsTest() =

    [<TestMethod>]
    member _.GuidEmptyPattern_GuidEmpty_GetMatchedUnit() =
        match Guid.empty with
        | GuidEmpty -> ignore "GuidEmpty pattern matched."
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion1Pattern_Version1Guid_GetExpectedComponents() =
        match Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8" with
        | GuidVersion1 (time, clkSeq, nodeId) ->
            Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc)) time
            Assert.equalTo 0x00b4s clkSeq
            Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |] nodeId
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion2Pattern_Version2Guid_GetExpectedComponents() =
        match Guid.parse "6ba7b810-9dad-21d1-b402-00c04fd430c8" with
        | GuidVersion2 (time, clkSeq, (domain, localId), nodeId) ->
            Assert.equalTo (DateTime(0x08BEFFD0E4344000L, DateTimeKind.Utc)) time
            Assert.equalTo 0x0034s clkSeq
            Assert.equalTo Guid.Domain.Org domain
            Assert.equalTo 0x6ba7b810 localId
            Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |] nodeId
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion3Pattern_Version3Guid_GetExpectedComponents() =
        match Guid.parse "a9ec4420-7252-3c11-ab70-512e10273537" with
        | GuidVersion3 (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0xa9; 0xec; 0x44; 0x20; 0x72; 0x52; 0x0c; 0x11
                   0x2b; 0x70; 0x51; 0x2e; 0x10; 0x27; 0x35; 0x37 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion4Pattern_Version4Guid_GetExpectedComponents() =
        match Guid.parse "2502f1d5-c2a9-47d3-b6d8-d7670094ace2" with
        | GuidVersion4 (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x25; 0x02; 0xf1; 0xd5; 0xc2; 0xa9; 0x07; 0xd3
                   0x36; 0xd8; 0xd7; 0x67; 0x00; 0x94; 0xac; 0xe2 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion5Pattern_Version5Guid_GetExpectedComponents() =
        match Guid.parse "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24" with
        | GuidVersion5 (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x76; 0x8a; 0x7b; 0x1b; 0xae; 0x51; 0x0c; 0x0a
                   0x3c; 0x9d; 0xa8; 0x5a; 0x34; 0x3f; 0x2c; 0x24 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.GuidVersion6Pattern_Version6Guid_GetExpectedComponents() =
        match Guid.parse "1d19dad6-ba7b-6810-80b4-00c04fd430c8" with
        | GuidVersion6 (time, clkSeq, nodeId) ->
            Assert.equalTo (DateTime(0x08BEFFD14FDBF810L, DateTimeKind.Utc)) time
            Assert.equalTo 0x00b4s clkSeq
            Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |] nodeId
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion7Pattern_Version7Guid_GetExpectedComponents() =
        match Guid.parse "017f22e2-79b0-7cc3-98c4-dc0c0c07398f" with
        | GuidVersion7 (time, (data, mask)) ->
            Assert.equalTo (DateTime(0x08D9F638A666EB00L, DateTimeKind.Utc)) time
            Assert.Seq.equalTo (Array.map byte
                [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0c; 0xc3
                   0x18; 0xc4; 0xdc; 0x0c; 0x0c; 0x07; 0x39; 0x8f |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidVersion8Pattern_Version8Guid_GetExpectedComponents() =
        match Guid.parse "05db6c94-bba6-8702-88aa-548f4d6cd700" with
        | GuidVersion8 (data, mask) ->
            Assert.Seq.equalTo (Array.map byte
                [| 0x05; 0xdb; 0x6c; 0x94; 0xbb; 0xa6; 0x07; 0x02
                   0x08; 0xaa; 0x54; 0x8f; 0x4d; 0x6c; 0xd7; 0x00 |]) data
            Assert.Seq.equalTo (Array.map byte
                [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                   0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]) mask
        | _ -> Assert.failWith "Unexpected pattern matched."

    [<TestMethod>]
    member _.GuidMaxValuePattern_GuidMaxValue_GetMatchedUnit() =
        match Guid.maxValue with
        | GuidMaxValue -> ignore "GuidMaxValue pattern matched."
        | _ -> Assert.failWith "Unexpected pattern matched."
#endif
