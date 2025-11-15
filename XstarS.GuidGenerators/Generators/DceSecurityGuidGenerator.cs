using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class DceSecurityGuidGenerator : TimeBasedGuidGenerator, IDceSecurityGuidGenerator
{
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
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new DceSecurityGuidGenerator());
            }

            return Volatile.Read(ref field) ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version2;

    private int LocalUserId => this.LocalIdProvider.LocalUserId;

    private int LocalGroupId => this.LocalIdProvider.LocalGroupId;

    public override Guid NewGuid()
    {
        return this.NewGuid(DceSecurityDomain.Person);
    }

    public override Guid NewGuid(DceSecurityDomain domain, int? localId = null)
    {
        var guid = base.NewGuid();
        var localIdValue = this.GetLocalId(domain, localId);
        var components = this.GuidComponents;
        components.SetDomain(ref guid, domain);
        components.SetLocalId(ref guid, localIdValue);
        return guid;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetLocalId(DceSecurityDomain domain, int? localId) => domain switch
    {
        DceSecurityDomain.Person => localId ?? this.LocalUserId,
        DceSecurityDomain.Group => localId ?? this.LocalGroupId,
        DceSecurityDomain.Org or _ => localId ?? default(int),
    };
}
