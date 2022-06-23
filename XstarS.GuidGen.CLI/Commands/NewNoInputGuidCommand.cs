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

    internal static readonly NewNoInputGuidCommand Version1R =
        new NewNoInputGuidCommand(useRandomNodeId: true);

    private readonly GuidVersion Version;

    private readonly bool UseRandomNodeId;

    private NewNoInputGuidCommand(GuidVersion version)
    {
        this.Version = version;
    }

    private NewNoInputGuidCommand(bool useRandomNodeId)
        : this(GuidVersion.Version1)
    {
        this.UseRandomNodeId = useRandomNodeId;
    }

    public override bool TryExecute(string[] args)
    {
        if (args.Length is not (1 or 2))
        {
            return false;
        }
        var verArg = args[0].ToUpper();
        var version = this.Version;
        var verNum = ((int)version).ToString();
        var useRandId = this.UseRandomNodeId;
        var expVerArg = $"-V{verNum}";
        if (useRandId) { expVerArg += "R"; }
        if (verArg != expVerArg)
        {
            return false;
        }

        var count = 1;
        if (args.Length == 2)
        {
            var countArg = args[1].ToUpper();
            if (!countArg.StartsWith("-C"))
            {
                return false;
            }
            var cParsed = int.TryParse(countArg[2..], out count);
            if (!cParsed || (count < 0)) { return false; }
        }

        var guidGen = useRandId ?
            GuidGenerator.CreateVersion1R() :
            GuidGenerator.OfVersion(version);
        foreach (var current in ..count)
        {
            var guid = guidGen.NewGuid();
            Console.WriteLine(guid.ToString());
        }
        return true;
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
