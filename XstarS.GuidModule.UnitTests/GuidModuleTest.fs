namespace XNetEx

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.UnitTesting
open XNetEx.UnitTesting.MSTest

[<TestClass>]
type GuidModuleTest() =

    [<TestMethod>]
    member _.CreateByOfFields_DeconstructByToFields_GetInputFields() =
        Guid.ofFields 0x00112233 0x4455s 0x6677s (0x88uy, 0x99uy)
                      (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy)
        |> Guid.toFields
        |> Assert.equalTo (0x00112233, 0x4455s, 0x6677s, (0x88uy, 0x99uy),
                           (0xAAuy, 0xBBuy, 0xCCuy, 0xDDuy, 0xEEuy, 0xFFuy))

    [<TestMethod>]
    member _.NewVersion4_WithoutInput_GetGuidWithVersion4() =
        Guid.newV4 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4

    [<TestMethod>]
    member _.TryGetNodeId_Version1Guid_GetExpectedNodeId() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetNodeId
        |- (ValueOption.isSome
            >> Assert.true')
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]

    [<TestMethod>]
    member _.NewVersion2_InvaildDomain_CatchArgumentOutofRangeException() =
        fun () -> Guid.newV2 (enumof 0xFFuy) |> ignore
        |> Assert.exception'<ArgumentOutOfRangeException>
