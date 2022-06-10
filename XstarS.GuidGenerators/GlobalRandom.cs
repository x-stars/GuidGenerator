using System;

namespace XstarS.GuidGenerators
{
    internal static class GlobalRandom
    {
        private static readonly Random Numbers = new Random();

        public static int Next()
        {
            return GlobalRandom.Numbers.Next();
        }

        public static double NextDouble()
        {
            return GlobalRandom.Numbers.NextDouble();
        }

        public static void NextBytes(byte[] buffer)
        {
            GlobalRandom.Numbers.NextBytes(buffer);
        }
    }
}
