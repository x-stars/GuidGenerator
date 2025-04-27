using System;

namespace XstarS.GuidGen.Commands;

internal abstract class SingleOptionCommand : ProgramCommand
{
    private readonly string[] OptionNames;

    protected SingleOptionCommand(params string[] optionNames)
    {
        this.OptionNames = optionNames;
    }

    public sealed override bool TryExecute(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }
        var optionNames = this.OptionNames;
        var optionArg = args[0].ToUpperInvariant();
        if (Array.IndexOf(optionNames, optionArg) < 0)
        {
            return false;
        }

        this.ExecuteCore(optionArg);
        return true;
    }

    protected abstract void ExecuteCore(string optionArg);
}
