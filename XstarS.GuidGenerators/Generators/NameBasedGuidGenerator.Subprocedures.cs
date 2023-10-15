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
            hashing.Initialize();
            this.AppendPrefixData(hashing, nsId);
            hashing.AppendData(name);
            var hashSize = hashing.HashSize / 8;
            var hash = (stackalloc byte[hashSize]);
            var result = hashing.TryGetFinalHash(hash, out var bytesWritten);
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

    private void AppendPrefixData(HashAlgorithm hashing, Guid nsId)
    {
        var guidBytes = (stackalloc byte[16]);
#if !UUIDREV_DISABLE
        var hashId = this.HashspaceId;
        if (hashId is Guid hashIdValue)
        {
            var hashIdResult = hashIdValue.TryWriteUuidBytes(guidBytes);
            Debug.Assert(hashIdResult);
            hashing.AppendData(guidBytes);
        }
#endif
        var nsIdResult = nsId.TryWriteUuidBytes(guidBytes);
        Debug.Assert(nsIdResult);
        hashing.AppendData(guidBytes);
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
            hashing.Initialize();
            this.AppendPrefixData(hashing, nsId);
            hashing.AppendData(name);
            var hash = hashing.GetFinalHash();
            return this.HashToGuid(hash);
        }
        finally
        {
            this.ReturnHashing(hashing);
        }
    }

    private unsafe void AppendPrefixData(HashAlgorithm hashing, Guid nsId)
    {
        var guidBytes = LocalBuffers.GuidBytes;
#if !UUIDREV_DISABLE
        var hashId = this.HashspaceId;
        if (hashId is Guid hashIdValue)
        {
            fixed (byte* pGuidBytes = &guidBytes[0])
            {
                *(Guid*)pGuidBytes = hashIdValue.ToBigEndian();
            }
            hashing.AppendData(guidBytes);
        }
#endif
        fixed (byte* pGuidBytes = &guidBytes[0])
        {
            *(Guid*)pGuidBytes = nsId.ToBigEndian();
        }
        hashing.AppendData(guidBytes);
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
        this.FillVersionField(ref guid);
        this.FillVariantField(ref guid);
        return guid;
    }
#endif
}
