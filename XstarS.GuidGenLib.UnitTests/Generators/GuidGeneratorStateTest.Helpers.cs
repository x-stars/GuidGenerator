using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorStateTest
{
    [TestCleanup]
    public void ResetGeneratorState()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        this.WriteStateFieldsToFile(fileName, version: 4122);
        _ = GuidGenerator.SetStateStorageFile(fileName);
    }

    private ref readonly Exception? CatchStateLoadException()
    {
        var catchBox = new StrongBox<Exception?>();
        GuidGenerator.StateStorageException += (sender, e) =>
        {
            if (e.OperationType == FileAccess.Read)
            {
                catchBox.Value = e.Exception;
            }
        };
        return ref catchBox.Value;
    }

    private IDisposable CreateTempFile(out string fileName)
    {
        var tempFile = Path.GetTempFileName();
        fileName = tempFile;
        return new DisposeAction(_ =>
        {
            if (File.Exists(tempFile))
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        });
    }

    private void WriteStateFieldsToFile(string fileName,
        int version, int? fieldFlags = null, long? timestamp = null,
        int? clockSeq = null, short? phyClkSeq = null, short? randClkSeq = null,
        byte[]? phyNodeId = null, byte[]? randNodeId = null)
    {
        using var stream = new FileStream(fileName, FileMode.Create);
        using var writer = new BinaryWriter(stream);
        fieldFlags ??=
            ((timestamp is null) ? 0x00 : 0x01) |
            ((clockSeq is null) ? 0x00 : 0x02) |
            ((phyNodeId is null) ? 0x00 : 0x04) |
            ((randNodeId is null) ? 0x00 : 0x08) |
            (((phyClkSeq is null) ? 0x00 : 0x01) << (2 * 8)) |
            (((randClkSeq is null) ? 0x00 : 0x02) << (2 * 8));
        if ((phyClkSeq is not null) || (randClkSeq is not null))
        {
            clockSeq = (int)(
                ((ushort)(phyClkSeq ?? 0) << (0 * 8)) |
                ((ushort)(randClkSeq ?? 0) << (2 * 8)));
        }
        phyNodeId ??= new byte[6];
        randNodeId ??= new byte[6];
        writer.Write(version);
        writer.Write((int)fieldFlags);
        writer.Write(timestamp ?? 0);
        writer.Write(clockSeq ?? 0);
        writer.Write(phyNodeId, 0, 6);
        writer.Write(randNodeId, 0, 6);
    }
}
