using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal class NewNoInputGuidCommand : ProgramCommand
{
    internal static readonly NewNoInputGuidCommand Default =
        new NewNoInputGuidCommand.DefaultVersion();

    internal static readonly NewNoInputGuidCommand Version1 =
        new NewNoInputGuidCommand(GuidVersion.Version1);

    internal static readonly NewNoInputGuidCommand Version4 =
        new NewNoInputGuidCommand(GuidVersion.Version4);

    internal static readonly NewNoInputGuidCommand Version6 =
        new NewNoInputGuidCommand(GuidVersion.Version6);

    internal static readonly NewNoInputGuidCommand Version7 =
        new NewNoInputGuidCommand(GuidVersion.Version7);

    internal static readonly NewNoInputGuidCommand Version8 =
        new NewNoInputGuidCommand(GuidVersion.Version8);

    internal static readonly NewNoInputGuidCommand Version1R =
        new NewNoInputGuidCommand(versionName: "Version1R");

    internal static readonly NewNoInputGuidCommand Version6P =
        new NewNoInputGuidCommand(versionName: "Version6P");

    private readonly GuidVersion Version;

    private readonly string? VersionName;

    private NewNoInputGuidCommand(GuidVersion version)
    {
        this.Version = version;
    }

    private NewNoInputGuidCommand(string versionName)
    {
        this.VersionName = versionName;
    }

    public override bool TryExecute(string[] args)
    {
        if (args.Length is not (1 or 2))
        {
            return false;
        }
        var verArg = args[0].ToUpperInvariant();
        var version = this.Version;
        var verName = this.VersionName;
        var expVerName = (verName is null) ?
            ((int)version).ToString() :
            verName["Version".Length..];
        if (verArg != $"-V{expVerName}")
        {
            return false;
        }

        var count = 1;
        if (args.Length == 2)
        {
            var countArg = args[1].ToUpperInvariant();
            if (!countArg.StartsWith(" -C")) { return false; }
            var cParsed = int.TryParse(countArg[2..], out count);
            if (!cParsed || (count < 0)) { return false; }
        }

        var guidGen = this.GetGuidGenerator();
        foreach (var current in ..count)
        {
            var guid = guidGen.NewGuid();
            Console.WriteLine(guid.ToString());
        }
        return true;
    }

    private IGuidGenerator GetGuidGenerator()
    {
        return this.VersionName switch
        {
            "Version1R" => GuidGenerator.Version1R,
            "Version6P" => GuidGenerator.Version6P,
            _ => GuidGenerator.OfVersion(this.Version),
        };
    }

    private sealed class DefaultVersion : NewNoInputGuidCommand
    {
        internal DefaultVersion() : base(GuidVersion.Version4) { }

        public override bool TryExecute(string[] args)
        {
            var newArgs = new string[args.Length + 1];
            Array.Copy(args, 0, newArgs, 1, args.Length);
            var verNum = ((int)this.Version).ToString();
            newArgs[0] = $"-V{verNum}";
            return base.TryExecute(newArgs);
        }
    }
}
