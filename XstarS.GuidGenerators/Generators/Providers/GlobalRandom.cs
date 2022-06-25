using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;

namespace XNetEx.Guids.Generators;

internal static class GlobalRandom
{
    private const int BufferLength = 4096;

    private static readonly BlockingCollection<RandomNumberGenerator> Generators =
        new BlockingCollection<RandomNumberGenerator>(GlobalRandom.Concurrency);

    [ThreadStatic] private static byte[]? LocalBuffer;

    [ThreadStatic] private static int LocalOffset;

    private static int Concurrency => Environment.ProcessorCount * 2;

    public static unsafe T Next<T>() where T : unmanaged
    {
        var buffer = GlobalRandom.GetOrInitBuffer();
        var offset = GlobalRandom.GetAndAddOffset(sizeof(T));
        fixed (byte* pBuffer = &buffer[offset])
        {
            return *(T*)pBuffer;
        }
    }

    public static void NextBytes(byte[] data)
    {
        var buffer = GlobalRandom.GetOrInitBuffer();
        var offset = GlobalRandom.GetAndAddOffset(data.Length);
        Buffer.BlockCopy(buffer, offset, data, 0, data.Length);
    }

    private static byte[] GetOrInitBuffer()
    {
        var buffer = GlobalRandom.LocalBuffer;
        if (buffer is null)
        {
            buffer = new byte[GlobalRandom.BufferLength];
            GlobalRandom.FillRandomBytes(buffer);
            GlobalRandom.LocalBuffer = buffer;
        }
        return buffer;
    }

    private static int GetAndAddOffset(int length)
    {
        Debug.Assert(length <= GlobalRandom.BufferLength);
        var nextOffset = GlobalRandom.LocalOffset + length;
        if (nextOffset > GlobalRandom.BufferLength)
        {
            var buffer = GlobalRandom.GetOrInitBuffer();
            GlobalRandom.FillRandomBytes(buffer);
            nextOffset = length;
        }
        GlobalRandom.LocalOffset = nextOffset;
        return nextOffset - length;
    }

    private static void FillRandomBytes(byte[] buffer)
    {
        var generators = GlobalRandom.Generators;
        if (!generators.TryTake(out var generator))
        {
            generator = RandomNumberGenerator.Create();
        }
        generator.GetBytes(buffer);
        if (!generators.TryAdd(generator))
        {
            generator.Dispose();
        }
    }
}
