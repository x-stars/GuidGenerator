using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;

namespace XstarS.GuidGenerators
{
    internal static class BenchmarkProgram
    {
        internal static void Main(string[] args)
        {
            var assembly = typeof(BenchmarkProgram).Assembly;
            var config = ManualConfig.Create(DefaultConfig.Instance);
            if (Environment.GetEnvironmentVariable("R_HOME") != null)
            {
                config.AddExporter(RPlotExporter.Default);
            }
            var summaries = BenchmarkRunner.Run(assembly, config);
        }
    }
}
