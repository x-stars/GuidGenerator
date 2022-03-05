using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal sealed class DceSecurityGuidGenerator : GuidGenerator
    {
        private static class Singleton
        {
            internal static readonly DceSecurityGuidGenerator Value =
                new DceSecurityGuidGenerator();
        }

        internal DceSecurityGuidGenerator() { }

        internal static DceSecurityGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => DceSecurityGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version2;

        public override Guid NewGuid()
        {
            throw new NotImplementedException();
        }
    }
}
