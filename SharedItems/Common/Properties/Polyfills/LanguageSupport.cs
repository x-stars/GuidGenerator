// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Reserved to be used by the compiler for tracking metadata.
    /// This class should not be used by developers in source code.
    /// </summary>
    [CompilerGenerated, DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }

    /// <summary>
    /// Used to indicate to the compiler that a method should be called
    /// in its containing module's initializer.
    /// </summary>
    [CompilerGenerated, DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class ModuleInitializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInitializerAttribute"/> class.
        /// </summary>
        public ModuleInitializerAttribute()
        {
        }
    }

    /// <summary>
    /// Indicates to the compiler that the .locals init flag
    /// should not be set in nested method headers when emitting to metadata.
    /// </summary>
    [CompilerGenerated, DebuggerNonUserCode]
    [AttributeUsage(
        AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct |
        AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method |
        AttributeTargets.Property | AttributeTargets.Event, Inherited = false)]
    internal sealed class SkipLocalsInitAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkipLocalsInitAttribute"/> class.
        /// </summary>
        public SkipLocalsInitAttribute()
        {
        }
    }
}
#endif

#if !NETCOREAPP3_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    using System.Diagnostics;

    /// <summary>
    /// Allows capturing of the expressions passed to a method.
    /// </summary>
    [CompilerGenerated, DebuggerNonUserCode]
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
