using System;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators;

internal static class GlobalRandom
{
    private static readonly Random Numbers = new Random();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static int Next()
    {
        return GlobalRandom.Numbers.Next();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static double NextDouble()
    {
        return GlobalRandom.Numbers.NextDouble();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void NextBytes(byte[] buffer)
    {
        GlobalRandom.Numbers.NextBytes(buffer);
    }
}
