using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract partial class NameBasedGuidGenerator : GuidGenerator, INameBasedGuidGenerator
{
    private readonly ThreadLocal<HashAlgorithm?> LocalHashing;

    protected NameBasedGuidGenerator()
    {
        this.LocalHashing = new ThreadLocal<HashAlgorithm?>(
            this.CreateHashing, this.TrackHashing);
    }

    protected virtual bool TrackHashing => false;

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
        ArgumentNullException.ThrowIfNull(name);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return this.NewGuid(nsId, (ReadOnlySpan<byte>)name);
#else
        var hashing = this.GetHashing();
        try
        {
            var hash = this.ComputeHash(hashing, nsId, name);
            return this.HashToGuid(hash);
        }
        finally
        {
            this.ReturnHashing(hashing);
        }
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override Guid NewGuid(Guid nsId, ReadOnlySpan<byte> name)
    {
        var hashing = this.GetHashing();
        try
        {
            var hashSize = hashing.HashSize / 8;
            var hash = ((uint)hashSize <= 1024) ?
                (stackalloc byte[hashSize]) : (new byte[hashSize]);
            var result = this.TryComputeHash(
                hashing, nsId, name, hash, out var bytesWritten);
            if (!result || (bytesWritten != hashSize))
            {
                throw new InvalidOperationException(
                    "The algorithm's implementation is incorrect.");
            }
            return this.HashToGuid(hash);
        }
        finally
        {
            this.ReturnHashing(hashing);
        }
    }
#endif

    protected abstract HashAlgorithm CreateHashing();

    protected virtual HashAlgorithm GetHashing()
    {
        var hashing = this.LocalHashing.Value!;
        Debug.Assert(hashing is not null);
        if (this.TrackHashing)
        {
            this.LocalHashing.Value = null;
        }
        return hashing!;
    }

    protected virtual void ReturnHashing(HashAlgorithm hashing)
    {
        if (this.TrackHashing)
        {
            this.LocalHashing.Value = hashing;
        }
    }

#if !UUIDREV_DISABLE
    protected void DisposeHashings()
    {
        Debug.Assert(this.TrackHashing);
        var hashings = this.LocalHashing.Values;
        foreach (var hashing in hashings)
        {
            hashing?.Dispose();
        }
        this.LocalHashing.Dispose();
    }
#endif

    private static class LocalBuffers
    {
        [ThreadStatic] private static byte[]? GuidBytesValue;

        internal static byte[] GuidBytes =>
            LocalBuffers.GuidBytesValue ??= new byte[16];
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private bool TryComputeHash(
        HashAlgorithm hashing, Guid nsId, ReadOnlySpan<byte> name,
        Span<byte> destination, out int bytesWritten)
    {
        var guidBytes = LocalBuffers.GuidBytes;
        var nsIdResult = nsId.TryWriteUuidBytes(guidBytes);
        Debug.Assert(nsIdResult);
        hashing.TransformBlock(guidBytes, 0, 16, null, 0);
        return hashing.TryComputeHash(name, destination, out bytesWritten);
    }

    private Guid HashToGuid(ReadOnlySpan<byte> hash)
    {
#if !UUIDREV_DISABLE
        if (hash.Length < 16)
        {
            throw new InvalidOperationException(
                "The algorithm's hash size is less than 128 bits.");
        }
#endif

        var guid = Uuid.FromBytes(hash[..16]);
        this.FillVersionFieldUnchecked(ref guid);
        this.FillVariantFieldUnchecked(ref guid);
        return guid;
    }
#else
    private unsafe byte[] ComputeHash(
        HashAlgorithm hashing, Guid nsId, byte[] name)
    {
        var guidBytes = LocalBuffers.GuidBytes;
        fixed (byte* pGuidBytes = &guidBytes[0])
        {
            *(Guid*)pGuidBytes = nsId.ToBigEndian();
        }
        hashing.TransformBlock(guidBytes, 0, 16, null, 0);
        return hashing.ComputeHash(name);
    }

    private unsafe Guid HashToGuid(byte[] hash)
    {
#if !UUIDREV_DISABLE
        if (hash.Length < 16)
        {
            throw new InvalidOperationException(
                "The algorithm's hash size is less than 128 bits.");
        }
#endif

        var uuid = default(Guid);
        fixed (byte* pHash = &hash[0])
        {
            uuid = *(Guid*)pHash;
        }
        var guid = uuid.ToBigEndian();
        this.FillVersionFieldUnchecked(ref guid);
        this.FillVariantFieldUnchecked(ref guid);
        return guid;
    }
#endif
}
