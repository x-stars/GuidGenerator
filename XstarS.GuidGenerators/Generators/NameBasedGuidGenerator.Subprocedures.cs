using System;
using System.Security.Cryptography;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
#elif UNSAFE_HELPERS
using System.Runtime.CompilerServices;
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
            var hash = (stackalloc byte[hashSize]);
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
        hashing.Initialize();
        var guidBytes = (stackalloc byte[16]);
        var nsIdResult = nsId.TryWriteUuidBytes(guidBytes);
        Debug.Assert(nsIdResult);
        hashing.AppendData(guidBytes);
        hashing.AppendData(name);
        return hashing.TryGetFinalHash(destination, out bytesWritten);
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
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
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

    private unsafe Guid HashToGuid(byte[] hash)
    {
#if !UUIDREV_DISABLE
        if (hash.Length < 16)
        {
            throw new InvalidOperationException(
                "The algorithm's hash size is less than 128 bits.");
        }
#endif

#if UNSAFE_HELPERS || NETCOREAPP3_0_OR_GREATER
        var uuid = Unsafe.ReadUnaligned<Guid>(ref hash[0]);
#else
        var uuid = default(Guid);
        fixed (byte* pHash = &hash[0])
        {
            uuid = *(Guid*)pHash;
        }
#endif
        var guid = uuid.ToBigEndian();
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }
#endif
}
