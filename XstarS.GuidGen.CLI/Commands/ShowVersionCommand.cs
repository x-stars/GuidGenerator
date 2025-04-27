using System;

namespace XstarS.GuidGen.Commands;

internal sealed class ShowVersionCommand : SingleOptionCommand
{
    internal static readonly ShowVersionCommand Instance = new();

    private ShowVersionCommand() : base("-VERSION") { }

    protected override void ExecuteCore(string optionArg)
    {
        var version = ThisAssembly.Info.InformationalVersion;
        version = version[..version.LastIndexOf('+')];
        Console.WriteLine($"{ThisAssembly.Info.Title} {version}");
    }
}
