using XstarS.GuidGen.Commands;

namespace XstarS.GuidGen;

internal static class Program
{
    internal static int Main(string[] args)
    {
        var result = ProgramCommand.Execute(args);
        return result ? 0 : 1;
    }
}
