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
            var nsBytes = ns.ToUuidByteArray();
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var input = new byte[nsBytes.Length + nameBytes.Length];
            Array.Copy(nsBytes, 0, input, 0, nsBytes.Length);
            Array.Copy(nameBytes, 0, input, nsBytes.Length, nameBytes.Length);
            var hashBytes = this.ComputeHash(input);
            var guidBytes = this.GetGuidBytes(hashBytes);
            return new Guid(guidBytes);
        }

        protected abstract byte[] ComputeHash(byte[] input);

        private byte[] GetGuidBytes(byte[] hashBytes)
        {
            const int GuidLength = 16;
            var guidBytes = new byte[GuidLength];
            Array.Copy(hashBytes, 0, guidBytes, 0, GuidLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(guidBytes, 0, 4);
                Array.Reverse(guidBytes, 4, 2);
                Array.Reverse(guidBytes, 6, 2);
            }
            var version = (int)this.Version << 4;
            guidBytes[7] = (byte)(guidBytes[7] & ~0xF0 | version);
            guidBytes[8] = (byte)(guidBytes[8] & ~0xC0 | 0x80);
            return guidBytes;
        }

        internal sealed class MD5Hashing : NameBasedGuidGenerator
        {
            private readonly HashAlgorithm Hashing;

            private MD5Hashing() { this.Hashing = MD5.Create(); }

            internal static NameBasedGuidGenerator.MD5Hashing Instance { get; } =
                new NameBasedGuidGenerator.MD5Hashing();

            public override GuidVersion Version => GuidVersion.Version3;

            protected override byte[] ComputeHash(byte[] input)
            {
                return this.Hashing.ComputeHash(input);
            }
        }

        internal sealed class SHA1Hashing : NameBasedGuidGenerator
        {
            private readonly HashAlgorithm Hashing;

            private SHA1Hashing() { this.Hashing = SHA1.Create(); }

            internal static NameBasedGuidGenerator.SHA1Hashing Instance { get; } =
                new NameBasedGuidGenerator.SHA1Hashing();

            public override GuidVersion Version => GuidVersion.Version5;

            protected override byte[] ComputeHash(byte[] input)
            {
                return this.Hashing.ComputeHash(input);
            }
        }
    }
}
