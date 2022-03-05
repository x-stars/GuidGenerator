using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class NameBasedGuidGenerator : GuidGenerator
    {
        private readonly int MaxHashingCount;

        private readonly ConcurrentBag<HashAlgorithm> Hasings;

        protected NameBasedGuidGenerator()
        {
            this.MaxHashingCount = Environment.ProcessorCount * 2;
            this.Hasings = new ConcurrentBag<HashAlgorithm>();
        }

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

        protected abstract HashAlgorithm CreateHashing();

        private byte[] ComputeHash(byte[] input)
        {
            var hashings = this.Hasings;
            if (!hashings.TryTake(out var hasing))
            {
                hasing = this.CreateHashing();
            }
            var hash = hasing.ComputeHash(input);
            var maxCount = this.MaxHashingCount;
            if (hashings.Count < maxCount)
            {
                hashings.Add(hasing);
            }
            return hash;
        }

        private byte[] GetGuidBytes(byte[] hashBytes)
        {
            var guidBytes = new byte[16];
            Array.Copy(hashBytes, 0, guidBytes, 0, 16);
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
            private MD5Hashing() { }

            internal static NameBasedGuidGenerator.MD5Hashing Instance { get; } =
                new NameBasedGuidGenerator.MD5Hashing();

            public override GuidVersion Version => GuidVersion.Version3;

            protected override HashAlgorithm CreateHashing() => MD5.Create();
        }

        internal sealed class SHA1Hashing : NameBasedGuidGenerator
        {
            private SHA1Hashing() { }

            internal static NameBasedGuidGenerator.SHA1Hashing Instance { get; } =
                new NameBasedGuidGenerator.SHA1Hashing();

            public override GuidVersion Version => GuidVersion.Version5;

            protected override HashAlgorithm CreateHashing() => SHA1.Create();
        }
    }
}
