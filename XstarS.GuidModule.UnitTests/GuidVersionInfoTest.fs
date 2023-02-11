namespace XNetEx.FSharp.Core

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidVersionInfoTest() =

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
