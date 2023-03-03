// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(REQUIRED_MEMBER_ATTRIBUTE || NET7_0_OR_GREATER)
namespace System.Runtime.CompilerServices
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Specifies that a type has required members or that a member is required.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field |
        AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class RequiredMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredMemberAttribute"/> class.
        /// </summary>
        public RequiredMemberAttribute()
        {
        }
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that this constructor sets all required members for the current type,
    /// and callers do not need to set any required members themselves.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    internal sealed class SetsRequiredMembersAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetsRequiredMembersAttribute"/> class.
        /// </summary>
        public SetsRequiredMembersAttribute()
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
