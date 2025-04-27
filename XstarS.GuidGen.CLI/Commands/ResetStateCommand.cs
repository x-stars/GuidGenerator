using XNetEx.Guids.Generators;

namespace XstarS.GuidGen.Commands;

internal sealed class ResetStateCommand : SingleOptionCommand
{
    internal static readonly ResetStateCommand Instance = new();

    private ResetStateCommand() : base("-RS", "-RESET") { }

    protected override void ExecuteCore(string optionArg)
    {
        GuidGenerator.ResetState();
    }
}
