using System;
using System.IO;
using XNetEx.Guids.Generators;
using XstarS.GuidGen.Commands;

namespace XstarS.GuidGen;

internal static class Program
{
    internal static int Main(string[] args)
    {
        try
        {
            Program.ConfigureStateStorage();
            var result = ProgramCommand.Execute(args);
            return result ? 0 : 1;
        }
        catch (PlatformNotSupportedException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 2;
        }
    }

    private static void ConfigureStateStorage()
    {
        GuidGenerator.StateStorageException += (sender, e) =>
        {
            if ((e.OperationType != FileAccess.Read) ||
                (e.Exception is not FileNotFoundException))
            {
                Console.Error.WriteLine(e.Exception);
            }
        };
        var storageDir = Program.GetStateStorageDirectory();
        var storageFile = "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24.state.bin";
        var storagePath = Path.Combine(storageDir, storageFile);
        _ = GuidGenerator.SetStateStorageFile(storagePath);
    }

    private static string GetStateStorageDirectory()
    {
#if ILC_DISABLE_REFLECTION
        static string GetEnvPathOrThrowNotFound(string name) =>
            Environment.GetEnvironmentVariable(name) ??
            throw new DirectoryNotFoundException($"Environment directory not found: {name}");
        var storageDir = 0 switch
        {
            _ when OperatingSystem.IsWindows() =>
                GetEnvPathOrThrowNotFound("LOCALAPPDATA"),
            _ when OperatingSystem.IsLinux() =>
                Environment.GetEnvironmentVariable("XDG_DATA_HOME") ??
                Path.Combine(GetEnvPathOrThrowNotFound("HOME"), ".local", "share"),
            _ when OperatingSystem.IsMacOS() =>
                Path.Combine(GetEnvPathOrThrowNotFound("HOME"), "Library", "Application Support"),
            _ => Environment.GetEnvironmentVariable("HOME") ??
                throw new PlatformNotSupportedException(
                    "Unable to get the environment directory on an unknown operating system."),
        };
        return Directory.CreateDirectory(storageDir).FullName;
#else
        return Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.Create);
#endif
    }
}
