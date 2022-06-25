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
    /// <summary>
    /// Reserved for Microsoft legacy GUID backward compatibility.
    /// </summary>
    Microsoft = 2,
    /// <summary>
    /// Reserved for future definition.
    /// </summary>
    Reserved = 3,
}
