using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class RandomGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile RandomGuidGenerator? Singleton;

    private RandomGuidGenerator() { }

    internal static RandomGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static RandomGuidGenerator Initialize()
            {
                return RandomGuidGenerator.Singleton ??=
                    new RandomGuidGenerator();
            }

            return RandomGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.Version4;

    public override Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        Debug.Assert(guid.GetVersion() == this.Version);
        Debug.Assert(guid.GetVariant() == this.Variant);
        return guid;
    }
}
