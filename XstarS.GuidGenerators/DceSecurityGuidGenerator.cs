using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal sealed class DceSecurityGuidGenerator : TimeBasedGuidGenerator, IDceSecurityGuidGenerator
    {
        private static class Singleton
        {
            internal static readonly DceSecurityGuidGenerator Value =
                new DceSecurityGuidGenerator();
        }

        private readonly Lazy<int> LazyLocalUserId;

        private readonly Lazy<int> LazyLocalGroupId;

        private DceSecurityGuidGenerator()
        {
            var provider = LocalIdProvider.Instance;
            this.LazyLocalUserId = new Lazy<int>(provider.GetLocalUserId);
            this.LazyLocalGroupId = new Lazy<int>(provider.GetLocalGroupId);
        }

        internal static new DceSecurityGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => DceSecurityGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version2;

        protected override int TimestampShift => 32;

        private int LocalUserId => this.LazyLocalUserId.Value;

        private int LocalGroupId => this.LazyLocalGroupId.Value;

        public override Guid NewGuid()
        {
            return this.NewGuid(DceSecurityDomain.Person);
        }

        public override Guid NewGuid(DceSecurityDomain domain, int? localId = null)
        {
            var guid = base.NewGuid();
            var iLocalId = this.GetLocalId(domain, localId);
            guid.TimeLow() = (uint)iLocalId;
            guid.ClkSeqHi_Var() = guid.ClkSeqLow();
            this.FillVariantField(ref guid);
            guid.ClkSeqLow() = (byte)domain;
            return guid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLocalId(DceSecurityDomain domain, int? localId) => domain switch
        {
            DceSecurityDomain.Person => localId ?? this.LocalUserId,
            DceSecurityDomain.Group => localId ?? this.LocalGroupId,
            DceSecurityDomain.Org => localId ?? default(int),
            _ => throw new ArgumentOutOfRangeException(nameof(domain))
        };
    }
}
