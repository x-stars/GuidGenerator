using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class NewNameBasedGuidCommand : ProgramCommand
{
    internal static readonly NewNameBasedGuidCommand Version3 =
        new NewNameBasedGuidCommand(GuidVersion.Version3);

    internal static readonly NewNameBasedGuidCommand Version5 =
        new NewNameBasedGuidCommand(GuidVersion.Version5);

    internal static readonly NewNameBasedGuidCommand Version8NSha256 =
        new NewNameBasedGuidCommand(hashingName: "SHA256");

    internal static readonly NewNameBasedGuidCommand Version8NSha384 =
        new NewNameBasedGuidCommand(hashingName: "SHA384");

    internal static readonly NewNameBasedGuidCommand Version8NSha512 =
        new NewNameBasedGuidCommand(hashingName: "SHA512");

    private readonly GuidVersion Version;

    private readonly string? HashingName;

    private NewNameBasedGuidCommand(GuidVersion version)
    {
        this.Version = version;
    }

    private NewNameBasedGuidCommand(string hashingName)
    {
        this.HashingName = hashingName;
    }

    public override bool TryExecute(string[] args)
    {
        var hashName = this.HashingName;
        var hashArgCount = (hashName is null) ? 0 : 1;
        if ((args.Length - hashArgCount) is not (2 or 3))
        {
            return false;
        }
        var verArg = args[0].ToUpper();
        var version = this.Version;
        var expVerName = (hashName is null) ?
            ((int)version).ToString() : "8N";
        var expVerArg = $"-V{expVerName}";
        if (verArg != expVerArg)
        {
            return false;
        }

        var hashArg = (hashName is null) ? null : args[1].ToUpper();
        if (hashArg != hashName) { return false; }
        var readInput = (args.Length - hashArgCount) == 2;
        var nsIdArg = args[hashArgCount + 1].ToUpper();
        var nsId = Guid.Empty;
        if (nsIdArg.StartsWith(":"))
        {
            var nsName = nsIdArg[1..];
            if (!this.TryParseNamespace(nsName, out nsId))
            {
                return false;
            }
        }
        else if (!Guid.TryParse(nsIdArg, out nsId))
        {
            return false;
        }

        var guidGen = this.GetGuidGenerator();
        if (readInput)
        {
            var name = default(string);
            while ((name = Console.ReadLine()) != null)
            {
                var guid = guidGen.NewGuid(nsId, name);
                Console.WriteLine(guid);
            }
        }
        else
        {
            var name = args[hashArgCount + 2];
            var guid = guidGen.NewGuid(nsId, name);
            Console.WriteLine(guid);
        }
        return true;
    }

    private INameBasedGuidGenerator GetGuidGenerator()
    {
        return this.HashingName switch
        {
            "SHA256" => GuidGenerator.Version8NSha256,
            "SHA384" => GuidGenerator.Version8NSha384,
            "SHA512" => GuidGenerator.Version8NSha512,
            _ => (INameBasedGuidGenerator)GuidGenerator.OfVersion(this.Version),
        };
    }

    private bool TryParseNamespace(string nsName, out Guid nsId)
    {
        nsId = nsName switch
        {
            "DNS" => GuidNamespaces.Dns,
            "URL" => GuidNamespaces.Url,
            "OID" => GuidNamespaces.Oid,
            "X500" => GuidNamespaces.X500,
            "MAX" => Uuid.MaxValue,
            "NIL" or _ => Guid.Empty,
        };
        return (nsId != Guid.Empty) || (nsName == "NIL");
    }
}
