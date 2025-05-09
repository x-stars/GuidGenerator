﻿using System;

namespace XNetEx.Guids.Generators;

#if !UUIDREV_DISABLE
using ITimeBasedGuidGenerator = IBlockingGuidGenerator;
#else
using ITimeBasedGuidGenerator = IGuidGenerator;
#endif

partial class GuidGenerator
{
    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of <see cref="Guid.Empty"/>.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of <see cref="Guid.Empty"/>.</returns>
    public static IGuidGenerator Empty => EmptyGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1.
    /// </summary>
    /// <returns>The <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1.</returns>
    public static ITimeBasedGuidGenerator Version1 => TimeBasedGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IDceSecurityGuidGenerator"/> instance of RFC 4122 UUID version 2.
    /// </summary>
    /// <returns>The <see cref="IDceSecurityGuidGenerator"/> instance of RFC 4122 UUID version 2.</returns>
    public static IDceSecurityGuidGenerator Version2 => DceSecurityGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 3.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 3.</returns>
    public static INameBasedGuidGenerator Version3 => NameBasedGuidGenerator.MD5Hashing.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 4.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 4122 UUID version 4.</returns>
    public static IGuidGenerator Version4 => RandomGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 5.
    /// </summary>
    /// <returns>The <see cref="INameBasedGuidGenerator"/> instance of RFC 4122 UUID version 5.</returns>
    public static INameBasedGuidGenerator Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance;

#if !UUIDREV_DISABLE
    /// <summary>
    /// Gets the <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6.
    /// </summary>
    /// <returns>The <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6.</returns>
    public static ITimeBasedGuidGenerator Version6 => TimeBasedGuidGenerator.Sequential.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 9562 UUID version 7.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 9562 UUID version 7.</returns>
    public static IGuidGenerator Version7 => UnixTimeBasedGuidGenerator.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of RFC 9562 UUID version 8
    /// example implementation (RFC 9562 Appendix B.1).
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of RFC 9562 UUID version 8
    /// example implementation (RFC 9562 Appendix B.1).</returns>
    public static IGuidGenerator Version8 => CustomGuidGenerator.Example.Instance;

    /// <summary>
    /// Gets the <see cref="IGuidGenerator"/> instance of <see cref="Uuid.MaxValue"/>.
    /// </summary>
    /// <returns>The <see cref="IGuidGenerator"/> instance of <see cref="Uuid.MaxValue"/>.</returns>
    public static IGuidGenerator MaxValue => MaxValueGuidGenerator.Instance;
#endif

    /// <summary>
    /// Gets the <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>The <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a non-volatile random node ID.</returns>
    public static ITimeBasedGuidGenerator Version1R => TimeBasedGuidGenerator.InstanceR;

#if !UUIDREV_DISABLE
    /// <summary>
    /// Gets the <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a physical (IEEE 802 MAC) address node ID.
    /// </summary>
    /// <returns>The <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a physical (IEEE 802 MAC) address node ID.</returns>
    public static ITimeBasedGuidGenerator Version6P => TimeBasedGuidGenerator.Sequential.InstanceP;

    /// <summary>
    /// Gets the <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a non-volatile random node ID.
    /// </summary>
    /// <returns>The <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a non-volatile random node ID.</returns>
    public static ITimeBasedGuidGenerator Version6R => TimeBasedGuidGenerator.Sequential.InstanceR;
#endif

    /// <summary>
    /// Creates a new <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="ITimeBasedGuidGenerator"/> instance of RFC 4122 UUID version 1
    /// using a volatile random node ID.</returns>
    public static ITimeBasedGuidGenerator CreateVersion1R() => TimeBasedGuidGenerator.CreateInstanceR();

#if !UUIDREV_DISABLE
    /// <summary>
    /// Creates a new <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a volatile random node ID.
    /// </summary>
    /// <returns>A new <see cref="ITimeBasedGuidGenerator"/> instance of RFC 9562 UUID version 6
    /// using a volatile random node ID.</returns>
    public static ITimeBasedGuidGenerator CreateVersion6R() => TimeBasedGuidGenerator.Sequential.CreateInstanceR();
#endif

    /// <summary>
    /// Gets the <see cref="GuidGenerator"/> instance of the specified GUID version number.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    /// <returns>The <see cref="GuidGenerator"/> instance of <paramref name="version"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version"/> is not a valid GUID version number.</exception>
    public static GuidGenerator OfVersion(byte version) => GuidGenerator.OfVersion((GuidVersion)version);

    /// <summary>
    /// Gets the <see cref="GuidGenerator"/> instance of the specified <see cref="GuidVersion"/>.
    /// </summary>
    /// <param name="version">The version of the <see cref="Guid"/> to generate.</param>
    /// <returns>The <see cref="GuidGenerator"/> instance of <paramref name="version"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="version"/> is not a valid <see cref="GuidVersion"/> value.</exception>
    public static GuidGenerator OfVersion(GuidVersion version) => version switch
    {
        GuidVersion.Empty => EmptyGuidGenerator.Instance,
        GuidVersion.Version1 => TimeBasedGuidGenerator.Instance,
        GuidVersion.Version2 => DceSecurityGuidGenerator.Instance,
        GuidVersion.Version3 => NameBasedGuidGenerator.MD5Hashing.Instance,
        GuidVersion.Version4 => RandomGuidGenerator.Instance,
        GuidVersion.Version5 => NameBasedGuidGenerator.SHA1Hashing.Instance,
#if !UUIDREV_DISABLE
        GuidVersion.Version6 => TimeBasedGuidGenerator.Sequential.Instance,
        GuidVersion.Version7 => UnixTimeBasedGuidGenerator.Instance,
        GuidVersion.Version8 => CustomGuidGenerator.Example.Instance,
        GuidVersion.MaxValue => MaxValueGuidGenerator.Instance,
#endif
        _ => throw new ArgumentOutOfRangeException(nameof(version)),
    };
}
