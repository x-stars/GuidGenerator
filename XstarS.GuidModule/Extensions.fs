namespace XNetEx.Guids.Generators

open System
open System.Runtime.CompilerServices
#if !UUIDREV_DISABLE
open System.Security.Cryptography
#endif

/// <summary>
/// Provides extra extension methods for <see cref="T:XNetEx.Guids.Generators.GuidGenerator"/>.
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
        [<Extension>]
        [<CompiledName("AsSequence")>]
        member this.AsSequence() : seq<Guid> =
            seq {
#if !UUIDREV_DISABLE
                use _ = this :> IDisposable
#endif
                while true do
                    yield this.NewGuid()
            }

#if !UUIDREV_DISABLE
    type private WeakTableOfHashing = ConditionalWeakTable<HashAlgorithm, INameBasedGuidGenerator>

    [<CompiledName("Version8NCache")>]
    let private v8NCache = WeakTableOfHashing()

    [<CompiledName("CreateVersion8NCallback")>]
    let private createV8N = WeakTableOfHashing.CreateValueCallback(GuidGenerator.CreateVersion8N)

    type GuidGenerator with

        /// <summary>
        /// Gets the cached <see cref="T:XNetEx.Guids.Generators.INameBasedGuidGenerator"/> instance
        /// of the specified hash algorithm. If the hash algorithm does not exist in the cache, a new
        /// <see cref="T:XNetEx.Guids.Generators.INameBasedGuidGenerator"/> instance will be created.
        /// </summary>
        /// <param name="hashing">The hash algorithm to get the cache.</param>
        /// <returns>The cached <see cref="T:XNetEx.Guids.Generators.INameBasedGuidGenerator"/>
        /// instance of <paramref name="hashing"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="hashing"/> is null.</exception>
        [<CompiledName("GetVersion8NCache")>]
        static member GetVersion8NCache(hashing: HashAlgorithm) : INameBasedGuidGenerator =
            v8NCache.GetValue(hashing, createV8N)
#endif
