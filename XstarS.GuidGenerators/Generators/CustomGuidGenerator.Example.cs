#if !UUIDREV_DISABLE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

partial class CustomGuidGenerator
{
    internal sealed class Example : CustomGuidGenerator, IGuidGenerator
    {
        private static volatile CustomGuidGenerator.Example? Singleton;

        private Example() : base(TimestampEpochs.UnixTime) { }

        internal static CustomGuidGenerator.Example Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static CustomGuidGenerator.Example Initialize()
                {
                    return CustomGuidGenerator.Example.Singleton ??=
                        new CustomGuidGenerator.Example();
                }

                return CustomGuidGenerator.Example.Singleton ?? Initialize();
            }
        }

        public override Guid NewGuid()
        {
            var guid = Guid.NewGuid();
            this.FillTimestampFields(ref guid);
            this.FillVersionField(ref guid);
            Debug.Assert(guid.GetVariant() == this.Variant);
            return guid;
        }

        private void FillTimestampFields(ref Guid guid)
        {
            var timestamp = this.GetCurrentTimestamp();
            var ts10NanoSec = timestamp * 10;
            guid.TimeLow() = (uint)(ts10NanoSec >> (4 * 8 - 4));
            guid.TimeMid() = (ushort)(ts10NanoSec >> (2 * 8 - 4));
            var timeHi = (ushort)((ts10NanoSec >> (0 * 8)) & ~0xF000);
            ref var timeHi_Ver = ref guid.TimeHi_Ver();
            timeHi_Ver = (ushort)(timeHi_Ver & 0xF000 | timeHi);
        }
    }
}
#endif
