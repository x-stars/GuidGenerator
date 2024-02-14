namespace XNetEx.Guids.Generators

open System
open System.Runtime.CompilerServices

/// <summary>
/// Provides extra extension methods for <see cref="GuidGenerator"/>.
/// </summary>
[<AutoOpen; Extension>]
module internal GuidGeneratorExtensions =

    type IGuidGenerator with

        /// <summary>
        /// Creates a new unlimited sequence that generates <see cref="T:System.Guid"/> instances
        /// by the current <see cref="T:XNetEx.Guids.Generators.IGuidGenerator"/>.
        /// </summary>
        /// <returns>A new unlimited sequence that generates <see cref="T:System.Guid"/> instances
        /// by the current <see cref="T:XNetEx.Guids.Generators.IGuidGenerator"/>.</returns>
        [<CompiledName("AsSequence"); Extension>]
        member internal this.AsSequence() : seq<Guid> =
            seq {
#if !UUIDREV_DISABLE
                use _ = this :> IDisposable
#endif
                while true do
                    yield this.NewGuid()
            }
