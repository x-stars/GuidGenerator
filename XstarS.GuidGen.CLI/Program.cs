using System;
using System.IO;
using XNetEx.Guids.Generators;
using XstarS.GuidGen.Commands;

namespace XstarS.GuidGen;

internal static class Program
{
    internal static int Main(string[] args)
    {
        Program.ConfigureStateStorage();
        var result = ProgramCommand.Execute(args);
        return result ? 0 : 1;
    }

    private static void ConfigureStateStorage()
    {
        GuidGeneratorState.StorageException += (sender, e) =>
        {
            if ((e.OperationType != FileAccess.Read) ||
                (e.Exception is not FileNotFoundException))
            {
                Console.Error.WriteLine(e.Exception);
            }
        };
        var storageDir = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.Create);
        var storageFile = "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24.state.bin";
        var storagePath = Path.Combine(storageDir, storageFile);
        _ = GuidGeneratorState.SetStorageFilePath(storagePath);
    }
}
