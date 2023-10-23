namespace XNetEx.FSharp.Control

open System

[<Sealed>]
type internal DisposeAction(action: bool -> unit) =
    inherit obj()

    let mutable isDisposed = false

    override this.Finalize() =
        try
            this.Dispose(false)
        finally
            base.Finalize()

    member this.Dispose() =
        this.Dispose(true)
        GC.SuppressFinalize(this)

    member private _.Dispose(disposing: bool) =
        if not isDisposed then
            action disposing
            isDisposed <- true

    interface IDisposable with
        member this.Dispose() =
            this.Dispose()
