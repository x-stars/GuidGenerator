namespace XNetEx.FSharp.Core

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidModuleTest() =

    [<TestMethod>]
    member _.EmptyGuid_ToFields_GetAllFieldsZero() =
        Guid.empty
        |> Guid.toFields
        |> Assert.equalTo (0, 0s, 0s, (0uy, 0uy),
                           (0uy, 0uy, 0uy, 0uy, 0uy, 0uy))

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.GuidMaxValue_ToFields_GetAllFieldsMaxValue() =
        Guid.maxValue
        |> Guid.toFields
        |> Assert.equalTo (0xFFFFFFFF, 0xFFFFs, 0xFFFFs, (0xFFuy, 0xFFuy),
                           (0xFFuy, 0xFFuy, 0xFFuy, 0xFFuy, 0xFFuy, 0xFFuy))
#endif

    [<TestMethod>]
    member _.CreateByFields_DeconstructToFields_GetInputFields() =
        Guid.ofFields 0x00112233 0x4455s 0x6677s (0x88uy, 0x99uy)
                      (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy)
        |> Guid.toFields
        |> Assert.equalTo (0x00112233, 0x4455s, 0x6677s, (0x88uy, 0x99uy),
                           (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy))

#if NET7_0_OR_GREATER
    [<TestMethod>]
    member _.CreateByUInt128_ConvertToUInt128_GetOriginalValue() =
        Guid.ofUInt128 (System.UInt128(0x0011223344556677uL, 0x8899AABBCCDDEEFFuL))
        |> Guid.toUInt128
        |> Assert.equalTo (System.UInt128(0x0011223344556677uL, 0x8899AABBCCDDEEFFuL))
#endif

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
                                   "{0x88,0x99,0xaa,0xbb,0xcc,0xdd,0xee,0xff}}")
           Guid.tryParseUrn "urn:uuid:00112233-4455-6677-8899-aabbccddeeff" |]
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
        |- (Guid.formatUrn
            >> Assert.equalTo "urn:uuid:00112233-4455-6677-8899-aabbccddeeff")
        |> ignore
