using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
using System.Runtime.InteropServices;
#endif

namespace XNetEx.Guids.Generators;

internal abstract partial class NameBasedGuidGenerator : GuidGenerator, INameBasedGuidGenerator
{
    private readonly BlockingCollection<HashAlgorithm> Hashings;

    private readonly Guid? HashspaceId;

    private NameBasedGuidGenerator() : this(hashspaceId: null) { }

    private NameBasedGuidGenerator(Guid? hashspaceId)
    {
        var concurrency = Environment.ProcessorCount * 2;
        this.Hashings = new BlockingCollection<HashAlgorithm>(concurrency);
        this.HashspaceId = hashspaceId;
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
        var hashId = this.HashspaceId;
        var hashIdSize = (hashId is null) ? 0 : guidSize;
        var nameOffset = hashIdSize + guidSize;
        var inputLength = nameOffset + name.Length;
        var input = (name.Length <= 1024) ?
            (stackalloc byte[inputLength]) : (new byte[inputLength]);
        if (hashId is Guid hashIdValue)
        {
            var hashIdResult = hashIdValue.TryWriteUuidBytes(input);
            Debug.Assert(hashIdResult);
        }
        var nsIdResult = nsId.TryWriteUuidBytes(input[hashIdSize..]);
        Debug.Assert(nsIdResult);
        name.CopyTo(input[nameOffset..]);
        return this.ComputeHashToGuid(input);
    }
#endif

    protected abstract HashAlgorithm CreateHashing();

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private Guid ComputeHashToGuid(ReadOnlySpan<byte> input)
    {
        var hashings = this.Hashings;
        if (!hashings.TryTake(out var hashing))
        {
            hashing = this.CreateHashing();
        }
        var hashSize = hashing.HashSize / 8;
        var hash = (stackalloc byte[hashSize]);
        var hashResult = hashing.TryComputeHash(
            input, hash, out var bytesWritten);
        if (!hashResult || (bytesWritten != hashSize))
        {
            throw new InvalidOperationException(
                "The algorithm's implementation is incorrect.");
        }
        if (!hashings.TryAdd(hashing))
        {
            hashing.Dispose();
        }
        return this.HashToGuid(hash);
    }

    private Guid HashToGuid(ReadOnlySpan<byte> hash)
    {
        if (hash.Length < 16)
        {
            throw new InvalidOperationException(
                "The algorithm's hash size is less than 128 bits.");
        }

        var uuid = MemoryMarshal.Read<Guid>(hash);
        var guid = uuid.ToBigEndian();
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }
#else
    private unsafe byte[] CreateInput(Guid nsId, byte[] name)
    {
        const int guidSize = 16;
        var hashId = this.HashspaceId;
        var hashIdSize = (hashId is null) ? 0 : guidSize;
        var nameOffset = hashIdSize + guidSize;
        var inputLength = nameOffset + name.Length;
        var input = new byte[inputLength];
        fixed (byte* pInput = &input[0])
        {
            if (hashId is Guid hashIdValue)
            {
                *(Guid*)pInput = hashIdValue.ToBigEndian();
            }
            *(Guid*)&pInput[hashIdSize] = nsId.ToBigEndian();
        }
        Buffer.BlockCopy(name, 0, input, nameOffset, name.Length);
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

    private unsafe Guid HashToGuid(byte[] hash)
    {
        if (hash.Length < 16)
        {
            throw new InvalidOperationException(
                "The algorithm's hash size is less than 128 bits.");
        }

        var uuid = default(Guid);
        fixed (byte* pHash = &hash[0])
        {
            uuid = *(Guid*)pHash;
        }
        var guid = uuid.ToBigEndian();
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }
#endif
}
