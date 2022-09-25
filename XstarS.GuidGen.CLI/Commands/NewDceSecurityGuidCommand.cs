using System;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class NewDceSecurityGuidCommand : ProgramCommand
{
    internal static readonly NewDceSecurityGuidCommand Version2 =
        new NewDceSecurityGuidCommand();

    private NewDceSecurityGuidCommand() { }

    public override bool TryExecute(string[] args)
    {
        if (args.Length is not (2 or 3 or 4))
        {
            return false;
        }
        var verArg = args[0].ToUpper();
        if (verArg != "-V2")
        {
            return false;
        }

        var count = 1;
        var domainArg = default(string);
        var siteIdArg = default(string);
        if (args[1].ToUpper().StartsWith("-C"))
        {
            var countArg = args[1];
            var cParsed = int.TryParse(countArg[2..], out count);
            if (!cParsed || (count < 0)) { return false; }
            if (args.Length == 2) { return false; }
            domainArg = args[2];
            if (args.Length == 4) { siteIdArg = args[3]; }
        }
        else
        {
            domainArg = args[1];
            if (args.Length == 3) { siteIdArg = args[2]; }
            if (args.Length == 4) { return false; }
        }

        var nSiteId = default(int?);
        var dParsed = Enum.TryParse<DceSecurityDomain>(
            domainArg, ignoreCase: true, out var domain);
        if (!dParsed) { return false; }
        if (domain is DceSecurityDomain.Person or DceSecurityDomain.Group)
        {
            if (siteIdArg is not null) { return false; }
        }
        else
        {
            if (siteIdArg is null) { return false; }
            var iParsed = this.TryParseSiteId(siteIdArg, out var siteId);
            if (!iParsed) { return false; }
            nSiteId = (int)siteId;
        }

        var guidGen = GuidGenerator.Version2;
        foreach (var current in ..count)
        {
            var guid = guidGen.NewGuid(domain, nSiteId);
            Console.WriteLine(guid);
        }
        return true;
    }

    private bool TryParseSiteId(string idText, out uint siteId)
    {
        var parsed = uint.TryParse(idText, out siteId);
        if (!parsed)
        {
            try
            {
                siteId = Convert.ToUInt32(idText, 16);
                parsed = true;
            }
            catch (Exception) { }
        }
        return parsed;
    }
}
