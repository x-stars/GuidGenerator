using System.Collections.Generic;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class ResetStateCommand : ProgramCommand
{
    internal static readonly ResetStateCommand Instance = new();

    private static readonly HashSet<string> ResetNames = ["-RS", "-RESET"];

    private ResetStateCommand() { }

    public override bool TryExecute(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }
        var resetNames = ResetStateCommand.ResetNames;
        var resetArg = args[0].ToUpperInvariant();
        if (!resetNames.Contains(resetArg))
        {
            return false;
        }

        GuidGenerator.ResetState();
        return true;
    }
}
