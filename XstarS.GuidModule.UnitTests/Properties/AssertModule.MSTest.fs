namespace XNetEx.UnitTesting.Adapters.MSTest

open System
open System.Collections
open System.Text.RegularExpressions
open Microsoft.FSharp.Core.LanguagePrimitives
open Microsoft.VisualStudio.TestTools.UnitTesting

[<AutoOpen>]
module internal AssertOperators =

    [<CompiledName("EnumOfValue")>]
    let inline enum<'T, 'Enum when 'Enum: enum<'T>> value =
        EnumOfValue<'T, 'Enum>(value)

    [<CompiledName("TeeAction")>]
    let inline tee (action: 'T -> unit) value =
        action value
        value

[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module internal Assert =

    type private SeqWrapper<'T>(source: seq<'T>) =
        interface ICollection with
            member _.Count = Seq.length source
            member _.IsSynchronized = false
            member _.SyncRoot = source
            member _.CopyTo(array, index) =
                source |> Seq.iteri (fun num item ->
                    array.SetValue(item, index + num))
            member _.GetEnumerator() = source.GetEnumerator()

    [<CompiledName("FailWith")>]
    let failWith message = Assert.Fail(message)

    [<CompiledName("Inconclusive")>]
    let inconclusive message = Assert.Inconclusive(message)

    [<CompiledName("True")>]
    let true' condition = Assert.IsTrue(condition)

    [<CompiledName("TrueWith")>]
    let trueWith message condition =
        Assert.IsTrue(condition, message)

    [<CompiledName("False")>]
    let false' condition = Assert.IsFalse(condition)

    [<CompiledName("FalseWith")>]
    let falseWith message condition =
        Assert.IsFalse(condition, message)

    [<CompiledName("Null")>]
    let null' (value: obj) = Assert.IsNull(value)

    [<CompiledName("NotNull")>]
    let notNull (value: obj) = Assert.IsNotNull(value)

    [<CompiledName("EqualTo")>]
    let equalTo<'T> expected actual =
        Assert.AreEqual<'T>(expected, actual)

    [<CompiledName("NotEqualTo")>]
    let notEqualTo<'T> expected actual =
        Assert.AreNotEqual<'T>(expected, actual)

    [<CompiledName("SameAs")>]
    let sameAs (expected: obj) (actual: obj) =
        Assert.AreSame(expected, actual)

    [<CompiledName("NotSameAs")>]
    let notSameAs (expected: obj) (actual: obj) =
        Assert.AreNotSame(expected, actual)

    [<CompiledName("OfType")>]
    let ofType<'T> (value: obj) =
        Assert.IsInstanceOfType(value, typeof<'T>)

    [<CompiledName("NotOfType")>]
    let notOfType<'T> (value: obj) =
        Assert.IsNotInstanceOfType(value, typeof<'T>)

    [<CompiledName("InSequence")>]
    let inSeq<'T> sequence (value: 'T) =
        CollectionAssert.Contains(SeqWrapper<'T>(sequence), value)

    [<CompiledName("NotInSequence")>]
    let notInSeq<'T> sequence (value: 'T) =
        CollectionAssert.DoesNotContain(SeqWrapper<'T>(sequence), value)

    [<CompiledName("Exception")>]
    let exception'<'T when 'T :> exn> action =
        Assert.ThrowsException<'T>(Action(action)) |> ignore

    module Seq =

        [<CompiledName("AllNotNull")>]
        let allNotNull<'T> sequence =
            CollectionAssert.AllItemsAreNotNull(SeqWrapper<'T>(sequence))

        [<CompiledName("AllOfType")>]
        let allOfType<'T> sequence =
            CollectionAssert.AllItemsAreInstancesOfType(
                SeqWrapper(Seq.cast<obj> sequence), typeof<'T>)

        [<CompiledName("ItemsUnique")>]
        let itemsUnique<'T> sequence =
            CollectionAssert.AllItemsAreUnique(SeqWrapper<'T>(sequence))

        [<CompiledName("EqualTo")>]
        let equalTo<'T> expected actual =
            CollectionAssert.AreEqual(
                SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        [<CompiledName("NotEqualTo")>]
        let notEqualTo<'T> expected actual =
            CollectionAssert.AreNotEqual(
                SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        [<CompiledName("SetEqualTo")>]
        let setEqualTo<'T> expected actual =
            CollectionAssert.AreEquivalent(
                SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        [<CompiledName("NotSetEqualTo")>]
        let notSetEqualTo<'T> expected actual =
            CollectionAssert.AreNotEquivalent(
                SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        [<CompiledName("Contains")>]
        let contains<'T> (value: 'T) sequence =
            CollectionAssert.Contains(SeqWrapper<'T>(sequence), value)

        [<CompiledName("NotContains")>]
        let notContains<'T> (value: 'T) sequence =
            CollectionAssert.DoesNotContain(SeqWrapper<'T>(sequence), value)

        [<CompiledName("SubsetOf")>]
        let subsetOf<'T> superset sequence =
            CollectionAssert.IsSubsetOf(
                SeqWrapper<'T>(sequence), SeqWrapper<'T>(superset))

        [<CompiledName("NotSubsetOf")>]
        let notSubsetOf<'T> superset sequence =
            CollectionAssert.IsNotSubsetOf(
                SeqWrapper<'T>(sequence), SeqWrapper<'T>(superset))

        [<CompiledName("SupersetOf")>]
        let supersetOf<'T> subset sequence =
            CollectionAssert.IsSubsetOf(
                SeqWrapper<'T>(subset), SeqWrapper<'T>(sequence))

        [<CompiledName("NotSupersetOf")>]
        let notSupersetOf<'T> subset sequence =
            CollectionAssert.IsNotSubsetOf(
                SeqWrapper<'T>(subset), SeqWrapper<'T>(sequence))

    module String =

        [<CompiledName("Contains")>]
        let contains substring value =
            StringAssert.Contains(value, substring)

        [<CompiledName("ContainsWithOption")>]
        let containsOpt (option: StringComparison) substring value =
            StringAssert.Contains(value, substring, option)

        [<CompiledName("StartsWith")>]
        let startsWith substring value =
            StringAssert.StartsWith(value, substring)

        [<CompiledName("StartsWithOption")>]
        let startsWithOpt (option: StringComparison) substring value =
            StringAssert.StartsWith(value, substring, option)

        [<CompiledName("EndsWith")>]
        let endsWith substring value =
            StringAssert.EndsWith(value, substring)

        [<CompiledName("EndsWithOption")>]
        let endsWithOpt (option: StringComparison) substring value =
            StringAssert.EndsWith(value, substring, option)

        [<CompiledName("Match")>]
        let match' pattern value =
            StringAssert.Matches(value, Regex(pattern))

        [<CompiledName("MatchWithOptions")>]
        let matchOpt (options: RegexOptions) pattern value =
            StringAssert.Matches(value, Regex(pattern, options))

        [<CompiledName("NotMatch")>]
        let notMatch pattern value =
            StringAssert.DoesNotMatch(value, Regex(pattern))

        [<CompiledName("NotMatchWithOptions")>]
        let notMatchOpt (options: RegexOptions) pattern value =
            StringAssert.DoesNotMatch(value, Regex(pattern, options))
