using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal sealed class EmptyGuidGenerator : GuidGenerator
    {
        private static class Singleton
        {
            internal static readonly EmptyGuidGenerator Value =
                new EmptyGuidGenerator();
        }

        private EmptyGuidGenerator() { }

        internal static EmptyGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => EmptyGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Empty;

        public override Guid NewGuid() => Guid.Empty;
    }
}
