namespace XNetEx.Guids.Components;

partial class GuidComponents
{
    private static readonly GuidComponents Instance = new GuidComponents();

    public static GuidComponents Common => GuidComponents.Instance;

    public static GuidFixedFormatComponents FixedFormat => GuidFixedFormatComponents.Instance;

    public static IGuidCommonComponents Empty => GuidComponents.Instance;

    public static ITimeNodeBasedGuidComponents Version1 => TimeNodeBasedGuidComponents.Version1.Instance;

    public static IDceSecurityGuidComponents Version2 => DceSecurityGuidComponents.Version2.Instance;

    public static IGuidCommonComponents Version3 => GuidComponents.Instance;

    public static IGuidCommonComponents Version4 => GuidComponents.Instance;

    public static IGuidCommonComponents Version5 => GuidComponents.Instance;

    public static ITimeNodeBasedGuidComponents Version6 => TimeNodeBasedGuidComponents.Version6.Instance;

    public static ITimeBasedGuidComponents Version7 => TimeBasedGuidComponents.Version7.Instance;

    public static IGuidCommonComponents Version8 => GuidComponents.Instance;

    public static IGuidCommonComponents MaxValue => GuidComponents.Instance;

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
        GuidVersion.MaxValue or _ => GuidComponents.Instance,
    };
}
