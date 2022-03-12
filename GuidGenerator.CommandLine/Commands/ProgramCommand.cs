using System;
using System.Collections.Generic;
using System.IO;

namespace XstarS.GuidGenerators.Commands
{
    internal abstract class ProgramCommand
    {
        protected ProgramCommand() { }

        public static bool Execute(string[] args)
        {
            IEnumerable<ProgramCommand> GetCommandChain()
            {
                yield return NewNoNameGuidCommand.Default;
                yield return NewNoNameGuidCommand.Version1;
                yield return NewNoNameGuidCommand.Version2;
                yield return NewNameBasedGuidCommand.Version3;
                yield return NewNoNameGuidCommand.Version4;
                yield return NewNameBasedGuidCommand.Version5;
                yield return ShowHelpCommand.Instance;
                yield return InvalidSyntaxCommand.Instance;
            }

            foreach (var command in GetCommandChain())
            {
                if (command.TryExecute(args))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SupportPathExt =>
            Environment.OSVersion.Platform < PlatformID.Unix;

        protected string GetCommandName()
        {
            var cmdPath = Environment.GetCommandLineArgs()[0];
            var cmdName = Path.GetFileNameWithoutExtension(cmdPath);
            var cmdExt = Path.GetExtension(cmdPath);
            if (this.SupportPathExt) { cmdExt = $"[{cmdExt}]"; }
            if (cmdExt.Length != 0) { cmdName += cmdExt; }
            return cmdName;
        }

        public abstract bool TryExecute(string[] args);
    }
}
