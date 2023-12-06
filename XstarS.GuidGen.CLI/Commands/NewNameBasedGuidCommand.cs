using System;
using System.Security.Cryptography;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class NewNameBasedGuidCommand : ProgramCommand
{
    internal static readonly NewNameBasedGuidCommand Version3 =
        new NewNameBasedGuidCommand(GuidVersion.Version3);

    internal static readonly NewNameBasedGuidCommand Version5 =
        new NewNameBasedGuidCommand(GuidVersion.Version5);

#if !UUIDREV_DISABLE
    internal static readonly NewNameBasedGuidCommand Version8NSha256 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA256);

    internal static readonly NewNameBasedGuidCommand Version8NSha384 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA384);

    internal static readonly NewNameBasedGuidCommand Version8NSha512 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA512);

#if NET8_0_OR_GREATER
    internal static readonly NewNameBasedGuidCommand Version8NSha3D256 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA3_256);

    internal static readonly NewNameBasedGuidCommand Version8NSha3D384 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA3_384);

    internal static readonly NewNameBasedGuidCommand Version8NSha3D512 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHA3_512);

    internal static readonly NewNameBasedGuidCommand Version8NShake128 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHAKE128);

    internal static readonly NewNameBasedGuidCommand Version8NShake256 =
        new NewNameBasedGuidCommand(HashAlgorithmNames.SHAKE256);
#endif
#endif

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
        var verArg = args[0].ToUpperInvariant();
        var version = this.Version;
        var expVerName = (hashName is null) ?
            ((int)version).ToString() : "8N";
        if (verArg != $"-V{expVerName}")
        {
            return false;
        }

        var hashArg = (hashName is null) ?
            null : args[1].ToUpperInvariant();
        if (hashArg != hashName) { return false; }
        var readInput = (args.Length - hashArgCount) == 2;
        var nsIdArg = args[hashArgCount + 1].ToUpperInvariant();
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
            while ((name = Console.In.ReadLine()) != null)
            {
                var guid = guidGen.NewGuid(nsId, name);
                Console.Out.WriteLine(in guid);
            }
        }
        else
        {
            var name = args[hashArgCount + 2];
            var guid = guidGen.NewGuid(nsId, name);
            Console.Out.WriteLine(in guid);
        }
        return true;
    }

    private INameBasedGuidGenerator GetGuidGenerator()
    {
        return this.HashingName switch
        {
#if !UUIDREV_DISABLE
            string => GuidGenerator.OfHashAlgorithm(this.HashingName),
#endif
            _ => (INameBasedGuidGenerator)GuidGenerator.OfVersion(this.Version),
        };
    }

    private bool TryParseNamespace(string name, out Guid result)
    {
        result = name switch
        {
            "DNS" => GuidNamespaces.Dns,
            "URL" => GuidNamespaces.Url,
            "OID" => GuidNamespaces.Oid,
            "X500" => GuidNamespaces.X500,
#if !UUIDREV_DISABLE
            "MAX" => Uuid.MaxValue,
#endif
            "NIL" or _ => Guid.Empty,
        };
        return (result != Guid.Empty) || (name == "NIL");
    }
}
