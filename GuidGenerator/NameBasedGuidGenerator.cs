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

            var input = this.CreateInput(ns, name);
            var hashBytes = this.ComputeHash(input);
            return this.HashBytesToGuid(hashBytes);
        }

        protected abstract HashAlgorithm CreateHashing();

        private unsafe byte[] CreateInput(Guid ns, string name)
        {
            var nameBytes = Encoding.UTF8.GetBytes(name);
            var input = new byte[16 + nameBytes.Length];
            fixed (byte* pInput = &input[0]) { ns.WriteUuidBytes(pInput); }
            Buffer.BlockCopy(nameBytes, 0, input, 16, nameBytes.Length);
            return input;
        }

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

        private unsafe Guid HashBytesToGuid(byte[] hashBytes)
        {
            var guid = default(Guid);
            fixed (byte* pHashBytes = &hashBytes[0])
            {
                var uuid = *(Guid*)pHashBytes;
                uuid.WriteUuidBytes((byte*)&guid);
            }
            this.FillVersionField(ref guid);
            this.FillVariantField(ref guid);
            return guid;
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
