using System;

namespace XstarS.GuidGenerators.Commands
{
    internal sealed class InvalidSyntaxCommand : ProgramCommand
    {
        internal static readonly InvalidSyntaxCommand Instance =
            new InvalidSyntaxCommand();

        private InvalidSyntaxCommand() { }

        public override bool TryExecute(string[] args)
        {
            var cmdName = this.GetCommandName();
            var errorLines = new[]
            {
                "The syntax of the command is incorrect.",
                $"Try '{cmdName} -Help' for more information.",
            };
            foreach (var errorLine in errorLines)
            {
                Console.Error.WriteLine(errorLine);
            }
            return false;
        }
    }
}
