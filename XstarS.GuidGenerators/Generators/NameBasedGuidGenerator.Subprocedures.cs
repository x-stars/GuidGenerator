using System;
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Diagnostics;
using System.Runtime.InteropServices;
#elif UNSAFE_HELPERS
using System.Runtime.CompilerServices;
#endif

namespace XNetEx.Guids.Generators;

partial class NameBasedGuidGenerator
{
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    private Guid ComputeHashToGuid(Guid nsId, ReadOnlySpan<byte> name)
    {
        const int guidSize = 16;
#if !UUIDREV_DISABLE
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
#else
        var inputLength = guidSize + name.Length;
        var input = (name.Length <= 1024) ?
            (stackalloc byte[inputLength]) : (new byte[inputLength]);
        var nsIdResult = nsId.TryWriteUuidBytes(input);
        Debug.Assert(nsIdResult);
        name.CopyTo(input[guidSize..]);
#endif
        return this.ComputeHashToGuid(input);
    }

    private Guid ComputeHashToGuid(ReadOnlySpan<byte> input)
    {
        var hashing = this.GetHashing();
        var hashSize = hashing.HashSize / 8;
        var hash = (stackalloc byte[hashSize]);
        try
        {
            var hashResult = hashing.TryComputeHash(
                input, hash, out var bytesWritten);
            if (!hashResult || (bytesWritten != hashSize))
            {
                throw new InvalidOperationException(
                    "The algorithm's implementation is incorrect.");
            }
        }
        finally
        {
            this.ReturnHashing(hashing);
        }
        return this.HashToGuid(hash);
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
    private unsafe byte[] CreateInput(Guid nsId, byte[] name)
    {
        const int guidSize = 16;
#if !UUIDREV_DISABLE
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
#else
        var inputLength = guidSize + name.Length;
        var input = new byte[inputLength];
        fixed (byte* pInput = &input[0])
        {
            *(Guid*)pInput = nsId.ToBigEndian();
        }
        Buffer.BlockCopy(name, 0, input, guidSize, name.Length);
#endif
        return input;
    }

    private byte[] ComputeHash(byte[] input)
    {
        var hashing = this.GetHashing();
        try
        {
            return hashing.ComputeHash(input);
        }
        finally
        {
            this.ReturnHashing(hashing);
        }
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
