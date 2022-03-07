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

            var cmdName = this.GetCommandName();
            var helpLines = new[]
            {
                $"Usage:  {cmdName} [-V1|-V2|-V4] [-Cn]",
                $"        {cmdName} -V3|-V5 :NS|GuidNS [Name]",
                $"        {cmdName} -?|-H|-Help",
                "Parameters:",
                "    -V1     generate time-based GUIDs.",
                "    -V2     generate DCE security GUIDs.",
                "    -V3     generate name-based GUID by MD5 hashing.",
                "    -V4     generate pesudo-random GUIDs (default).",
                "    -V5     generate name-based GUID by SHA1 hashing.",
                "    -Cn     generate n GUIDs of the current version.",
                "    :NS     specify a well-known GUID namespace,",
                "            which can be :DNS, :URL, :OID or :X500.",
                "    GuidNS  specify a user-defined GUID namespace.",
                "    Name    specify the name to generate GUID,",
                "            or empty to read from standard input.",
                "    -?|-H|-Help",
                "            show the current help message.",
            };
            foreach (var helpLine in helpLines)
            {
                Console.WriteLine(helpLine);
            }
            return true;
        }
    }
}
