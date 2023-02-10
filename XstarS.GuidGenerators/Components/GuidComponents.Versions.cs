using System;

namespace XNetEx.Guids.Components;

partial class GuidComponents
{
    private static readonly GuidComponents Instance = new GuidComponents();

    public static GuidComponents Common => GuidComponents.Instance;

    public static ICommonGuidComponents Empty => GuidComponents.Instance;

    public static ITimeNodeBasedGuidComponents Version1 => TimeNodeBasedGuidComponents.Version1.Instance;

    public static IDceSecurityGuidComponents Version2 => DceSecurityGuidComponents.Version2.Instance;

    public static ICommonGuidComponents Version3 => GuidComponents.Instance;

    public static ICommonGuidComponents Version4 => GuidComponents.Instance;

    public static ICommonGuidComponents Version5 => GuidComponents.Instance;

    public static ITimeNodeBasedGuidComponents Version6 => TimeNodeBasedGuidComponents.Version6.Instance;

    public static ITimeBasedGuidComponents Version7 => TimeBasedGuidComponents.Version7.Instance;

    public static ICommonGuidComponents Version8 => GuidComponents.Instance;

    public static ICommonGuidComponents MaxValue => GuidComponents.Instance;

    public static GuidComponents OfVersion(GuidVersion version) => version switch
    {
        GuidVersion.Empty => GuidComponents.Instance,
        GuidVersion.Version1 => TimeNodeBasedGuidComponents.Version1.Instance,
        GuidVersion.Version2 => DceSecurityGuidComponents.Version2.Instance,
        GuidVersion.Version3 => GuidComponents.Instance,
        GuidVersion.Version4 => GuidComponents.Instance,
        GuidVersion.Version5 => GuidComponents.Instance,
        GuidVersion.Version6 => TimeNodeBasedGuidComponents.Version6.Instance,
        GuidVersion.Version7 => TimeBasedGuidComponents.Version7.Instance,
        GuidVersion.Version8 => GuidComponents.Instance,
        GuidVersion.MaxValue => GuidComponents.Instance,
        _ => throw new ArgumentOutOfRangeException(nameof(version)),
    };
}
