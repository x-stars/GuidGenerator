using System;
using System.Collections.Generic;

namespace XstarS.GuidGenerators.Commands
{
    internal sealed class ShowHelpCommand : ProgramCommand
    {
        internal static readonly ShowHelpCommand Instance =
            new ShowHelpCommand();

        private static readonly HashSet<string> HelpNames =
            new HashSet<string>() { "-?", "-H", "-HELP" };

        private ShowHelpCommand() { }

        public override bool TryExecute(string[] args)
        {
            if (args.Length != 1)
            {
                return false;
            }
            var helpNames = ShowHelpCommand.HelpNames;
            var helpArg = args[0].ToUpper();
            if (!helpNames.Contains(helpArg))
            {
                return false;
            }

            IEnumerable<string> GetHelpMessage()
            {
                var cmdName = this.GetCommandName();
                yield return $"Usage:  {cmdName} [-V1|-V4|-V1R] [-Cn]";
                yield return $"        {cmdName} -V2 [-Cn] Domain [SiteID]";
                yield return $"        {cmdName} -V3|-V5 :NS|GuidNS [Name]";
                yield return $"        {cmdName} -?|-H|-Help";
                yield return "Parameters:";
                yield return "    -V1     generate time-based GUIDs.";
                yield return "    -V2     generate DCE security GUIDs.";
                yield return "    -V3     generate name-based GUID by MD5 hashing.";
                yield return "    -V4     generate pseudo-random GUIDs (default).";
                yield return "    -V5     generate name-based GUID by SHA1 hashing.";
                yield return "    -V1R    generate time-based GUIDs (random node ID).";
                yield return "    -Cn     generate n GUIDs of the current version.";
                yield return "    Domain  specify a DCE security domain,";
                yield return "            which can be Person, Group or Org.";
                yield return "    SiteID  specify a user-defined local ID";
                yield return "            for DCE security domain Org (required).";
                yield return "    :NS     specify a well-known GUID namespace,";
                yield return "            which can be :DNS, :URL, :OID or :X500.";
                yield return "    GuidNS  specify a user-defined GUID namespace.";
                yield return "    Name    specify the name to generate GUID,";
                yield return "            or empty to read from standard input.";
                yield return "    -?|-H|-Help";
                yield return "            show the current help message.";
            }

            foreach (var helpLine in GetHelpMessage())
            {
                Console.WriteLine(helpLine);
            }
            return true;
        }
    }
}
