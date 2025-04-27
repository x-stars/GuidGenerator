using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Running;
#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
using System.Security.Cryptography;
#endif

internal static class BenchmarkProgram
{
    internal static void Main(string[] args)
    {
        var assembly = typeof(BenchmarkProgram).Assembly;
        var config = DefaultConfig.Instance.AddFilter(
            new SimpleFilter(BenchmarkProgram.FilterBenchmarkCase));
        _ = BenchmarkSwitcher.FromAssembly(assembly).Run(args, config);
    }

    private static bool FilterBenchmarkCase(BenchmarkCase benchmarkCase)
    {
        return benchmarkCase.Descriptor.DisplayInfo switch
        {
#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
            var value when value.Contains("Sha3D256") => SHA3_256.IsSupported,
            var value when value.Contains("Sha3D384") => SHA3_384.IsSupported,
            var value when value.Contains("Sha3D512") => SHA3_512.IsSupported,
            var value when value.Contains("Shake128") => Shake128.IsSupported,
            var value when value.Contains("Shake256") => Shake256.IsSupported,
#endif
            _ => true,
        };
    }
}
