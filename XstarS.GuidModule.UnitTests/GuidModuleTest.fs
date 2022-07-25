namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidModuleTest() =

    [<TestMethod>]
    member _.EmptyGuid_GetPropertyValue_GetAllFieldsZero() =
        Guid.empty
        |> Guid.toFields
        |> Assert.equalTo (0, 0s, 0s, (0uy, 0uy),
                           (0uy, 0uy, 0uy, 0uy, 0uy, 0uy))

    [<TestMethod>]
    member _.NewVersion1_WithoutInput_GetGuidOfVersion1() =
        Guid.newV1 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version1

    [<TestMethod>]
    member _.NewVersion1RSequence_WithoutInput_GetGuidsWithSameNodeId() =
        Guid.newV1RSeq ()
        |> Seq.take 2
        |> tee (Seq.map Guid.version
                >> Seq.iter (Assert.equalTo Guid.Version.Version1))
        |> Seq.map Guid.tryGetNodeId
        |> tee (Seq.iter (Assert.true' << ValueOption.isSome))
        |> Seq.map ValueOption.get
        |> Seq.toArray
        |> fun nodes -> (nodes.[0], nodes.[1])
        |> CollectionAssert.AreEqual

    [<TestMethod>]
    member _.NewVersion2_PersonDomain_GetGuidOfVersion2() =
        Guid.newV2 Guid.Domain.Person
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version2

    [<TestMethod>]
    member _.NewVersion2OfOrg_SpecifiedLocalId_GetFieldOfInputLocalId() =
        Guid.newV2Org 0x01234567
        |> Guid.toFields
        |> fun struct (localId, _, _, _, _) -> localId
        |> Assert.equalTo 0x01234567

    [<TestMethod>]
    member _.NewVersion2_InvaildDomain_CatchArgumentOutofRangeException() =
        fun () -> Guid.newV2 (enumof 0xFFuy) |> ignore
        |> Assert.exception'<ArgumentOutOfRangeException>

    [<TestMethod>]
    member _.NewVersion3ByString_SpecifiedUrl_GetExpectedGuid() =
        Guid.newV3S Guid.nsUrl "https://github.com/x-stars/GuidGenerator"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version3)
        |> Assert.equalTo (Guid.parse "a9ec4420-7252-3c11-ab70-512e10273537")

    [<TestMethod>]
    member _.NewVersion4_WithoutInput_GetGuidOfVersion4() =
        Guid.newV4 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4

    [<TestMethod>]
    member _.NewVersion5ByString_SpecifiedUrl_GetExpectedGuid() =
        Guid.newV5S Guid.nsUrl "https://github.com/x-stars/GuidGenerator"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version5)
        |> Assert.equalTo (Guid.parse "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24")

    [<TestMethod>]
    member _.CreateByFields_DeconstructToFields_GetInputFields() =
        Guid.ofFields 0x00112233 0x4455s 0x6677s (0x88uy, 0x99uy)
                      (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy)
        |> Guid.toFields
        |> Assert.equalTo (0x00112233, 0x4455s, 0x6677s, (0x88uy, 0x99uy),
                           (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy))

    [<TestMethod>]
    member _.CreateByByteArray_ConvertToUuidByteArray_GetReversedByteOrder() =
        Array.map byte
            [| 0x00; 0x11; 0x22; 0x33; 0x44; 0x55; 0x66; 0x77
               0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
        |> Guid.ofBytes
        |> Guid.toBytesUuid
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x33; 0x22; 0x11; 0x00; 0x55; 0x44; 0x77; 0x66
               0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |])

    [<TestMethod>]
    member _.CreateByUuidByteArray_ConvertToByteArray_GetReversedByteOrder() =
        Array.map byte
            [| 0x00; 0x11; 0x22; 0x33; 0x44; 0x55; 0x66; 0x77
               0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |]
        |> Guid.ofBytesUuid
        |> Guid.toBytes
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x33; 0x22; 0x11; 0x00; 0x55; 0x44; 0x77; 0x66
               0x88; 0x99; 0xAA; 0xBB; 0xCC; 0xDD; 0xEE; 0xFF |])

    [<TestMethod>]
    member _.TryParseExact_WithSpecifiedFormats_GetExpectedGuids() =
        [| Guid.tryParseExact "N" "00112233445566778899aabbccddeeff"
           Guid.tryParseExact "D" "00112233-4455-6677-8899-aabbccddeeff"
           Guid.tryParseExact "B" "{00112233-4455-6677-8899-aabbccddeeff}"
           Guid.tryParseExact "P" "(00112233-4455-6677-8899-aabbccddeeff)"
           Guid.tryParseExact "X" ("{0x00112233,0x4455,0x6677," +
                                   "{0x88,0x99,0xaa,0xbb,0xcc,0xdd,0xee,0xff}}") |]
        |> Array.iter (
            tee (Assert.true' << ValueOption.isSome)
            >> ValueOption.get
            >> Assert.equalTo (Guid.parse "00112233-4455-6677-8899-aabbccddeeff"))

    [<TestMethod>]
    member _.Format_WithSpecifiedFormats_GetExpectedRepresentations() =
        Guid.parse "00112233-4455-6677-8899-aabbccddeeff"
        |- (Guid.format "N"
            >> Assert.equalTo "00112233445566778899aabbccddeeff")
        |- (Guid.format "D"
            >> Assert.equalTo "00112233-4455-6677-8899-aabbccddeeff")
        |- (Guid.format "B"
            >> Assert.equalTo "{00112233-4455-6677-8899-aabbccddeeff}")
        |- (Guid.format "P"
            >> Assert.equalTo "(00112233-4455-6677-8899-aabbccddeeff)")
        |- (Guid.format "X"
            >> Assert.equalTo ("{0x00112233,0x4455,0x6677," +
                               "{0x88,0x99,0xaa,0xbb,0xcc,0xdd,0xee,0xff}}"))
        |> ignore

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
    member _.GuidVersionInfo_Version1Guid_GetExpectedFlags() =
        Guid.newV1 ()
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.true'  << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version2Guid_GetExpectedFlags() =
        Guid.newV2 Guid.Domain.Group
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.true'  << Guid.hasNodeId)
        |- (Assert.true'  << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version3Guid_GetExpectedFlags() =
        Guid.newV3S Guid.nsDns "github.com"
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.true'  << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version4Guid_GetExpectedFlags() =
        Guid.newV4 ()
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.true'  << Guid.isRandomized)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version5Guid_GetExpectedFlags() =
        Guid.newV5S Guid.nsDns "github.com"
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.true'  << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

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
