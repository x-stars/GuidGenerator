namespace XNetEx.FSharp.Core

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidCustomStateSeqBuilderTest() =

    [<TestMethod>]
    member _.CreateCustomStateSeqBuilder_Version1Zero_GetVersion1Guid() =
        Guid.customStateSeq Guid.Version.Version1 { }
        |> Seq.head
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version1

    [<TestMethod>]
    member _.CreateCustomStateSeqBuilder_Version1UseValidProviders_GetExpectedComponents() =
        let mutable timestamp = DateTime.UtcNow.Ticks
        Guid.customStateSeq Guid.Version.Version1 {
            timeFunc (fun () ->
                timestamp <- timestamp + 1L
                DateTime(timestamp, DateTimeKind.Utc))
            clockSeq 0x0123s
            nodeIdFunc (fun () -> Array.init 6 byte)
        }
        |> Seq.head
        |- (Guid.version
            >> Assert.equalTo Guid.Version.Version1)
        |- (Guid.tryGetTime
            >> ValueOption.get
            >> Assert.equalTo (DateTime(timestamp, DateTimeKind.Utc)))
        |- (Guid.tryGetClockSeq
            >> ValueOption.get
            >> Assert.equalTo 0x0123s)
        |- (Guid.tryGetNodeId
            >> ValueOption.get
            >> Assert.Seq.equalTo (Array.init 6 byte))
        |> ignore

    [<TestMethod>]
    member _.CreateCustomStateSeqBuilder_Version1UseValidProvidersAndStates_GetExpectedComponents() =
        let mutable timestamp = DateTime.UtcNow.Ticks
        Guid.customStateSeq Guid.Version.Version1 {
#if NET8_0_OR_GREATER
            timeProvider (CustomTimeProvider(fun () ->
                timestamp <- timestamp + 1L
                DateTimeOffset(timestamp, TimeSpan.Zero)))
#else
            timeFunc (fun () ->
                timestamp <- timestamp + 1L
                DateTimeOffset(timestamp, TimeSpan.Zero))
#endif
            clockSeq 0x0123s
            nodeId (Array.init 6 byte)
        }
        |> Seq.head
        |- (Guid.version
            >> Assert.equalTo Guid.Version.Version1)
        |- (Guid.tryGetTime
            >> ValueOption.get
            >> Assert.equalTo (DateTime(timestamp, DateTimeKind.Utc)))
        |- (Guid.tryGetClockSeq
            >> ValueOption.get
            >> Assert.equalTo 0x0123s)
        |- (Guid.tryGetNodeId
            >> ValueOption.get
            >> Assert.Seq.equalTo (Array.init 6 byte))
        |> ignore

    [<TestMethod>]
    member _.CreateCustomStateSeqBuilder_Version1UseInvalidProviders_CatchArgumentExceptions() =
        let seqBuilder = Guid.customStateSeq Guid.Version.Version1
#if NET8_0_OR_GREATER
        fun () -> seqBuilder { timeProvider null } |> ignore
        |> Assert.exception'<ArgumentNullException> |> ignore
#endif
        fun () -> seqBuilder { nodeIdSource Guid.NodeIdSource.None } |> ignore
        |> Assert.exception'<InvalidOperationException> |> ignore
        fun () -> seqBuilder { nodeIdSource (enumof -1) } |> ignore
        |> Assert.exception'<ArgumentOutOfRangeException> |> ignore
        fun () -> seqBuilder { nodeId null } |> ignore
        |> Assert.exception'<ArgumentNullException> |> ignore

    [<TestMethod>]
    member _.CreateCustomStateSeqBuilderOfVersion_Version1Zero_GetVersion1Guid() =
        Guid.customStateSeqOfVersion 1uy { }
        |> Seq.head
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version1
