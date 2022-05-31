using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal sealed class RandomizedGuidGenerator : GuidGenerator
    {
        private static class Singleton
        {
            internal static readonly RandomizedGuidGenerator Value =
                new RandomizedGuidGenerator();
        }

        private RandomizedGuidGenerator() { }

        internal static RandomizedGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => RandomizedGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version4;

        public override Guid NewGuid() => Guid.NewGuid();
    }
}
