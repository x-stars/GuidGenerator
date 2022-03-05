using System;

namespace XstarS.GuidGenerators
{
    internal sealed class RandomizedGuidGenerator : GuidGenerator
    {
        internal RandomizedGuidGenerator() { }

        public override GuidVersion Version => GuidVersion.Version4;

        public override Guid NewGuid() => Guid.NewGuid();
    }
}
