// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(COMPILER_FEATURE_REQUIRED_ATTRIBUTE_EXTERNAL || NET7_0_OR_GREATER)
namespace System.Runtime.CompilerServices
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Indicates that compiler support for a particular feature is required
    /// for the location where this attribute is applied.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    internal sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        /// <summary>
        /// Initializes a <see cref="CompilerFeatureRequiredAttribute"/> instance
        /// for the passed in compiler feature.
        /// </summary>
        /// <param name="featureName">The name of the required compiler feature.</param>
        public CompilerFeatureRequiredAttribute(string featureName)
        {
            this.FeatureName = featureName;
        }

        /// <summary>
        /// The name of the compiler feature.
        /// </summary>
        public string FeatureName { get; }

        /// <summary>
        /// Gets a value that indicates whether the compiler can choose
        /// to allow access to the location where this attribute is applied
        /// if it does not understand <see cref="FeatureName"/>.
        /// </summary>
        /// <returns><see langword="true"/> to let the compiler choose
        /// to allow access to the location where this attribute is applied
        /// if it does not understand <see cref="FeatureName"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public bool IsOptional { get; /*init*/set; }

        /// <summary>
        /// The <see cref="FeatureName"/> used for the ref structs C# feature.
        /// </summary>
        public const string RefStructs = nameof(RefStructs);

        /// <summary>
        /// The <see cref="FeatureName"/> used for the required members C# feature.
        /// </summary>
        public const string RequiredMembers = nameof(RequiredMembers);
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
