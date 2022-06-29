// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
#pragma warning disable

#if !(CALLER_INFO_ATTRIBUTES || NET45_OR_GREATER || NETCOREAPP || NETSTANDARD)
namespace System.Runtime.CompilerServices
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Allows you to obtain the full path of the source file that contains the caller.
    /// This is the file path at the time of compile.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerFilePathAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerFilePathAttribute"/> class.
        /// </summary>
        public CallerFilePathAttribute()
        {
        }
    }

    /// <summary>
    /// Allows you to obtain the line number in the source file at which the method is called.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerLineNumberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerLineNumberAttribute"/> class.
        /// </summary>
        public CallerLineNumberAttribute()
        {
        }
    }

    /// <summary>
    /// Allows you to obtain the method or property name of the caller to the method.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerMemberNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerMemberNameAttribute"/> class.
        /// </summary>
        public CallerMemberNameAttribute()
        {
        }
    }
}
#endif

#if !(CALLER_INFO_ATTRIBUTES || NETCOREAPP3_0_OR_GREATER)
namespace System.Runtime.CompilerServices
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Allows capturing of the expressions passed to a method.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the targeted parameter.</param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the target parameter name of the CallerArgumentExpression.
        /// </summary>
        /// <returns>The name of the targeted parameter of the CallerArgumentExpression.</returns>
        public string ParameterName { get; }
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
