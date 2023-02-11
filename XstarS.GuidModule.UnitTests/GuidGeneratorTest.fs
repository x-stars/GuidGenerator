namespace XNetEx.FSharp.Core

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidGeneratorTest() =

    [<TestMethod>]
    member _.NewVersion1_WithoutInput_GetGuidOfVersion1() =
        Guid.newV1 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version1

    [<TestMethod>]
    member _.NewVersion1R_WithoutInput_GetNodeIdWithOddFirstByte() =
        Guid.newV1R ()
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun nodeId -> nodeId[0] &&& 0x01uy
        |> Assert.equalTo 0x01uy

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
    member _.NewVersion2OfOther_UnknownDomain_GetGuidWithInputDomainAndLocalId() =
        Guid.newV2Other (enumof 0xFFuy) 0xFFFF
        |> Guid.tryGetLocalId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.equalTo ((enumof 0xFFuy), 0xFFFF)

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
