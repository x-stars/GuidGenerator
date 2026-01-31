using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class RandomGuidGenerator : GuidGenerator, IGuidGenerator
{
    private RandomGuidGenerator() { }

    internal static RandomGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            static RandomGuidGenerator Initialize()
            {
                lock (GuidGenerator.InitSyncRoot)
                {
                    return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                        new RandomGuidGenerator());
                }
            }

            return Volatile.Read(ref field) ?? Initialize();
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
