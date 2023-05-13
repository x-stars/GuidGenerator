#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class MaxValueGuidGenerator : GuidGenerator, IGuidGenerator
{
    private static volatile MaxValueGuidGenerator? Singleton;

    private MaxValueGuidGenerator() { }

    internal static MaxValueGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static MaxValueGuidGenerator Initialize()
            {
                return MaxValueGuidGenerator.Singleton ??=
                    new MaxValueGuidGenerator();
            }

            return MaxValueGuidGenerator.Singleton ?? Initialize();
        }
    }

    public override GuidVersion Version => GuidVersion.MaxValue;

    public override GuidVariant Variant => GuidVariant.Reserved;

    public override Guid NewGuid()
    {
        var guid = Uuid.MaxValue;
        Debug.Assert(guid.GetVersion() == this.Version);
        Debug.Assert(guid.GetVariant() == this.Variant);
        return guid;
    }
}
#endif
