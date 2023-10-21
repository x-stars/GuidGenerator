namespace XNetEx.FSharp.Core

open System
open System.Diagnostics
open System.IO
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.Control
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
[<DoNotParallelize>]
type GuidGeneratorStateTest() =

    let catchStateExn () : ref<exn> =
        let refExn = ref null
        Guid.onStateExn (fun e ->
            if e.OperationType = FileAccess.Read then
                refExn.Value <- e.Exception)
        refExn

    let createTempFile (fileName: outref<string>): IDisposable =
        let tempFile = Path.GetTempFileName()
        fileName <- tempFile
        new DisposeAction(fun _ ->
            if File.Exists(tempFile) then
                try
                    File.Delete(tempFile)
                with
                | ex -> Debug.WriteLine(ex))

    [<TestMethod>]
    member _.LoadGeneratorState_FileWithRandomNodeId_GetNodeIdFromFile() =
        let refExn = catchStateExn ()
        let mutable fileName = null
        use tempFile = createTempFile &fileName
        let addByte1 = (op (+) 1) >> byte
        if true then
            use stream = new FileStream(fileName, FileMode.Create)
            use writer = new BinaryWriter(stream)
            writer.Write(4122)
            writer.Write(0x08)
            writer.Write(0L)
            writer.Write(0)
            writer.Write((Array.init 6 byte), 0, 6)
            writer.Write((Array.init 6 addByte1), 0, 6)
        fileName
        |> Guid.loadState
        |> Assert.true'
        refExn.Value
        |> Assert.null'
        Guid.newV1R ()
        |> Guid.tryGetNodeId
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> Assert.Seq.equalTo (Array.init 6 addByte1)

    [<TestMethod>]
    member _.OnStateException_NonExistingFile_CatchFileNotFoundException() =
        let refExn = catchStateExn ()
        let mutable fileName = null
        use tempFile = createTempFile &fileName
        fileName
        |> tee (File.Delete)
        |> Guid.loadState
        |> Assert.false'
        refExn.Value
        |> Assert.ofType<FileNotFoundException>

    [<TestCleanup>]
    member _.ResetGeneratorState() =
        let mutable fileName = null
        use tempFile = createTempFile &fileName
        if true then
            let emptyNodeId = Array.zeroCreate<byte> 6
            use stream = new FileStream(fileName, FileMode.Create)
            use writer = new BinaryWriter(stream)
            writer.Write(4122)
            writer.Write(0x00)
            writer.Write(0L)
            writer.Write(0)
            writer.Write(emptyNodeId, 0, 6)
            writer.Write(emptyNodeId, 0, 6)
        Guid.loadState fileName |> ignore
