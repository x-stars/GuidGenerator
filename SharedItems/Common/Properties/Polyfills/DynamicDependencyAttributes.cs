﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable
#nullable enable

#if !(DYNAMIC_DEPENDENCY_ATTRIBUTES_EXTERNAL || NET5_0_OR_GREATER)
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies the types of members that are dynamically accessed.
    /// This enumeration has a <see cref="FlagsAttribute"/> attribute
    /// that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    internal enum DynamicallyAccessedMemberTypes
    {
        /// <summary>
        /// Specifies no members.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the default, parameterless public constructor.
        /// </summary>
        PublicParameterlessConstructor = 0x0001,
        /// <summary>
        /// Specifies all public constructors.
        /// </summary>
        PublicConstructors = 0x0002 | PublicParameterlessConstructor,
        /// <summary>
        /// Specifies all non-public constructors.
        /// </summary>
        NonPublicConstructors = 0x0004,
        /// <summary>
        /// Specifies all public methods.
        /// </summary>
        PublicMethods = 0x0008,
        /// <summary>
        /// Specifies all non-public methods.
        /// </summary>
        NonPublicMethods = 0x0010,
        /// <summary>
        /// Specifies all public fields.
        /// </summary>
        PublicFields = 0x0020,
        /// <summary>
        /// Specifies all non-public fields.
        /// </summary>
        NonPublicFields = 0x0040,
        /// <summary>
        /// Specifies all public nested types.
        /// </summary>
        PublicNestedTypes = 0x0080,
        /// <summary>
        /// Specifies all non-public nested types.
        /// </summary>
        NonPublicNestedTypes = 0x0100,
        /// <summary>
        /// Specifies all public properties.
        /// </summary>
        PublicProperties = 0x0200,
        /// <summary>
        /// Specifies all non-public properties.
        /// </summary>
        NonPublicProperties = 0x0400,
        /// <summary>
        /// Specifies all public events.
        /// </summary>
        PublicEvents = 0x0800,
        /// <summary>
        /// Specifies all non-public events.
        /// </summary>
        NonPublicEvents = 0x1000,
        /// <summary>
        /// Specifies all interfaces implemented by the type.
        /// </summary>
        Interfaces = 0x2000,
        /// <summary>
        /// Specifies all members.
        /// </summary>
        All = ~None,
    }

    /// <summary>
    /// Indicates that certain members on a specified <see cref="Type"/> are accessed dynamically,
    /// for example through <see cref="Reflection"/>.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Field | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter |
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Method |
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct,
        Inherited = false)]
    internal sealed class DynamicallyAccessedMembersAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicallyAccessedMembersAttribute"/> class
        /// with the specified member types.
        /// </summary>
        /// <param name="memberTypes">The types of members dynamically accessed.</param>
        public DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes)
        {
            this.MemberTypes = memberTypes;
        }

        /// <summary>
        /// Gets the <see cref="DynamicallyAccessedMemberTypes"/> which specifies the type
        /// of members dynamically accessed.
        /// </summary>
        public DynamicallyAccessedMemberTypes MemberTypes { get; }
    }

    /// <summary>
    /// States a dependency that one member has on another.
    /// </summary>
    [DebuggerNonUserCode, ExcludeFromCodeCoverage]
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method,
        AllowMultiple = true, Inherited = false)]
    internal sealed class DynamicDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDependencyAttribute"/> class
        /// with the specified signature of a member on the same type as the consumer.
        /// </summary>
        /// <param name="memberSignature">The signature of the member depended on.</param>
        public DynamicDependencyAttribute(string memberSignature)
        {
            this.MemberSignature = memberSignature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDependencyAttribute"/> class
        /// with the specified signature of a member on a <see cref="System.Type"/>.
        /// </summary>
        /// <param name="memberSignature">The signature of the member depended on.</param>
        /// <param name="type">The <see cref="System.Type"/> containing <paramref name="memberSignature"/>.</param>
        public DynamicDependencyAttribute(string memberSignature, Type type)
        {
            this.MemberSignature = memberSignature;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDependencyAttribute"/> class
        /// with the specified signature of a member on a type in an assembly.
        /// </summary>
        /// <param name="memberSignature">The signature of the member depended on.</param>
        /// <param name="typeName">The full name of the type containing the specified member.</param>
        /// <param name="assemblyName">The assembly name of the type containing the specified member.</param>
        public DynamicDependencyAttribute(string memberSignature, string typeName, string assemblyName)
        {
            this.MemberSignature = memberSignature;
            this.TypeName = typeName;
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDependencyAttribute"/> class
        /// with the specified types of members on a <see cref="System.Type"/>.
        /// </summary>
        /// <param name="memberTypes">The types of members depended on.</param>
        /// <param name="type">The <see cref="System.Type"/> containing the specified members.</param>
        public DynamicDependencyAttribute(DynamicallyAccessedMemberTypes memberTypes, Type type)
        {
            this.MemberTypes = memberTypes;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicDependencyAttribute"/> class
        /// with the specified types of members on a type in an assembly.
        /// </summary>
        /// <param name="memberTypes">The types of members depended on.</param>
        /// <param name="typeName">The full name of the type containing the specified members.</param>
        /// <param name="assemblyName">The assembly name of the type containing the specified members.</param>
        public DynamicDependencyAttribute(
            DynamicallyAccessedMemberTypes memberTypes, string typeName, string assemblyName)
        {
            this.MemberTypes = memberTypes;
            this.TypeName = typeName;
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        /// Gets the signature of the member depended on.
        /// </summary>
        public string? MemberSignature { get; }

        /// <summary>
        /// Gets the types of the members that are depended on, for example, fields and properties.
        /// </summary>
        public DynamicallyAccessedMemberTypes MemberTypes { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> containing the specified member.
        /// </summary>
        public Type? Type { get; }

        /// <summary>
        /// Gets the full name of the type containing the specified member.
        /// </summary>
        public string? TypeName { get; }

        /// <summary>
        /// Gets the assembly name of the specified type.
        /// </summary>
        public string? AssemblyName { get; }

        /// <summary>
        /// Gets or sets the condition in which the dependency is applicable.
        /// </summary>
        public string? Condition { get; set; }
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
