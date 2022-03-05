using System;
using System.Security.Cryptography;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class NameBasedGuidGenerator : GuidGenerator
    {
        protected NameBasedGuidGenerator() { }

        public sealed override Guid NewGuid()
        {
            return this.NewGuid(Guid.Empty, string.Empty);
        }

        public sealed override Guid NewGuid(Guid ns, string name)
        {
            var nsBytes = ns.ToByteArray();
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var input = new byte[nsBytes.Length + nameBytes.Length];
            Array.Copy(nsBytes, 0, input, 0, nsBytes.Length);
            Array.Copy(nameBytes, 0, input, nsBytes.Length, nameBytes.Length);
            var hashBytes = this.ComputeHash(input);
            var guidBytes = this.ProcessHashBytes(hashBytes);
            return new Guid(guidBytes);
        }

        protected abstract byte[] ComputeHash(byte[] input);

        private byte[] ProcessHashBytes(byte[] hashBytes)
        {
            throw new NotImplementedException();
        }

        internal sealed class MD5Hashing : NameBasedGuidGenerator
        {
            private readonly HashAlgorithm Hashing;

            internal MD5Hashing() { this.Hashing = MD5.Create(); }

            public override GuidVersion Version => GuidVersion.Version3;

            protected override byte[] ComputeHash(byte[] input)
            {
                return this.Hashing.ComputeHash(input);
            }
        }

        internal sealed class SHA1Hashing : NameBasedGuidGenerator
        {
            private readonly HashAlgorithm Hashing;

            internal SHA1Hashing() { this.Hashing = SHA1.Create(); }

            public override GuidVersion Version => GuidVersion.Version5;

            protected override byte[] ComputeHash(byte[] input)
            {
                return this.Hashing.ComputeHash(input);
            }
        }
    }
}
