using System;

namespace XstarS.GuidGen.Commands;

internal sealed class ShowVersionCommand : SingleOptionCommand
{
    internal static readonly ShowVersionCommand Instance = new();

    private ShowVersionCommand() : base("-VERSION") { }

    protected override void ExecuteCore(string optionArg)
    {
        Console.WriteLine($"{AssemblyInfo.Title} {AssemblyInfo.Version}");
    }
}
