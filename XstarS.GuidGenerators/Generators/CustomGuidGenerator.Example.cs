#if !FEATURE_DISABLE_UUIDREV
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
            var tsNanoSec = timestamp * 100;
            guid.TimeLow() = (uint)(tsNanoSec >> (2 * 8));
            guid.TimeMid() = (ushort)(tsNanoSec >> (0 * 8));
        }
    }
}
#endif
