using System;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class ResetStateCommand : ProgramCommand
{
    internal static readonly ResetStateCommand Instance = new();

    private static readonly string[] ResetNames = ["-RS", "-RESET"];

    private ResetStateCommand() { }

    public override bool TryExecute(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }
        var resetNames = ResetStateCommand.ResetNames;
        var resetArg = args[0].ToUpperInvariant();
        if (Array.IndexOf(resetNames, resetArg) < 0)
        {
            return false;
        }

        GuidGenerator.ResetState();
        return true;
    }
}
