namespace XNetEx.Guids;

/// <summary>
/// Represents domains used by DCE Security (<see cref="GuidVersion.Version2"/>) UUIDs.
/// </summary>
public enum DceSecurityDomain : byte
{
    /// <summary>
    /// Represents the DCE Security Person domain.
    /// </summary>
    Person = 0,
    /// <summary>
    /// Represents the DCE Security Group domain.
    /// </summary>
    Group = 1,
    /// <summary>
    /// Represents the DCE Security Org domain.
    /// </summary>
    Org = 2,
}
