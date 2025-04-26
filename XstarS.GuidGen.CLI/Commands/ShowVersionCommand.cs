using System;

namespace XstarS.GuidGen.Commands;

internal sealed class ShowVersionCommand : ProgramCommand
{
    internal static readonly ShowVersionCommand Instance = new();

    private static readonly string[] VersionNames = ["-V", "-VERSION"];

    private ShowVersionCommand() { }

    public override bool TryExecute(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }
        var versionNames = ShowVersionCommand.VersionNames;
        var versionArg = args[0].ToUpperInvariant();
        if (Array.IndexOf(versionNames, versionArg) < 0)
        {
            return false;
        }

        var program = ThisAssembly.Info.Title;
        var version = ThisAssembly.Info.InformationalVersion;
        version = version[..version.LastIndexOf('+')];
        Console.WriteLine($"{program}, {version}");
        return true;
    }
}
