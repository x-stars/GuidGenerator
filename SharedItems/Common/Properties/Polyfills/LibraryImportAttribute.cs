// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(LIBRARY_IMPORT_ATTRIBUTE || NET7_0_OR_GREATER)
namespace System.Runtime.InteropServices
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Indicates that a source generator should create a function for marshalling arguments
    /// instead of relying on the runtime to generate an equivalent marshalling function at run time.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal sealed class LibraryImportAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryImportAttribute"/>.
        /// </summary>
        /// <param name="libraryName">Name of the library containing the import.</param>
        public LibraryImportAttribute(string libraryName)
        {
            this.LibraryName = libraryName;
        }

        /// <summary>
        /// Gets the name of the library containing the import.
        /// </summary>
        public string LibraryName { get; }

        /// <summary>
        /// Gets or sets the name of the entry point to be called.
        /// </summary>
        public string? EntryPoint { get; set; }

        /// <summary>
        /// Gets or sets an object that specifies how to marshal string arguments to the method.
        /// </summary>
        public StringMarshalling StringMarshalling { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> used to control
        /// how string arguments to the method are marshalled.
        /// </summary>
        public Type? StringMarshallingCustomType { get; set; }

        /// <summary>
        /// Gets or sets a value that specifies whether the callee sets an error
        /// (<c>SetLastError</c> on Windows or <c>errno</c> on other platforms)
        /// before returning from the attributed method.
        /// </summary>
        public bool SetLastError { get; set; }
    }

    /// <summary>
    /// Specifies how strings should be marshalled for generated p/invokes
    /// </summary>
    internal enum StringMarshalling
    {
        /// <summary>
        /// Indicates a specific marshaller is supplied in
        /// <see cref="LibraryImportAttribute.StringMarshallingCustomType"/>.
        /// </summary>
        Custom = 0,
        /// <summary>
        /// Use the platform-provided UTF-8 marshaller.
        /// </summary>
        Utf8,
        /// <summary>
        /// Use the platform-provided UTF-16 marshaller.
        /// </summary>
        Utf16,
    }
}
#endif

#if !(EXCLUDE_FROM_CODE_COVERAGE_ATTRIBUTE || NETCOREAPP3_0_OR_GREATER)
#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    // Excludes the attributed code from code coverage information.
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
