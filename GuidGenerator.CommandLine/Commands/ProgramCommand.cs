using System;
using System.IO;

namespace XstarS.GuidGenerators.Commands
{
    internal abstract class ProgramCommand
    {
        protected ProgramCommand() { }

        public static bool Execute(string[] args)
        {
            var commands = new ProgramCommand[]
            {
                NewNoNameGuidCommand.Default,
                NewNoNameGuidCommand.Version1,
                NewNoNameGuidCommand.Version2,
                NewNameBasedGuidCommand.Version3,
                NewNoNameGuidCommand.Version4,
                NewNameBasedGuidCommand.Version5,
                ShowHelpCommand.Instance,
                InvalidSyntaxCommand.Instance,
            };
            foreach (var command in commands)
            {
                if (command.TryExecute(args))
                {
                    return true;
                }
            }
            return false;
        }

        protected string GetCommandName()
        {
            var cmdPath = Environment.GetCommandLineArgs()[0];
            var cmdName = Path.GetFileNameWithoutExtension(cmdPath);
            var cmdExt = Path.GetExtension(cmdPath);
            if (cmdExt.Length != 0) { cmdName += $"[{cmdExt}]"; }
            return cmdName;
        }

        public abstract bool TryExecute(string[] args);
    }
}
