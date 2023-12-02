namespace XNetEx.Guids.Components;

internal interface IGuidCommonComponents
    : IGuidVariantComponent, IGuidVersionComponent, IGuidRawDataComponent
{
}

internal interface ITimeBasedGuidComponents
    : IGuidCommonComponents, IGuidTimestampComponent
{
}

internal interface ITimeNodeBasedGuidComponents
    : ITimeBasedGuidComponents, IGuidClockSequenceComponent, IGuidNodeIdComponent
{
}

internal interface IDceSecurityGuidComponents
    : ITimeNodeBasedGuidComponents, IGuidDomainComponent, IGuidLocalIdComponent
{
}

internal interface INameBasedGuidComponents
    : IGuidCommonComponents, IGuidHashDataComponent
{
}

internal interface IRandomizedGuidComponents
    : IGuidCommonComponents, IGuidRandomDataComponent
{
}

internal interface ITimeRandomBasedGuidComponents
    : ITimeBasedGuidComponents, IRandomizedGuidComponents
{
}

#if !UUIDREV_DISABLE
internal interface ICustomizedGuidComponents
    : IGuidCommonComponents, IGuidCustomDataComponent
{
}
#endif
