﻿using System;
using System.Runtime.CompilerServices;
using XNetEx.Guids.Components;

namespace XNetEx.Guids.Generators;

/// <summary>
/// Provides methods to generate a <see cref="Guid"/>.
/// </summary>
public abstract partial class GuidGenerator : IGuidGenerator, IGuidGeneratorInfo
{
    /// <summary>
    /// Initialize a new instance of the <see cref="GuidGenerator"/> class.
    /// </summary>
    protected GuidGenerator() { }

    /// <summary>
    /// Gets the version of the <see cref="Guid"/> generated by this instance.
    /// </summary>
    /// <returns>The version of the <see cref="Guid"/> generated by this instance.</returns>
    public abstract GuidVersion Version { get; }

    /// <summary>
    /// Gets the variant of the <see cref="Guid"/> generated by this instance.
    /// </summary>
    /// <returns>The variant of the <see cref="Guid"/> generated by this instance.</returns>
    public virtual GuidVariant Variant => GuidVariant.Rfc4122;

    /// <summary>
    /// Gets a value that indicates whether the <see cref="Guid"/>
    /// generated by this instance depends on the input parameters.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="Guid"/>
    /// generated by this instance depends on the input parameters;
    /// otherwise, <see langword="false"/>.</returns>
    public virtual bool RequiresInput => this.Version.IsNameBased();

#if !FEATURE_DISABLE_UUIDREV
    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
#endif

    /// <summary>
    /// Generates a new <see cref="Guid"/> instance.
    /// </summary>
    /// <returns>A new <see cref="Guid"/> instance.</returns>
    /// <exception cref="ObjectDisposedException">
    /// This instance has already been disposed.</exception>
    public abstract Guid NewGuid();

    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name byte array used to generate the <see cref="Guid"/>.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// The provided hash algorithm's hash size is less than 128 bits.</exception>
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// This instance has already been disposed.</exception>
    public virtual Guid NewGuid(Guid nsId, byte[] name)
    {
        throw new NotSupportedException(
            "This instance does not support generating a name-based Guid.");
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified namespace ID and name.
    /// </summary>
    /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
    /// <param name="name">The name byte span used to generate the <see cref="Guid"/>.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="nsId"/> and <paramref name="name"/>.</returns>
    /// <exception cref="InvalidOperationException">The provided hash algorithm's
    /// implementation is incorrect or the hash size is less than 128 bits.</exception>
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a name-based <see cref="Guid"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// This instance has already been disposed.</exception>
    public virtual Guid NewGuid(Guid nsId, ReadOnlySpan<byte> name)
    {
        return this.NewGuid(nsId, name.ToArray());
    }
#endif

    /// <summary>
    /// Generates a new <see cref="Guid"/> instance based on the specified DCE Security domain and local ID.
    /// </summary>
    /// <param name="domain">The DCE Security domain used to generate the <see cref="Guid"/>.</param>
    /// <param name="localId">The site-defined local ID used to generate the <see cref="Guid"/>,
    /// or <see langword="null"/> to get the local user or group ID from the system.</param>
    /// <returns>A new <see cref="Guid"/> instance generated based on
    /// <paramref name="domain"/> and <paramref name="localId"/>.</returns>
    /// <exception cref="PlatformNotSupportedException">
    /// The current operating system does not support getting the local user or group ID.</exception>
    /// <exception cref="NotSupportedException">
    /// This instance does not support generating a DCE Security <see cref="Guid"/>.</exception>
    /// <exception cref="ObjectDisposedException">
    /// This instance has already been disposed.</exception>
    public virtual Guid NewGuid(DceSecurityDomain domain, int? localId = null)
    {
        throw new NotSupportedException(
            "This instance does not support generating a DCE Security Guid.");
    }

#if !FEATURE_DISABLE_UUIDREV
    /// <summary>
    /// Releases the unmanaged resources used by this instance
    /// and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release
    /// both managed and unmanaged resources; <see langword="false"/>
    /// to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing) { }
#endif

    /// <summary>
    /// Fills the version field of the specified <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to fill the version field.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void FillVersionField(ref Guid guid)
    {
        GuidComponents.Common.SetVersion(ref guid, this.Version);
    }

    /// <summary>
    /// Fills the variant field of the specified <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to fill the variant field.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void FillVariantField(ref Guid guid)
    {
        GuidComponents.Common.SetVariant(ref guid, this.Variant);
    }
}
