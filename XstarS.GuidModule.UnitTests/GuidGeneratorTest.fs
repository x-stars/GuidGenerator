﻿namespace XNetEx.FSharp.Core

open System
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

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.NewVersion1RPooledSequence_WithoutInput_GetGuidsOfVersion1() =
        Guid.newV1RPoolSeq ()
        |> Seq.take 10
        |> Seq.map Guid.version
        |> Seq.iter (Assert.equalTo Guid.Version.Version1)
#endif

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
    member _.NewVersion3_EmptyName_GetExpectedGuid() =
        Guid.newV3 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version3)
        |> Assert.equalTo (Guid.parse "4ae71336-e44b-39bf-b9d2-752e234818a5")

    [<TestMethod>]
    member _.NewVersion3ByString_SpecifiedUrl_GetExpectedGuid() =
        Guid.newV3S Guid.nsUrl "https://github.com/x-stars/GuidGenerator"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version3)
        |> Assert.equalTo (Guid.parse "a9ec4420-7252-3c11-ab70-512e10273537")

    [<TestMethod>]
    member _.NewVersion3ByEncoding_SpecifiedUrl_GetExpectedGuid() =
        "https://github.com/x-stars/GuidGenerator"
        |> Guid.newV3Enc Guid.nsUrl Text.Encoding.Unicode
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version3)
        |> Assert.equalTo (Guid.parse "cba55721-443e-3ba5-a123-98b80572e58d")

    [<TestMethod>]
    member _.NewVersion4_WithoutInput_GetGuidOfVersion4() =
        Guid.newV4 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version4

    [<TestMethod>]
    member _.NewVersion5_EmptyName_GetExpectedGuid() =
        Guid.newV5 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version5)
        |> Assert.equalTo (Guid.parse "e129f27c-5103-5c5c-844b-cdf0a15e160d")

    [<TestMethod>]
    member _.NewVersion5ByString_SpecifiedUrl_GetExpectedGuid() =
        Guid.newV5S Guid.nsUrl "https://github.com/x-stars/GuidGenerator"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version5)
        |> Assert.equalTo (Guid.parse "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24")

    [<TestMethod>]
    member _.NewVersion5ByEncoding_SpecifiedUrl_GetExpectedGuid() =
        "https://github.com/x-stars/GuidGenerator"
        |> Guid.newV5Enc Guid.nsUrl Text.Encoding.Unicode
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version5)
        |> Assert.equalTo (Guid.parse "747e7776-7df9-55eb-bf56-c1b6aaebf422")

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.NewVersion6_WithoutInput_GetGuidOfVersion6() =
        Guid.newV6 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version6

    [<TestMethod>]
    member _.NewVersion6P_WithoutInput_GetNodeIdWithEvenFirstByte() =
        Guid.newV6P ()
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun nodeId -> nodeId[0] &&& 0x01uy
        |> Assert.equalTo 0x00uy

    [<TestMethod>]
    member _.NewVersion6R_WithoutInput_GetNodeIdWithOddFirstByte() =
        Guid.newV6R ()
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun nodeId -> nodeId[0] &&& 0x01uy
        |> Assert.equalTo 0x01uy

    [<TestMethod>]
    member _.NewVersion6RSequence_WithoutInput_GetGuidsWithSameNodeId() =
        Guid.newV6RSeq ()
        |> Seq.take 2
        |> tee (Seq.map Guid.version
                >> Seq.iter (Assert.equalTo Guid.Version.Version6))
        |> Seq.map Guid.tryGetNodeId
        |> tee (Seq.iter (Assert.true' << ValueOption.isSome))
        |> Seq.map ValueOption.get
        |> Seq.toArray
        |> fun nodes -> (nodes.[0], nodes.[1])
        |> CollectionAssert.AreEqual

    [<TestMethod>]
    member _.NewVersion6RPooledSequence_WithoutInput_GetGuidsOfVersion6() =
        Guid.newV6RPoolSeq ()
        |> Seq.take 10
        |> Seq.map Guid.version
        |> Seq.iter (Assert.equalTo Guid.Version.Version6)

    [<TestMethod>]
    member _.NewVersion7_WithoutInput_GetGuidOfVersion7() =
        Guid.newV7 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version7

    [<TestMethod>]
    member _.NewVersion7_WithoutInput_GetMonotonicVersion7Guids() =
        fun _ -> Guid.newV7 ()
        |> Seq.init 1000
        |> tee (Seq.map Guid.version
                >> Seq.iter (Assert.equalTo Guid.Version.Version7))
        |> Seq.pairwise
        |> Seq.map (fun (lastGuid, guid) -> guid.CompareTo(lastGuid) > 0)
        |> Assert.Seq.notContains false
#endif
