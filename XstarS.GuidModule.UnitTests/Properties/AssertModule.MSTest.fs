﻿// Copyright (c) 2022 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace XNetEx.UnitTesting

/// <summary>
/// Contains a set of operators and functions for unit testing.
/// </summary>
[<AutoOpen>]
module internal UnitTestOperators =

    /// <summary>
    /// Apply an action to a value and returns the value,
    /// the value being on the left, the action on the right.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="action">The action to apply.</param>
    /// <returns>The input value.</returns>
    let inline ( |- ) value (action: 'T -> unit) =
        action value
        value

    /// <summary>
    /// Apply an action to a value and returns the value.
    /// </summary>
    /// <param name="action">The action to apply.</param>
    /// <param name="value">The input value.</param>
    /// <returns>The input value.</returns>
    [<CompiledName("TeeAction")>]
    let inline tee (action: 'T -> unit) value =
        action value
        value

    /// <summary>
    /// Build an enum value from an underlying value.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <returns>The value as an enumeration.</returns>
    [<CompiledName("EnumOfValue")>]
    let inline enumof<'T, 'Enum when 'Enum: enum<'T>> value =
        LanguagePrimitives.EnumOfValue<'T, 'Enum> value

namespace XNetEx.UnitTesting.MSTest

open System
open System.Collections
open System.Text.RegularExpressions
open Microsoft.VisualStudio.TestTools.UnitTesting

/// <summary>
/// A collection of functions to test various conditions within unit tests.
/// If the condition being tested is not met, an exception is raised.
/// </summary>
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module internal Assert =

    /// <summary>
    /// Provides a <see cref="T:System.Collections.ICollection"/> wrapper to
    /// the type <see cref="T:Microsoft.FSharp.Collections.seq`1"/>.
    /// </summary>
    /// <param name="source">The <see cref="T:Microsoft.FSharp.Collections.seq`1"/>.</param>
    [<Sealed>]
    type private SeqWrapper<'T>(source: seq<'T>) =
        interface ICollection with
            member _.Count = Seq.length source
            member _.IsSynchronized = false
            member _.SyncRoot = source
            member _.CopyTo(array, index) =
                source |> Seq.iteri (fun num item ->
                    array.SetValue(item, index + num))
            member _.GetEnumerator() = source.GetEnumerator()

    /// <summary>
    /// Raises an <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"/>.
    /// </summary>
    /// <param name="message">The message to include in the exception.
    /// The message is shown in test results.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Always raised.</exception>
    [<CompiledName("FailWith")>]
    let failWith message = Assert.Fail(message)

    /// <summary>
    /// Raises an <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertInconclusiveException"/>.
    /// </summary>
    /// <param name="message">The message to include in the exception.
    /// The message is shown in test results.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertInconclusiveException">
    /// Always raised.</exception>
    [<CompiledName("Inconclusive")>]
    let inconclusive message = Assert.Inconclusive(message)

    /// <summary>
    /// Tests whether the specified condition is <see langword="true"/>
    /// and raises an exception if the condition is <see langword="false"/>.
    /// </summary>
    /// <param name="condition">The condition the test expects to be <see langword="true"/>.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="condition"/> is <see langword="false"/>.</exception>
    [<CompiledName("True")>]
    let true' condition = Assert.IsTrue(condition)

    /// <summary>
    /// Tests whether the specified condition is <see langword="true"/>
    /// and raises an exception if the condition is <see langword="false"/>.
    /// </summary>
    /// <param name="condition">The condition the test expects to be <see langword="true"/>.</param>
    /// <param name="message">The message to include in the exception
    /// when <paramref name="condition"/> is <see langword="false"/>.
    /// The message is shown in test results.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="condition"/> is <see langword="false"/>.</exception>
    [<CompiledName("TrueOrElse")>]
    let trueOrElse message condition =
        Assert.IsTrue(condition, message)

    /// <summary>
    /// Tests whether the specified condition is <see langword="false"/>
    /// and raises an exception if the condition is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition the test expects to be <see langword="false"/>.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="condition"/> is <see langword="true"/>.</exception>
    [<CompiledName("False")>]
    let false' condition = Assert.IsFalse(condition)

    /// <summary>
    /// Tests whether the specified condition is <see langword="false"/>
    /// and raises an exception if the condition is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition the test expects to be <see langword="false"/>.</param>
    /// <param name="message">The message to include in the exception
    /// when <paramref name="condition"/> is <see langword="true"/>.
    /// The message is shown in test results.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="condition"/> is <see langword="true"/>.</exception>
    [<CompiledName("FalseOrElse")>]
    let falseOrElse message condition =
        Assert.IsFalse(condition, message)

    /// <summary>
    /// Tests whether the specified object is <see langword="null"/> and raises an exception if it is not.
    /// </summary>
    /// <param name="value">The object the test expects to be <see langword="null"/>.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="value"/> is not <see langword="null"/>.</exception>
    [<CompiledName("Null")>]
    let null' (value: obj) = Assert.IsNull(value)

    /// <summary>
    /// Tests whether the specified object is non-<see langword="null"/>
    /// and raises an exception if it is <see langword="null"/>.
    /// </summary>
    /// <param name="value">The object the test expects not to be <see langword="null"/>.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="value"/> is <see langword="null"/>.</exception>
    [<CompiledName("NotNull")>]
    let notNull (value: obj) = Assert.IsNotNull(value)

    /// <summary>
    /// Tests whether the specified values are equal
    /// and raises an exception if the two values are not equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="expected">The first value to compare.
    /// This is the value the tests expects.</param>
    /// <param name="actual">The second value to compare.
    /// This is the value produced by the code under test.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
    [<CompiledName("EqualTo")>]
    let equalTo<'T> expected actual =
        Assert.AreEqual<'T>(expected, actual)

    /// <summary>
    /// Tests whether the specified values are unequal
    /// and raises an exception if the two values are equal.
    /// </summary>
    /// <typeparam name="T">The type of values to compare.</typeparam>
    /// <param name="notExpected">The first value to compare.
    /// This is the value the test expects not to match <paramref name="actual"/>.</param>
    /// <param name="actual">The second value to compare.
    /// This is the value produced by the code under test.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.</exception>
    [<CompiledName("NotEqualTo")>]
    let notEqualTo<'T> notExpected actual =
        Assert.AreNotEqual<'T>(notExpected, actual)

    /// <summary>
    /// Tests whether the specified objects both refer to the same object
    /// and raises an exception if the two inputs do not refer to the same object.
    /// </summary>
    /// <param name="expected">The first value to compare.
    /// This is the value the tests expects.</param>
    /// <param name="actual">The second value to compare.
    /// This is the value produced by the code under test.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="expected"/> does not refer to
    /// the same object as <paramref name="actual"/>.</exception>
    [<CompiledName("SameAs")>]
    let sameAs (expected: obj) (actual: obj) =
        Assert.AreSame(expected, actual)

    /// <summary>
    /// Tests whether the specified objects refer to different objects
    /// and raises an exception if the two inputs refer to the same object.
    /// </summary>
    /// <param name="notExpected">The first value to compare.
    /// This is the value the test expects not to match <paramref name="actual"/>.</param>
    /// <param name="actual">The second value to compare.
    /// This is the value produced by the code under test.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="notExpected"/> refers to
    /// the same object as <paramref name="actual"/>.</exception>
    [<CompiledName("NotSameAs")>]
    let notSameAs (notExpected: obj) (actual: obj) =
        Assert.AreNotSame(notExpected, actual)

    /// <summary>
    /// Tests whether the specified object is an instance of the expected type
    /// and raises an exception if the expected type is not in the inheritance hierarchy of the object.
    /// </summary>
    /// <typeparam name="T">The expected type of <paramref name="value"/>.</typeparam>
    /// <param name="value">The object the test expects to be of the specified type.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="value"/> is <see langword="null"/> or <typeparamref name="T"/>
    /// is not in the inheritance hierarchy of <paramref name="value"/>.</exception>
    [<CompiledName("OfType")>]
    let ofType<'T> (value: obj) =
        Assert.IsInstanceOfType(value, typeof<'T>)

    /// <summary>
    /// Tests whether the specified object is not an instance of the wrong type
    /// and raises an exception if the specified type is in the inheritance hierarchy of the object.
    /// </summary>
    /// <typeparam name="T">The type that <paramref name="value"/> should not be.</typeparam>
    /// <param name="value">The object the test expects not to be of the specified type.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="value"/> is not <see langword="null"/> and <typeparamref name="T"/>
    /// is in the inheritance hierarchy of <paramref name="value"/>.</exception>
    [<CompiledName("NotOfType")>]
    let notOfType<'T> (value: obj) =
        Assert.IsNotInstanceOfType(value, typeof<'T>)

    /// <summary>
    /// Tests whether the specified sequence contains the specified element
    /// and raises an exception if the element is not in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence in which to search for the element.</param>
    /// <param name="element">The element that is expected to be in the sequence.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="element"/> is not found in <paramref name="sequence"/>.</exception>
    [<CompiledName("InSequence")>]
    let inSeq<'T> sequence (element: 'T) =
        CollectionAssert.Contains(SeqWrapper<'T>(sequence), element)

    /// <summary>
    /// Tests whether the specified sequence does not contain the specified element
    /// and raises an exception if the element is in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="sequence">The sequence in which to search for the element.</param>
    /// <param name="element">The element that is expected not to be in the sequence.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="element"/> is found in <paramref name="sequence"/>.</exception>
    [<CompiledName("NotInSequence")>]
    let notInSeq<'T> sequence (element: 'T) =
        CollectionAssert.DoesNotContain(SeqWrapper<'T>(sequence), element)

    /// <summary>
    /// Tests whether the code specified by <paramref name="action"/> raises
    /// exact given exception of type <typeparamref name="T"/> (and not of derived type)
    /// and raises <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"/>
    /// if code does not raises exception or raises exception of type other than <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of exception expected to be raised.</typeparam>
    /// <param name="action">Function of code to be tested and which is expected to raise exception.</param>
    /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
    /// Raised if <paramref name="action"/> does not raises exception of type <typeparamref name="T"/>.</exception>
    [<CompiledName("Exception")>]
    let exception'<'T when 'T :> exn> action =
        Assert.ThrowsException<'T>(Action(action)) |> ignore

    /// <summary>
    /// A collection of functions to test various conditions associated with sequences within unit tests.
    /// If the condition being tested is not met, an exception is raised.
    /// </summary>
    [<RequireQualifiedAccess>]
    module Seq =

        /// <summary>
        /// Tests whether all elements in the specified sequence are non-<see langword="null"/>
        /// and raises an exception if any element is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence in which to search for null elements.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if a <see langword="null"/> element is found in <paramref name="sequence"/>.</exception>
        [<CompiledName("AllNotNull")>]
        let allNotNull<'T> sequence =
            CollectionAssert.AllItemsAreNotNull(SeqWrapper<'T>(sequence))

        /// <summary>
        /// Tests whether all elements in the specified sequence are instances of the expected type
        /// and raises an exception if the expected type is not in the inheritance hierarchy
        /// of one or more of the elements.
        /// </summary>
        /// <typeparam name="T">The expected type of each element of <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">
        /// The sequence containing elements the test expects to be of the specified type.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if an element in <paramref name="sequence"/> is <see langword="null"/>
        /// or <typeparamref name="T"/> is not in the inheritance hierarchy
        /// of an element in <paramref name="sequence"/>.</exception>
        [<CompiledName("AllOfType")>]
        let allOfType<'T> sequence =
            CollectionAssert.AllItemsAreInstancesOfType(SeqWrapper(Seq.cast<obj> sequence), typeof<'T>)

        /// <summary>
        /// Tests whether all items in the specified sequence are unique or not
        /// and raises if any two elements in the sequence are equal.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence in which to search for duplicate elements.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if a two or more equal elements are found in <paramref name="sequence"/>.</exception>
        [<CompiledName("ItemsUnique")>]
        let itemsUnique<'T> sequence =
            CollectionAssert.AllItemsAreUnique(SeqWrapper<'T>(sequence))

        /// <summary>
        /// Tests whether the specified sequences are equal
        /// and raises an exception if the two sequences are not equal.
        /// Equality is defined as having the same elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="expected">The first sequence to compare.
        /// This is the sequence the tests expects.</param>
        /// <param name="actual">The second sequence to compare.
        /// This is the sequence produced by the code under test.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [<CompiledName("EqualTo")>]
        let equalTo<'T> expected actual =
            CollectionAssert.AreEqual(SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        /// <summary>
        /// Tests whether the specified sequences are unequal
        /// and raises an exception if the two sequences are equal.
        /// Equality is defined as having the same elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="notExpected">The first sequence to compare.
        /// This is the sequence the tests expects not to match <paramref name="actual"/>.</param>
        /// <param name="actual">The second sequence to compare.
        /// This is the sequence produced by the code under test.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.</exception>
        [<CompiledName("NotEqualTo")>]
        let notEqualTo<'T> notExpected actual =
            CollectionAssert.AreNotEqual(SeqWrapper<'T>(notExpected), SeqWrapper<'T>(actual))

        /// <summary>
        /// Tests whether two sequences contain the same elements
        /// and raises an exception if either sequence contains an element not in the other sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="expected">The first sequence to compare.
        /// This is the sequence the tests expects.</param>
        /// <param name="actual">The second sequence to compare.
        /// This is the sequence produced by the code under test.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if an element was found in one of the sequences but not the other.</exception>
        [<CompiledName("SetEqualTo")>]
        let setEqualTo<'T> expected actual =
            CollectionAssert.AreEquivalent(SeqWrapper<'T>(expected), SeqWrapper<'T>(actual))

        /// <summary>
        /// Tests whether two sequences contain the different elements
        /// and raises an exception if the two sequences contain identical elements without regard to order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="notExpected">The first sequence to compare.
        /// This is the sequence the tests expects to be different than the actual sequence.</param>
        /// <param name="actual">The second sequence to compare.
        /// This is the sequence produced by the code under test.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if the two sequences contained the same elements,
        /// including the same number of duplicate occurrences of each element.</exception>
        [<CompiledName("NotSetEqualTo")>]
        let notSetEqualTo<'T> notExpected actual =
            CollectionAssert.AreNotEquivalent(SeqWrapper<'T>(notExpected), SeqWrapper<'T>(actual))

        /// <summary>
        /// Tests whether the specified sequence contains the specified element
        /// and raises an exception if the element is not in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="element">The element that is expected to be in the sequence.</param>
        /// <param name="sequence">The sequence in which to search for the element.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="element"/> is not found in <paramref name="sequence"/>.</exception>
        [<CompiledName("Contains")>]
        let contains<'T> (element: 'T) sequence =
            CollectionAssert.Contains(SeqWrapper<'T>(sequence), element)

        /// <summary>
        /// Tests whether the specified sequence does not contain the specified element
        /// and raises an exception if the element is in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="element">The element that is expected not to be in the sequence.</param>
        /// <param name="sequence">The sequence in which to search for the element.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="element"/> is found in <paramref name="sequence"/>.</exception>
        [<CompiledName("NotContains")>]
        let notContains<'T> (element: 'T) sequence =
            CollectionAssert.DoesNotContain(SeqWrapper<'T>(sequence), element)

        /// <summary>
        /// Tests whether one sequence is a subset of another sequence
        /// and raises an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="superset">The sequence expected
        /// to be a superset of <paramref name="sequence"/>.</param>
        /// <param name="sequence">The sequence expected
        /// to be a subset of <paramref name="superset"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if an element in <paramref name="superset"/>
        /// is not found in <paramref name="superset"/>.</exception>
        [<CompiledName("SubsetOf")>]
        let subsetOf<'T> superset sequence =
            CollectionAssert.IsSubsetOf(SeqWrapper<'T>(sequence), SeqWrapper<'T>(superset))

        /// <summary>
        /// Tests whether one sequence is not a subset of another sequence
        /// and raises an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="superset">The sequence expected
        /// not to be a superset of <paramref name="sequence"/>.</param>
        /// <param name="sequence">The sequence expected
        /// not to be a subset of <paramref name="superset"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if every element in <paramref name="sequence"/>
        /// is also found in <paramref name="superset"/>.</exception>
        [<CompiledName("NotSubsetOf")>]
        let notSubsetOf<'T> superset sequence =
            CollectionAssert.IsNotSubsetOf(SeqWrapper<'T>(sequence), SeqWrapper<'T>(superset))

        /// <summary>
        /// Tests whether one sequence is a superset of another sequence
        /// and raises an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="subset">The sequence expected
        /// to be a subset of <paramref name="sequence"/>.</param>
        /// <param name="sequence">The sequence expected
        /// to be a superset of <paramref name="subset"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if an element in <paramref name="subset"/>
        /// is not found in <paramref name="sequence"/>.</exception>
        [<CompiledName("SupersetOf")>]
        let supersetOf<'T> subset sequence =
            CollectionAssert.IsSubsetOf(SeqWrapper<'T>(subset), SeqWrapper<'T>(sequence))

        /// <summary>
        /// Tests whether one sequence is not a subset of another sequence
        /// and raises an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence expected
        /// not to be a superset of <paramref name="subset"/>.</param>
        /// <param name="subset">The sequence expected
        /// not to be a subset of <paramref name="sequence"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if every element in <paramref name="subset"/>
        /// is also found in <paramref name="sequence"/>.</exception>
        [<CompiledName("NotSupersetOf")>]
        let notSupersetOf<'T> subset sequence =
            CollectionAssert.IsNotSubsetOf(SeqWrapper<'T>(subset), SeqWrapper<'T>(sequence))

    /// <summary>
    /// A collection of functions to test various conditions associated with strings within unit tests.
    /// If the condition being tested is not met, an exception is raised.
    /// </summary>
    [<RequireQualifiedAccess>]
    module String =

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and raises an exception if the substring does not occur within the test string.
        /// </summary>
        /// <param name="substring">The string expected to occur within <paramref name="value"/>.</param>
        /// <param name="value">The string expected to contain <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="substring"/> is not found in <paramref name="value"/>.</exception>
        [<CompiledName("Contains")>]
        let contains substring value =
            StringAssert.Contains(value, substring)

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and raises an exception if the substring does not occur within the test string.
        /// </summary>
        /// <param name="option">The comparison method to compare strings.</param>
        /// <param name="substring">The string expected to occur within <paramref name="value"/>.</param>
        /// <param name="value">The string expected to contain <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="substring"/> is not found in <paramref name="value"/>.</exception>
        [<CompiledName("ContainsWithOption")>]
        let containsOpt (option: StringComparison) substring value =
            StringAssert.Contains(value, substring, option)

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and raises an exception if the test string does not start with the substring.
        /// </summary>
        /// <param name="substring">The string expected to be a prefix of <paramref name="value"/>.</param>
        /// <param name="value">The string expected to begin with <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not begin with <paramref name="substring"/>.</exception>
        [<CompiledName("StartsWith")>]
        let startsWith substring value =
            StringAssert.StartsWith(value, substring)

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and raises an exception if the test string does not start with the substring.
        /// </summary>
        /// <param name="option">The comparison method to compare strings.</param>
        /// <param name="substring">The string expected to be a prefix of <paramref name="value"/>.</param>
        /// <param name="value">The string expected to begin with <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not begin with <paramref name="substring"/>.</exception>
        [<CompiledName("StartsWithOption")>]
        let startsWithOpt (option: StringComparison) substring value =
            StringAssert.StartsWith(value, substring, option)

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and raises an exception if the test string does not end with the substring.
        /// </summary>
        /// <param name="substring">The string expected to be a suffix of <paramref name="value"/>.</param>
        /// <param name="value">The string expected to end with <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not end with <paramref name="substring"/>.</exception>
        [<CompiledName("EndsWith")>]
        let endsWith substring value =
            StringAssert.EndsWith(value, substring)

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and raises an exception if the test string does not end with the substring.
        /// </summary>
        /// <param name="option">The comparison method to compare strings.</param>
        /// <param name="substring">The string expected to be a suffix of <paramref name="value"/>.</param>
        /// <param name="value">The string expected to end with <paramref name="substring"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not end with <paramref name="substring"/>.</exception>
        [<CompiledName("EndsWithOption")>]
        let endsWithOpt (option: StringComparison) substring value =
            StringAssert.EndsWith(value, substring, option)

        /// <summary>
        /// Tests whether the specified string matches a regular expression
        /// and raises an exception if the string does not match the expression.
        /// </summary>
        /// <param name="pattern">The regular expression
        /// that <paramref name="value"/> is expected to match.</param>
        /// <param name="value">The string expected to match <paramref name="pattern"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not match <paramref name="pattern"/>.</exception>
        [<CompiledName("Match")>]
        let match' pattern value =
            StringAssert.Matches(value, Regex(pattern))

        /// <summary>
        /// Tests whether the specified string matches a regular expression
        /// and raises an exception if the string does not match the expression.
        /// </summary>
        /// <param name="options">A bitwise combination of the enumeration values
        /// that modify the regular expression specified by <paramref name="pattern"/>.</param>
        /// <param name="pattern">The regular expression
        /// that <paramref name="value"/> is expected to match.</param>
        /// <param name="value">The string expected to match <paramref name="pattern"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> does not match <paramref name="pattern"/>.</exception>
        [<CompiledName("MatchWithOptions")>]
        let matchOpt (options: RegexOptions) pattern value =
            StringAssert.Matches(value, Regex(pattern, options))

        /// <summary>
        /// Tests whether the specified string does not match a regular expression
        /// and raises an exception if the string matches the expression.
        /// </summary>
        /// <param name="pattern">The regular expression
        /// that <paramref name="value"/> is expected to not match.</param>
        /// <param name="value">The string expected not to match <paramref name="pattern"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> matches <paramref name="pattern"/>.</exception>
        [<CompiledName("NotMatch")>]
        let notMatch pattern value =
            StringAssert.DoesNotMatch(value, Regex(pattern))

        /// <summary>
        /// Tests whether the specified string does not match a regular expression
        /// and raises an exception if the string matches the expression.
        /// </summary>
        /// <param name="options">A bitwise combination of the enumeration values
        /// that modify the regular expression specified by <paramref name="pattern"/>.</param>
        /// <param name="pattern">The regular expression
        /// that <paramref name="value"/> is expected to not match.</param>
        /// <param name="value">The string expected not to match <paramref name="pattern"/>.</param>
        /// <exception cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException">
        /// Raised if <paramref name="value"/> matches <paramref name="pattern"/>.</exception>
        [<CompiledName("NotMatchWithOptions")>]
        let notMatchOpt (options: RegexOptions) pattern value =
            StringAssert.DoesNotMatch(value, Regex(pattern, options))
