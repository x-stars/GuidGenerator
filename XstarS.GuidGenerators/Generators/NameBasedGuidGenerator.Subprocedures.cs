using System;
using System.Security.Cryptography;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private Guid ComputeHashToGuid(Guid nsId, ReadOnlySpan<byte> name)
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

    private bool TryComputeHash(
        HashAlgorithm hashing, Guid nsId, ReadOnlySpan<byte> name,
        Span<byte> destination, out int bytesWritten)
    {
        if (!IncrementalHashAlgorithm.IsSupported
#if DEBUG
            == (Environment.TickCount % 2 != 0) // For testing fallback.
#endif
            )
        {
            return this.TryComputeHashFallback(
                hashing, nsId, name, destination, out bytesWritten);
        }

        hashing.Initialize();
        var guidBytes = (stackalloc byte[16]);
        var nsIdResult = nsId.TryWriteUuidBytes(guidBytes);
        Debug.Assert(nsIdResult);
        hashing.AppendData(guidBytes);
        hashing.AppendData(name);
        return hashing.TryGetFinalHash(destination, out bytesWritten);
    }

    private bool TryComputeHashFallback(
        HashAlgorithm hashing, Guid nsId, ReadOnlySpan<byte> name,
        Span<byte> destination, out int bytesWritten)
    {
#if DEBUG
        hashing.Initialize();
#endif
        const int guidSize = 16;
        var inputLength = guidSize + name.Length;
        var input = ((uint)name.Length <= 1024) ?
            (stackalloc byte[inputLength]) : (new byte[inputLength]);
        var nsIdResult = nsId.TryWriteUuidBytes(input);
        Debug.Assert(nsIdResult);
        name.CopyTo(input[guidSize..]);
        return hashing.TryComputeHash(input, destination, out bytesWritten);
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
    private static class LocalBuffers
    {
        [ThreadStatic] private static byte[]? GuidBytesValue;

        internal static byte[] GuidBytes =>
            LocalBuffers.GuidBytesValue ??= new byte[16];
    }

    private Guid ComputeHashToGuid(Guid nsId, byte[] name)
    {
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
    }

    private unsafe byte[] ComputeHash(
        HashAlgorithm hashing, Guid nsId, byte[] name)
    {
        if (!IncrementalHashAlgorithm.IsSupported
#if DEBUG
            == (Environment.TickCount % 2 != 0) // For testing fallback.
#endif
            )
        {
            return this.ComputeHashFallback(hashing, nsId, name);
        }

        hashing.Initialize();
        var guidBytes = LocalBuffers.GuidBytes;
        fixed (byte* pGuidBytes = &guidBytes[0])
        {
            *(Guid*)pGuidBytes = nsId.ToBigEndian();
        }
        hashing.AppendData(guidBytes);
        hashing.AppendData(name);
        return hashing.GetFinalHash();
    }

    private unsafe byte[] ComputeHashFallback(
        HashAlgorithm hashing, Guid nsId, byte[] name)
    {
#if DEBUG
        hashing.Initialize();
#endif
        const int guidSize = 16;
        var inputLength = guidSize + name.Length;
        var input = new byte[inputLength];
        fixed (byte* pInput = &input[0])
        {
            *(Guid*)pInput = nsId.ToBigEndian();
        }
        Buffer.BlockCopy(name, 0, input, guidSize, name.Length);
        return hashing.ComputeHash(input);
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
