// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(SCOPED_REF_ATTRIBUTE_EXTERNAL || NET7_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Used to indicate a byref escapes and is not scoped.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter,
        AllowMultiple = false, Inherited = false)]
    internal sealed class UnscopedRefAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnscopedRefAttribute"/> class.
        /// </summary>
        public UnscopedRefAttribute()
        {
        }
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
