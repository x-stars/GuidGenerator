// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

#if !(NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that null is allowed as an input even if the corresponding type disallows it.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property,
        Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllowNullAttribute"/> class.
        /// </summary>
        public AllowNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that null is disallowed as an input even if the corresponding type allows it.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property,
        Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisallowNullAttribute"/> class.
        /// </summary>
        public DisallowNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that an output may be null even if the corresponding type disallows it.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.ReturnValue,
        Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaybeNullAttribute"/> class.
        /// </summary>
        public MaybeNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that an output will not be null even if the corresponding type allows it.
    /// Specifies that an input argument was not null when the call returns.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.ReturnValue,
        Inherited = false)]
    internal sealed class NotNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullAttribute"/> class.
        /// </summary>
        public NotNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref="ReturnValue"/>,
    /// the parameter may be null even if the corresponding type disallows it.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name="returnValue">The return value condition.
        /// If the method returns this value, the associated parameter may be null.</param>
        public MaybeNullWhenAttribute(bool returnValue)
        {
            this.ReturnValue = returnValue;
        }

        /// <summary>
        /// Gets the return value condition.
        /// </summary>
        /// <returns>The return value condition.
        /// If the method returns this value, the associated parameter may be null.</returns>
        public bool ReturnValue { get; }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref="ReturnValue"/>,
    /// the parameter will not be null even if the corresponding type allows it.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name="returnValue">The return value condition.
        /// If the method returns this value, the associated parameter will not be null.</param>
        public NotNullWhenAttribute(bool returnValue)
        {
            this.ReturnValue = returnValue;
        }

        /// <summary>
        /// Gets the return value condition.
        /// </summary>
        /// <returns>The return value condition.
        /// If the method returns this value, the associated parameter may be null.</returns>
        public bool ReturnValue { get; }
    }

    /// <summary>
    /// Specifies that the output will be non-null if the named parameter is non-null.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
        AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with the associated parameter name.
        /// </summary>
        /// <param name="parameterName">The associated parameter name.
        /// The output will be non-null if the argument to the parameter specified is non-null.</param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the associated parameter name.
        /// </summary>
        /// <returns>The associated parameter name.
        /// The output will be non-null if the argument to the parameter specified is non-null.</returns>
        public string ParameterName { get; }
    }

    /// <summary>
    /// Applied to a method that will never return under any circumstance.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotReturnAttribute"/> class.
        /// </summary>
        public DoesNotReturnAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that the method will not return
    /// if the associated Boolean parameter is passed the specified value.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with the specified parameter value.
        /// </summary>
        /// <param name="parameterValue">The condition parameter value.
        /// Code after the method will be considered unreachable by diagnostics
        /// if the argument to the associated parameter matches this value.</param>
        public DoesNotReturnIfAttribute(bool parameterValue)
        {
            this.ParameterValue = parameterValue;
        }

        /// <summary>
        /// Gets the condition parameter value.
        /// </summary>
        /// <returns>The condition parameter value.
        /// Code after the method will be considered unreachable by diagnostics
        /// if the argument to the associated parameter matches this value.</returns>
        public bool ParameterValue { get; }
    }
}
#endif

#if !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that the method or property will ensure
    /// that the listed field and property members have not-null values.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Property,
        Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with a field or property member.
        /// </summary>
        /// <param name="member">
        /// The field or property member that is promised to be not-null.</param>
        public MemberNotNullAttribute(string member)
        {
            this.Members = new[] { member };
        }

        /// <summary>
        /// Initializes the attribute with the list of field and property members.
        /// </summary>
        /// <param name="members">
        /// The list of field and property members that are promised to be not-null.</param>
        public MemberNotNullAttribute(params string[] members)
        {
            this.Members = members;
        }

        /// <summary>
        /// Gets field or property member names.
        /// </summary>
        /// <returns>
        /// The list of field and property members that are promised to be not-null.</returns>
        public string[] Members { get; }
    }

    /// <summary>
    /// Specifies that the method or property will ensure
    /// that the listed field and property members have not-null values
    /// when returning with the specified return value condition.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Property,
        Inherited = false, AllowMultiple = true)]
    internal sealed class MemberNotNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with
        /// the specified return value condition and a field or property member.
        /// </summary>
        /// <param name="returnValue">The return value condition.
        /// If the method returns this value, the associated parameter will not be null.</param>
        /// <param name="member">
        /// The field or property member that is promised to be not-null.</param>
        public MemberNotNullWhenAttribute(bool returnValue, string member)
        {
            this.ReturnValue = returnValue;
            this.Members = new[] { member };
        }

        /// <summary>
        /// Initializes the attribute with
        /// the specified return value condition and list of field and property members.
        /// </summary>
        /// <param name="returnValue">The return value condition.
        /// If the method returns this value, the associated parameter will not be null.</param>
        /// <param name="members">
        /// The list of field and property members that are promised to be not-null.</param>
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members)
        {
            this.ReturnValue = returnValue;
            this.Members = members;
        }

        /// <summary>
        /// Gets the return value condition.
        /// </summary>
        /// <returns>The return value condition.
        /// If the method returns this value, the associated parameter will not be null.</returns>
        public bool ReturnValue { get; }

        /// <summary>
        /// Gets field or property member names.
        /// </summary>
        /// <returns>
        /// The list of field and property members that are promised to be not-null.</returns>
        public string[] Members { get; }
    }
}
#endif

#if !(NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
#if !(EXCLUDE_FROM_CODE_COVERAGE || NETCOREAPP3_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    [SuppressMessage("Microsoft.Design",
                     "CA1018:MarkAttributesWithAttributeUsageAttribute")]
    internal sealed partial class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif
#endif
