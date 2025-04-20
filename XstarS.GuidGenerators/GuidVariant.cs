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
    /// <summary>
    /// Represents the variant specified in RFC 4122.
    /// </summary>
    Rfc4122 = 1,
#if !UUIDREV_DISABLE
    /// <summary>
    /// Represents the variant specified in RFC 9562.
    /// This is an alias for <see cref="GuidVariant.Rfc4122"/>.
    /// </summary>
    Rfc9562 = Rfc4122,
#endif
    /// <summary>
    /// Represents the variant specified in OSF DCE 1.1.
    /// This is an alias for <see cref="GuidVariant.Rfc4122"/>.
    /// </summary>
    OsfDce = Rfc4122,
    /// <summary>
    /// Represents the variant of Microsoft legacy GUIDs.
    /// </summary>
    Microsoft = 2,
    /// <summary>
    /// Reserved for future definition.
    /// </summary>
    Reserved = 3,
}
