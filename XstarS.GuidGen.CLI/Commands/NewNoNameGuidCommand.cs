using System;

namespace XstarS.GuidGenerators.Commands
{
    internal class NewNoNameGuidCommand : ProgramCommand
    {
        internal static readonly NewNoNameGuidCommand Default =
            new NewNoNameGuidCommand.DefaultVersion();

        internal static readonly NewNoNameGuidCommand Version1 =
            new NewNoNameGuidCommand(GuidVersion.Version1);

        internal static readonly NewNoNameGuidCommand Version2 =
            new NewNoNameGuidCommand(GuidVersion.Version2);

        internal static readonly NewNoNameGuidCommand Version4 =
            new NewNoNameGuidCommand(GuidVersion.Version4);

        private readonly GuidVersion Version;

        private NewNoNameGuidCommand(GuidVersion version)
        {
            this.Version = version;
        }

        public override bool TryExecute(string[] args)
        {
            if ((args.Length != 1) && (args.Length != 2))
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

            var count = 1;
            if (args.Length == 2)
            {
                var countArg = args[1].ToUpper();
                if (!countArg.StartsWith("-C"))
                {
                    return false;
                }
                var parsed = int.TryParse(countArg[2..], out count);
                if (!parsed || (count < 0)) { return false; }
            }
            foreach (var _ in ..count)
            {
                var guid = GuidGenerator.NewGuid(version);
                Console.WriteLine(guid.ToString());
            }
            return true;
        }

        private sealed class DefaultVersion : NewNoNameGuidCommand
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
}
