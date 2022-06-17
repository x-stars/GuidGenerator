using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
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

        public override Guid NewGuid() => Guid.Empty;
    }
}
