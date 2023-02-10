namespace XNetEx.Guids.Components;

internal interface ICommonGuidComponents
    : IGuidVariantComponent, IGuidVersionComponent
{
}

internal interface ITimeBasedGuidComponents
    : ICommonGuidComponents, IGuidTimestampComponent
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
