namespace XNetEx

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.UnitTesting
open XNetEx.UnitTesting.MSTest

[<TestClass>]
type GuidModuleTest() =

    [<TestMethod>]
    member _.NewVersion4_WithoutInput_GetGuidWithVersion4() =
        Guid.newV4 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4

    [<TestMethod>]
    member _.TryGetNodeId_Version1Guid_GetExpectedNodeId() =
        Guid.parse "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
        |> Guid.tryGetNodeId
        |> tee (ValueOption.isSome
                >> Assert.true')
        |> ValueOption.get
        |> Assert.Seq.equalTo [| 0x00uy; 0xc0uy; 0x4fuy; 0xd4uy; 0x30uy; 0xc8uy |]
