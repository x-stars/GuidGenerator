namespace XNetEx.FSharp.Core

open System
open System.Diagnostics
open System.IO
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
[<DoNotParallelize>]
type GuidGeneratorStateTest() =

    [<TestMethod>]
    member _.LoadGeneratorState_NonExistingFile_CatchFileNotFoundException() =
        let mutable exception': Exception = null
        Guid.onStateExn (fun e ->
            if e.OperationType = FileAccess.Read then
                exception' <- e.Exception)
        let fileName = Path.GetTempFileName()
        try
            fileName
            |> tee (File.Delete)
            |> Guid.loadState
            |> Assert.false'
            exception'
            |> Assert.ofType<FileNotFoundException>
        finally
            if File.Exists(fileName) then
                try
                    File.Delete(fileName)
                with
                | ex -> Debug.WriteLine(ex)

    [<TestMethod>]
    member _.LoadGeneratorState_EmptyFile_CatchEndOfStreamException() =
        let mutable exception': Exception = null
        Guid.onStateExn (fun e ->
            if e.OperationType = FileAccess.Read then
                exception' <- e.Exception)
        let fileName = Path.GetTempFileName()
        try
            fileName
            |> Guid.loadState
            |> Assert.false'
            exception'
            |> Assert.ofType<EndOfStreamException>
        finally
            if File.Exists(fileName) then
                try
                    File.Delete(fileName)
                with
                | ex -> Debug.WriteLine(ex)

    [<TestMethod>]
    member _.LoadGeneratorState_UnknownVersioNumber_CatchInvalidDataException() =
        let mutable exception': Exception = null
        Guid.onStateExn (fun e ->
            if e.OperationType = FileAccess.Read then
                exception' <- e.Exception)
        let fileName = Path.GetTempFileName()
        try
            if true then
                use stream = new FileStream(fileName, FileMode.Create)
                use writer = new BinaryWriter(stream)
                writer.Write(1234)
            fileName
            |> Guid.loadState
            |> Assert.false'
            exception'
            |> Assert.ofType<InvalidDataException>
        finally
            if File.Exists(fileName) then
                try
                    File.Delete(fileName)
                with
                | ex -> Debug.WriteLine(ex)

    [<TestMethod>]
    member _.LoadGeneratorState_FileWithRandomNodeId_GetNodeIdFromFile() =
        let mutable exception': Exception = null
        Guid.onStateExn (fun e ->
            if e.OperationType = FileAccess.Read then
                exception' <- e.Exception)
        let fileName = Path.GetTempFileName()
        try
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
            exception'
            |> Assert.null'
            Guid.newV1R ()
            |> Guid.tryGetNodeId
            |> tee (Assert.true' << ValueOption.isSome)
            |> ValueOption.get
            |> Assert.Seq.equalTo (Array.init 6 addByte1)
        finally
            if File.Exists(fileName) then
                try
                    File.Delete(fileName)
                with
                | ex -> Debug.WriteLine(ex)
