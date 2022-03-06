using System;

namespace XstarS.GuidGenerators.Commands
{
    internal sealed class NewNameBasedGuidCommand : ProgramCommand
    {
        internal static readonly NewNameBasedGuidCommand Version3 =
            new NewNameBasedGuidCommand(GuidVersion.Version3);

        internal static readonly NewNameBasedGuidCommand Version5 =
            new NewNameBasedGuidCommand(GuidVersion.Version5);

        private readonly GuidVersion Version;

        private NewNameBasedGuidCommand(GuidVersion version)
        {
            this.Version = version;
        }

        public override bool TryExecute(string[] args)
        {
            if ((args.Length != 2) && (args.Length != 3))
            {
                return false;
            }
            var verArg = args[0].ToUpper();
            var version = this.Version;
            var verNum = ((int)version).ToString();
            if (verArg != $"-V{verNum}")
            {
                return false;
            }

            var readInput = args.Length == 2;
            var nsArg = args[1].ToUpper();
            var ns = Guid.Empty;
            if (nsArg.StartsWith(":"))
            {
                var knownNs = nsArg.Substring(1);
                if (!GuidNamespaces.TryFindByName(knownNs, out ns))
                {
                    return false;
                }
            }
            else if (!Guid.TryParse(nsArg, out ns))
            {
                return false;
            }

            if (readInput)
            {
                var name = default(string);
                while ((name = Console.ReadLine()) != null)
                {
                    var guid = GuidGenerator.NewGuid(version, ns, name);
                    Console.WriteLine(guid);
                }
            }
            else
            {
                var name = args[2];
                var guid = GuidGenerator.NewGuid(version, ns, name);
                Console.WriteLine(guid);
            }
            return true;
        }
    }
}
