namespace XNetEx.Guids.Components;

partial class GuidComponents
{
    private static readonly GuidComponents Instance = new();

    public static GuidComponents Common => GuidComponents.Instance;

    public static GuidFixedFormatComponents FixedFormat => GuidFixedFormatComponents.Instance;

    public static IGuidCommonComponents Empty => GuidComponents.Instance;

    public static ITimeNodeBasedGuidComponents Version1 => TimeNodeBasedGuidComponents.Version1.Instance;

    public static IDceSecurityGuidComponents Version2 => DceSecurityGuidComponents.Version2.Instance;

    public static INameBasedGuidComponents Version3 => NameBasedGuidComponents.Version3.Instance;

    public static IRandomizedGuidComponents Version4 => RandomizedGuidComponents.Version4.Instance;

    public static INameBasedGuidComponents Version5 => NameBasedGuidComponents.Version5.Instance;

#if !UUIDREV_DISABLE
    public static ITimeNodeBasedGuidComponents Version6 => TimeNodeBasedGuidComponents.Version6.Instance;

    public static ITimeRandomBasedGuidComponents Version7 => TimeRandomBasedGuidComponents.Version7.Instance;

    public static ICustomizedGuidComponents Version8 => CustomizedGuidComponents.Version8.Instance;

    public static IGuidCommonComponents MaxValue => GuidComponents.Instance;
#endif

    public static GuidComponents OfVersion(GuidVersion version) => version switch
    {
        GuidVersion.Empty => GuidComponents.Instance,
        GuidVersion.Version1 => TimeNodeBasedGuidComponents.Version1.Instance,
        GuidVersion.Version2 => DceSecurityGuidComponents.Version2.Instance,
        GuidVersion.Version3 => NameBasedGuidComponents.Version3.Instance,
        GuidVersion.Version4 => RandomizedGuidComponents.Version4.Instance,
        GuidVersion.Version5 => NameBasedGuidComponents.Version5.Instance,
#if !UUIDREV_DISABLE
        GuidVersion.Version6 => TimeNodeBasedGuidComponents.Version6.Instance,
        GuidVersion.Version7 => TimeRandomBasedGuidComponents.Version7.Instance,
        GuidVersion.Version8 => CustomizedGuidComponents.Version8.Instance,
        GuidVersion.MaxValue => GuidComponents.Instance,
#endif
        _ => GuidComponents.Instance,
    };
}
