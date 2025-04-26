using System;
using System.Collections.Generic;

namespace XstarS.GuidGen.Commands;

internal sealed class ShowHelpCommand : ProgramCommand
{
    internal static readonly ShowHelpCommand Instance = new();

    private static readonly string[] HelpNames = ["-?", "-H", "-HELP"];

    private ShowHelpCommand() { }

    public override bool TryExecute(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }
        var helpNames = ShowHelpCommand.HelpNames;
        var helpArg = args[0].ToUpperInvariant();
        if (Array.IndexOf(helpNames, helpArg) < 0)
        {
            return false;
        }

        IEnumerable<string> GetHelpMessage()
        {
            var cmdName = this.GetCommandName();
#if !UUIDREV_DISABLE
            yield return $"Generate RFC 4122/9562 compliant GUIDs.";
#else
            yield return $"Generate RFC 4122 compliant GUIDs.";
#endif
            yield return $"Usage:  {cmdName} [-V1|-V4|-V1R] [-Cn]";
            yield return $"        {cmdName} -V2 Domain [LocalID]";
            yield return $"        {cmdName} -V3|-V5 :NS|GuidNS [Name]";
#if !UUIDREV_DISABLE
            yield return $"        {cmdName} -V6|-V7|-V8|-V6P|-V6R [-Cn]";
            yield return $"        {cmdName} -V8N Hash :NS|GuidNS [Name]";
#endif
            yield return $"        {cmdName} -V|-Version";
            yield return $"        {cmdName} -?|-H|-Help";
            yield return "Parameters:";
            yield return "    -V1     Generate time-based GUID.";
            yield return "    -V2     Generate DCE Security GUID.";
            yield return "    -V3     Generate name-based GUID by MD5 hashing.";
            yield return "    -V4     Generate pseudo-random GUID (default).";
            yield return "    -V5     Generate name-based GUID by SHA1 hashing.";
#if !UUIDREV_DISABLE
            yield return "    -V6     Generate reordered time-based GUID.";
            yield return "    -V7     Generate Unix Epoch time-based GUID.";
            yield return "    -V8     Generate custom GUID (example impl).";
#endif
            yield return "    -V1R    Generate time-based GUID (random node ID).";
#if !UUIDREV_DISABLE
            yield return "    -V6P    Generate reordered time-based GUID";
            yield return "            (IEEE 802 MAC address node ID).";
            yield return "    -V6R    Generate reordered time-based GUID";
            yield return "            (non-volatile random node ID).";
            yield return "    -V8N    Generate custom GUID (name-based).";
#endif
            yield return "    -Cn     Generate n GUIDs of the current version.";
            yield return "    Domain  Specify a DCE Security domain,";
            yield return "            which can be Person, Group or Org.";
            yield return "    LocalID Specify a user-defined local ID";
            yield return "            for DCE Security domain Org (required).";
            yield return "    :NS     Specify a well-known GUID namespace,";
            yield return "            which can be :DNS, :URL, :OID or :X500.";
            yield return "    GuidNS  Specify a user-defined GUID namespace.";
            yield return "    Name    Specify the name to generate GUID,";
            yield return "            or empty to read from standard input.";
#if !UUIDREV_DISABLE
            yield return "    Hash    Specify a well-known hash algorithm,";
#if NET8_0_OR_GREATER
            yield return "            which can be SHA256, SHA384, SHA512,";
            yield return "                SHA3-256, SHA3-384, SHA3-512,";
            yield return "                SHAKE128 or SHAKE256.";
#else
            yield return "            which can be SHA256, SHA384 or SHA512.";
#endif
#endif
            yield return "    -V|-Version";
            yield return "            Show the version information.";
            yield return "    -?|-H|-Help";
            yield return "            Show the current help message.";
        }

        foreach (var helpLine in GetHelpMessage())
        {
            Console.Out.WriteLine(helpLine);
        }
        return true;
    }
}
