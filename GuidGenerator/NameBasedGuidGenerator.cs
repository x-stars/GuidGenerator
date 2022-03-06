﻿using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
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
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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
            this.FillVersionField(guidBytes);
            this.FillVariantField(guidBytes);
            return guidBytes;
        }

        internal sealed class MD5Hashing : NameBasedGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly NameBasedGuidGenerator.MD5Hashing Value =
                    new NameBasedGuidGenerator.MD5Hashing();
            }

            private MD5Hashing() { }

            internal static NameBasedGuidGenerator.MD5Hashing Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => NameBasedGuidGenerator.MD5Hashing.Singleton.Value;
            }

            public override GuidVersion Version => GuidVersion.Version3;

            protected override HashAlgorithm CreateHashing() => MD5.Create();
        }

        internal sealed class SHA1Hashing : NameBasedGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly NameBasedGuidGenerator.SHA1Hashing Value =
                    new NameBasedGuidGenerator.SHA1Hashing();
            }

            private SHA1Hashing() { }

            internal static NameBasedGuidGenerator.SHA1Hashing Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => NameBasedGuidGenerator.SHA1Hashing.Singleton.Value;
            }

            public override GuidVersion Version => GuidVersion.Version5;

            protected override HashAlgorithm CreateHashing() => SHA1.Create();
        }
    }
}
