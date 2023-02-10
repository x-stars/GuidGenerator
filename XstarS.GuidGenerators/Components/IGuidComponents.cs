namespace XNetEx.Guids.Components;

internal interface IGuidCommonComponents
    : IGuidVariantComponent, IGuidVersionComponent
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
