using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace XstarS.GuidGenerators;

internal abstract class NameBasedGuidGenerator : GuidGenerator, INameBasedGuidGenerator
{
    private readonly BlockingCollection<HashAlgorithm> Hashings;

    private NameBasedGuidGenerator()
    {
        var concurrency = Environment.ProcessorCount * 2;
        this.Hashings = new BlockingCollection<HashAlgorithm>(concurrency);
    }

    public sealed override Guid NewGuid()
    {
        return this.NewGuid(Guid.Empty, Array.Empty<byte>());
    }

    public sealed override Guid NewGuid(Guid nsId, byte[] name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        var input = this.CreateInput(nsId, name);
        var hashBytes = this.ComputeHash(input);
        return this.HashBytesToGuid(hashBytes);
    }

    protected abstract HashAlgorithm CreateHashing();

    private unsafe byte[] CreateInput(Guid nsId, byte[] name)
    {
        const int guidSize = 16;
        var input = new byte[guidSize + name.Length];
        fixed (byte* pInput = &input[0]) { nsId.WriteUuidBytes(pInput); }
        Buffer.BlockCopy(name, 0, input, guidSize, name.Length);
        return input;
    }

    private byte[] ComputeHash(byte[] input)
    {
        var hashings = this.Hashings;
        if (!hashings.TryTake(out var hashing))
        {
            hashing = this.CreateHashing();
        }
        var hash = hashing.ComputeHash(input);
        if (!hashings.TryAdd(hashing))
        {
            hashing.Dispose();
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
        private static volatile NameBasedGuidGenerator.MD5Hashing? Singleton;

        private MD5Hashing() { }

        internal static NameBasedGuidGenerator.MD5Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.MD5Hashing Initialize()
                {
                    return NameBasedGuidGenerator.MD5Hashing.Singleton ??=
                        new NameBasedGuidGenerator.MD5Hashing();
                }

                return NameBasedGuidGenerator.MD5Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version3;

        protected override HashAlgorithm CreateHashing() => MD5.Create();
    }

    internal sealed class SHA1Hashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.SHA1Hashing? Singleton;

        private SHA1Hashing() { }

        internal static NameBasedGuidGenerator.SHA1Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.SHA1Hashing Initialize()
                {
                    return NameBasedGuidGenerator.SHA1Hashing.Singleton ??=
                        new NameBasedGuidGenerator.SHA1Hashing();
                }

                return NameBasedGuidGenerator.SHA1Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version5;

        protected override HashAlgorithm CreateHashing() => SHA1.Create();
    }
}
