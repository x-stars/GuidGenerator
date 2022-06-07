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
            if (args.Length is not (2 or 3))
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
            var nsIdArg = args[1].ToUpper();
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

            var guidGen = GuidGenerator.OfVersion(version);
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
                var name = args[2];
                var guid = guidGen.NewGuid(nsId, name);
                Console.WriteLine(guid);
            }
            return true;
        }

        private bool TryParseNamespace(string nsName, out Guid nsId)
        {
            nsId = nsName switch
            {
                "DNS" => GuidNamespaces.Dns,
                "URL" => GuidNamespaces.Url,
                "OID" => GuidNamespaces.Oid,
                "X500" => GuidNamespaces.X500,
                _ => Guid.Empty
            };
            return nsId != Guid.Empty;
        }
    }
}
