using System;

namespace XstarS.GuidGenerators
{
    internal sealed class DceSecurityGuidGenerator : GuidGenerator
    {
        internal DceSecurityGuidGenerator() { }

        public override GuidVersion Version => GuidVersion.Version2;

        public override Guid NewGuid()
        {
            throw new NotImplementedException();
        }
    }
}
