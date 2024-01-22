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
        if (args.Length is not (2 or 3))
        {
            return false;
        }
        var verArg = args[0].ToUpperInvariant();
        if (verArg != "-V2")
        {
            return false;
        }

        var domainArg = args[1];
        var siteIdArg = (args.Length == 3) ? args[2] : null;
        var nSiteId = default(int?);
        var dParsed = Enum.TryParse<DceSecurityDomain>(
            domainArg, ignoreCase: true, out var domain);
        if (!dParsed) { return false; }
        if (siteIdArg is not null)
        {
            if (siteIdArg is null) { return false; }
            var iParsed = this.TryParseSiteId(siteIdArg, out var siteId);
            if (!iParsed) { return false; }
            nSiteId = (int)siteId;
        }
        else if (domain is not (DceSecurityDomain.Person or DceSecurityDomain.Group))
        {
            return false;
        }

        var guidGen = GuidGenerator.Version2;
        var guid = guidGen.NewGuid(domain, nSiteId);
        Console.Out.WriteLine(in guid);
        return true;
    }

    private bool TryParseSiteId(string idText, out uint result)
    {
        var parsed = uint.TryParse(idText, out result);
        if (!parsed)
        {
            try
            {
                result = Convert.ToUInt32(idText, 16);
                parsed = true;
            }
            catch (Exception) { }
        }
        return parsed;
    }
}
