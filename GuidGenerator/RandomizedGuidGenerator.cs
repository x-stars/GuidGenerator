using System;

namespace XstarS.GuidGenerators
{
    internal sealed class RandomizedGuidGenerator : GuidGenerator
    {
        private RandomizedGuidGenerator() { }

        internal static RandomizedGuidGenerator Instance { get; } =
            new RandomizedGuidGenerator();

        public override GuidVersion Version => GuidVersion.Version4;

        public override Guid NewGuid() => Guid.NewGuid();
    }
}
