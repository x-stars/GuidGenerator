using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal sealed class RandomizedGuidGenerator : GuidGenerator, IGuidGenerator
    {
        private static volatile RandomizedGuidGenerator? Singleton;

        private RandomizedGuidGenerator() { }

        internal static RandomizedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static RandomizedGuidGenerator Initialize()
                {
                    return RandomizedGuidGenerator.Singleton ??=
                        new RandomizedGuidGenerator();
                }

                return RandomizedGuidGenerator.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version4;

        public override Guid NewGuid() => Guid.NewGuid();
    }
}
