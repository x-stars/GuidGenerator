using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class DceSecurityGuidGenerator : TimeBasedGuidGenerator, IDceSecurityGuidGenerator
{
    private static volatile DceSecurityGuidGenerator? Singleton;

    private readonly LocalIdProvider LocalIdProvider;

    private DceSecurityGuidGenerator()
    {
        this.LocalIdProvider = LocalIdProvider.Instance;
    }

    internal static new DceSecurityGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static DceSecurityGuidGenerator Initialize()
            {
                return DceSecurityGuidGenerator.Singleton ??=
                    new DceSecurityGuidGenerator();
            }

            return DceSecurityGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version2;

    protected override int TimestampShift => 32;

    private int LocalUserId => this.LocalIdProvider.LocalUserId;

    private int LocalGroupId => this.LocalIdProvider.LocalGroupId;

    public override Guid NewGuid()
    {
        return this.NewGuid(DceSecurityDomain.Person);
    }

    public override Guid NewGuid(DceSecurityDomain domain, int? localId = null)
    {
        var guid = base.NewGuid();
        var iLocalId = this.GetLocalId(domain, localId);
        guid.TimeLow() = (uint)iLocalId;
        guid.ClkSeqHi_Var() = guid.ClkSeqLow();
        this.FillVariantField(ref guid);
        guid.ClkSeqLow() = (byte)domain;
        return guid;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetLocalId(DceSecurityDomain domain, int? localId) => domain switch
    {
        DceSecurityDomain.Person => localId ?? this.LocalUserId,
        DceSecurityDomain.Group => localId ?? this.LocalGroupId,
        DceSecurityDomain.Org => localId ?? default(int),
        _ => throw new ArgumentOutOfRangeException(nameof(domain))
    };
}
