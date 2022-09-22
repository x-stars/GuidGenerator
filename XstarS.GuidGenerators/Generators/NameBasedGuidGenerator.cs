using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace XNetEx.Guids.Generators;

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
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return this.NewGuid(Guid.Empty, ReadOnlySpan<byte>.Empty);
#else
        return this.NewGuid(Guid.Empty, Array.Empty<byte>());
#endif
    }

    public sealed override Guid NewGuid(Guid nsId, byte[] name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return this.NewGuid(nsId, (ReadOnlySpan<byte>)name);
#else
        var input = this.CreateInput(nsId, name);
        var hash = this.ComputeHash(input);
        return this.HashToGuid(hash);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override Guid NewGuid(Guid nsId, ReadOnlySpan<byte> name)
    {
        const int guidSize = 16;
        var inputLength = guidSize + name.Length;
        var input = (name.Length <= 1024) ?
            (stackalloc byte[inputLength]) : (new byte[inputLength]);
        _ = nsId.TryWriteUuidBytes(input);
        name.CopyTo(input[guidSize..]);
        var hashings = this.Hashings;
        if (!hashings.TryTake(out var hashing))
        {
            hashing = this.CreateHashing();
        }
        var hashSize = hashing.HashSize / 8;
        var hash = (stackalloc byte[hashSize]);
        _ = hashing.TryComputeHash(input, hash, out _);
        if (!hashings.TryAdd(hashing))
        {
            hashing.Dispose();
        }
        return this.HashToGuid(hash);
    }
#endif

    protected abstract HashAlgorithm CreateHashing();

#if !(NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER)
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
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private unsafe Guid HashToGuid(ReadOnlySpan<byte> hash)
#else
    private unsafe Guid HashToGuid(byte[] hash)
#endif
    {
        var guid = default(Guid);
        fixed (byte* pHash = &hash[0])
        {
            var uuid = *(Guid*)pHash;
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
