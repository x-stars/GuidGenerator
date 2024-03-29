﻿using BenchmarkDotNet.Running;

namespace XNetEx.Guids;

internal static class BenchmarkProgram
{
    internal static void Main(string[] args)
    {
        var assembly = typeof(BenchmarkProgram).Assembly;
        _ = BenchmarkSwitcher.FromAssembly(assembly).Run(args);
    }
}
