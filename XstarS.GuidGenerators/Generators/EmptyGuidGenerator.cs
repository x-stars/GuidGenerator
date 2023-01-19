using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class EmptyGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile EmptyGuidGenerator? Singleton;

    private EmptyGuidGenerator() { }

    internal static EmptyGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static EmptyGuidGenerator Initialize()
            {
                return EmptyGuidGenerator.Singleton ??=
                    new EmptyGuidGenerator();
            }

            return EmptyGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Empty;

    public override GuidVariant Variant => GuidVariant.Ncs;

    public override Guid NewGuid()
    {
        var guid = Guid.Empty;
        Debug.Assert(guid.GetVersion() == this.Version);
        Debug.Assert(guid.GetVariant() == this.Variant);
        return guid;
    }
}
