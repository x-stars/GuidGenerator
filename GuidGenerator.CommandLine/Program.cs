using System;

namespace XstarS.GuidGenerators
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            foreach (var verNum in new[] { 1, 3, 4, 5 })
            {
                var version = (GuidVersion)verNum;
                var guid = GuidGenerator.NewGuid(version);
                Console.WriteLine(guid.ToString());
            }
        }
    }
}
