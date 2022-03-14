using System;
using System.Collections.Generic;

namespace XstarS.GuidGenerators.Commands
{
    internal sealed class InvalidSyntaxCommand : ProgramCommand
    {
        internal static readonly InvalidSyntaxCommand Instance =
            new InvalidSyntaxCommand();

        private InvalidSyntaxCommand() { }

        public override bool TryExecute(string[] args)
        {
            IEnumerable<string> GetErrorMessage()
            {
                var cmdName = this.GetCommandName();
                yield return "The syntax of the command is incorrect.";
                yield return $"Try '{cmdName} -Help' for more information.";
            }

            foreach (var errorLine in GetErrorMessage())
            {
                Console.Error.WriteLine(errorLine);
            }
            return false;
        }
    }
}
