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

        private readonly Lazy<int> LazyLocalUserID;

        private readonly Lazy<int> LazyLocalGroupID;

        private DceSecurityGuidGenerator()
        {
            var provider = LocalIDProvider.Instance;
            this.LazyLocalUserID = new Lazy<int>(provider.GetLocalUserID);
            this.LazyLocalGroupID = new Lazy<int>(provider.GetLocalGroupID);
        }

        internal static new DceSecurityGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => DceSecurityGuidGenerator.Singleton.Value;
        }

        public override GuidVersion Version => GuidVersion.Version2;

        private int LocalUserID => this.LazyLocalUserID.Value;

        private int LocalGroupID => this.LazyLocalGroupID.Value;

        public override Guid NewGuid()
        {
            return this.NewGuid(DceSecurityDomain.Person, null);
        }

        public override Guid NewGuid(DceSecurityDomain domain, int? localID = null)
        {
            var guid = base.NewGuid();
            var iLocalID = this.GetLocalID(domain, localID);
            guid.TimeLow() = (uint)iLocalID;
            guid.ClkSeqHi_Var() = guid.ClkSeqLow();
            this.FillVariantField(ref guid);
            guid.ClkSeqLow() = (byte)domain;
            return guid;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLocalID(DceSecurityDomain domain, int? localID) => domain switch
        {
            DceSecurityDomain.Person => localID ?? this.LocalUserID,
            DceSecurityDomain.Group => localID ?? this.LocalGroupID,
            DceSecurityDomain.Org => localID ?? default(int),
            _ => throw new ArgumentOutOfRangeException(nameof(domain))
        };
    }
}
