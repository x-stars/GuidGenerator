using System;
using System.Collections.Generic;
using System.IO;

namespace XstarS.GuidGen.Commands;

internal abstract class ProgramCommand
{
    protected ProgramCommand() { }

    private bool SupportPathExt =>
        Environment.OSVersion.Platform < PlatformID.Unix;

    public static bool Execute(string[] args)
    {
        static IEnumerable<ProgramCommand> GetCommandChain()
        {
            yield return NewNoInputGuidCommand.Default;
            yield return NewNoInputGuidCommand.Version1;
            yield return NewDceSecurityGuidCommand.Version2;
            yield return NewNameBasedGuidCommand.Version3;
            yield return NewNoInputGuidCommand.Version4;
            yield return NewNameBasedGuidCommand.Version5;
            yield return NewNoInputGuidCommand.Version1R;
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

    public abstract bool TryExecute(string[] args);

    protected string GetCommandName()
    {
        var cmdPath = Environment.GetCommandLineArgs()[0];
        var cmdName = Path.GetFileNameWithoutExtension(cmdPath);
        var cmdExt = Path.GetExtension(cmdPath);
        if (this.SupportPathExt) { cmdExt = $"[{cmdExt}]"; }
        if (cmdExt.Length != 0) { cmdName += cmdExt; }
        return cmdName;
    }
}
