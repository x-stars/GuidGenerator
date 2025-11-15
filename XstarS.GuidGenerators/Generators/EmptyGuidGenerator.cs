using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class EmptyGuidGenerator : GuidGenerator, IGuidGenerator
{
    private EmptyGuidGenerator() { }

    internal static EmptyGuidGenerator Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static EmptyGuidGenerator Initialize()
            {
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    new EmptyGuidGenerator());
            }

            return Volatile.Read(ref field) ?? Initialize();
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
