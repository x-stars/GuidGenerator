using System;

namespace XNetEx.Guids;

/// <summary>
/// Represents the variant of a <see cref="Guid"/>.
/// </summary>
public enum GuidVariant : byte
{
    /// <summary>
    /// Reserved for NCS UUID backward compatibility.
    /// </summary>
    Ncs = 0,
#if !UUIDREV_DISABLE
    /// <summary>
    /// Represents the variant specified in RFC 4122 and RFC 9562.
    /// </summary>
#else
    /// <summary>
    /// Represents the variant specified in RFC 4122.
    /// </summary>
#endif
    Rfc4122 = 1,
    /// <summary>
    /// Represents the variant of Microsoft legacy GUIDs.
    /// </summary>
    Microsoft = 2,
    /// <summary>
    /// Reserved for future definition.
    /// </summary>
    Reserved = 3,
}
