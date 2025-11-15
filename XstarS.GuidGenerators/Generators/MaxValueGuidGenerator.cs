#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class MaxValueGuidGenerator : GuidGenerator, IGuidGenerator
{
    private MaxValueGuidGenerator() { }

    internal static MaxValueGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static MaxValueGuidGenerator Initialize()
            {
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new MaxValueGuidGenerator());
            }

            return Volatile.Read(ref field) ?? Initialize();
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
