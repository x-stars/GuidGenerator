using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
[DoNotParallelize]
public class GuidGeneratorStateTest
{
    [TestMethod]
    public void SetStateStorageFile_NonExistingFile_CatchFileNotFoundException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        File.Delete(fileName);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exception, typeof(FileNotFoundException));
    }

    [TestMethod]
    public void SetStateStorageFile_EmptyFile_CatchEndOfStreamException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exception, typeof(EndOfStreamException));
    }

    [TestMethod]
    public void SetStateStorageFile_LockedFile_CatchIOException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        using var fileLock = new FileStream(fileName,
            FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exception, typeof(IOException));
    }

    [TestMethod]
    public void SetStateStorageFile_UnknownVersionNumber_CatchInvalidDataException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        this.WriteStateFieldsToFile(fileName, version: 1234);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exception, typeof(InvalidDataException));
    }

    [TestMethod]
    public void SetStateStorageFile_FileWithMaxTimestamp_GetIncrementClockSeq()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        var inputClockSeq = 42;
        this.WriteStateFieldsToFile(fileName, version: 4122,
            timestamp: long.MaxValue, clockSeq: inputClockSeq);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exception);
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetClockSequence(out var clockSeq);
        Assert.AreEqual((short)(inputClockSeq + 1), clockSeq);
    }

    [TestMethod]
    public void SetStateStorageFile_FileWithClockSeqField_GetClockSeqFromFile()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        this.WriteStateFieldsToFile(fileName, version: 4122, clockSeq: 42);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exception);
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetClockSequence(out var clockSeq);
        Assert.AreEqual(42, clockSeq);
    }

    [TestMethod]
    public void SetStateStorageFile_FileWithPhysicalNodeId_GetDifferentClockSeq()
    {
        var guid0 = GuidGenerator.Version1.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        var inputNodeId = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55 };
        this.WriteStateFieldsToFile(fileName, version: 4122, phyNodeId: inputNodeId);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exception);
        var guid1 = GuidGenerator.Version1.NewGuid();
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        Assert.AreNotEqual(clockSeq0, clockSeq1);
    }

    [TestMethod]
    public void SetStateStorageFile_FileWithRandomNodeId_GetNodeIdFromFile()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        var inputNodeId = new byte[] { 0xFF, 0xEE, 0xDD, 0xCC, 0xBB, 0xAA };
        this.WriteStateFieldsToFile(fileName, version: 4122, randNodeId: inputNodeId);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exception);
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetNodeId(out var nodeId);
        CollectionAssert.AreEqual(inputNodeId, nodeId);
    }

    [TestMethod]
    public void SetStateStorageFile_FileWithNonFieldFlagSet_NotGetAnyField()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        ref var exception = ref this.CatchStateLoadException();
        var inputClockSeq = 42;
        var inputNodeId0 = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55 };
        var inputNodeId1 = new byte[] { 0xFF, 0xEE, 0xDD, 0xCC, 0xBB, 0xAA };
        this.WriteStateFieldsToFile(fileName, version: 4122,
            fieldFlags: 0x00, timestamp: long.MaxValue, clockSeq: inputClockSeq,
            phyNodeId: inputNodeId0, randNodeId: inputNodeId1);
        var loadResult = GuidGenerator.SetStateStorageFile(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exception);
        var guid = GuidGenerator.Version1R.NewGuid();
        _ = guid.TryGetClockSequence(out var clockSeq);
        _ = guid.TryGetNodeId(out var nodeId);
        Assert.AreNotEqual(inputClockSeq, clockSeq);
        CollectionAssert.AreNotEqual(inputNodeId1, nodeId);
    }

    private ref Exception? CatchStateLoadException()
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
        int? clockSeq = null, byte[]? phyNodeId = null, byte[]? randNodeId = null)
    {
        using var stream = new FileStream(fileName, FileMode.Create);
        using var writer = new BinaryWriter(stream);
        fieldFlags ??=
            ((timestamp is null) ? 0x00 : 0x01) |
            ((clockSeq is null) ? 0x00 : 0x02) |
            ((phyNodeId is null) ? 0x00 : 0x04) |
            ((randNodeId is null) ? 0x00 : 0x08);
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
