namespace XNetEx.FSharp.Core

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidVersionInfoTest() =

    [<TestMethod>]
    member _.GuidVersionInfo_EmptyGuid_GetExpectedFlags() =
        Guid.empty
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version1Guid_GetExpectedFlags() =
        Guid.newV1 ()
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.true'  << Guid.hasClockSeq)
        |- (Assert.true'  << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version2Guid_GetExpectedFlags() =
        Guid.newV2 Guid.Domain.Group
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.true'  << Guid.hasClockSeq)
        |- (Assert.true'  << Guid.hasNodeId)
        |- (Assert.true'  << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version3Guid_GetExpectedFlags() =
        Guid.newV3S Guid.nsDns "github.com"
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.true'  << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version4Guid_GetExpectedFlags() =
        Guid.newV4 ()
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.true'  << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version5Guid_GetExpectedFlags() =
        Guid.newV5S Guid.nsDns "github.com"
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.true'  << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

#if !FEATURE_DISABLE_UUIDREV
    [<TestMethod>]
    member _.GuidVersionInfo_Version6Guid_GetExpectedFlags() =
        Guid.newV6 ()
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.true'  << Guid.hasClockSeq)
        |- (Assert.true'  << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version7Guid_GetExpectedFlags() =
        Guid.newV7 ()
        |- (Assert.true'  << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.true'  << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_Version8Guid_GetExpectedFlags() =
        Guid.newV8 ()
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore

    [<TestMethod>]
    member _.GuidVersionInfo_GuidMaxValue_GetExpectedFlags() =
        Guid.maxValue
        |- (Assert.false' << Guid.isTimeBased)
        |- (Assert.false' << Guid.isNameBased)
        |- (Assert.false' << Guid.isRandomized)
        |- (Assert.false' << Guid.hasClockSeq)
        |- (Assert.false' << Guid.hasNodeId)
        |- (Assert.false' << Guid.hasLocalId)
        |> ignore
#endif
