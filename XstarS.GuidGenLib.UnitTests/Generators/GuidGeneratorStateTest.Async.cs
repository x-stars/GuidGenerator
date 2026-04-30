using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

partial class GuidGeneratorStateTest
{
    [TestMethod]
    public async Task SetStateStorageFileAsync_NonExistingFile_CatchFileNotFoundException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        File.Delete(fileName);
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exceptionBox.Value, typeof(FileNotFoundException));
    }

    [TestMethod]
    public async Task SetStateStorageFileAsync_EmptyFile_CatchEndOfStreamException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exceptionBox.Value, typeof(EndOfStreamException));
    }

    [TestMethod]
    public async Task SetStateStorageFileAsync_LockedFile_CatchIOException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        using var fileLock = new FileStream(fileName,
            FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exceptionBox.Value, typeof(IOException));
    }

    [TestMethod]
    public async Task SetStateStorageFileAsync_UnknownVersionNumber_CatchInvalidDataException()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        this.WriteStateFieldsToFile(fileName, version: 1234);
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(fileName);
        Assert.IsFalse(loadResult);
        Assert.IsInstanceOfType(exceptionBox.Value, typeof(InvalidDataException));
    }

    [TestMethod]
    public async Task SetStateStorageFileAsync_FileWithBothClockSeqFields_GetClockSeqFromFile()
    {
        using var tempFile = this.CreateTempFile(out var fileName);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        this.WriteStateFieldsToFile(fileName, version: 4122, phyClkSeq: 42, randClkSeq: 44);
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(fileName);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exceptionBox.Value);
        var guid0 = GuidGenerator.Version1.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        Assert.AreEqual(42, clockSeq0);
#if !UUIDREV_DISABLE
        var guid1 = GuidGenerator.Version6R.NewGuid();
#else
        var guid1 = GuidGenerator.Version1R.NewGuid();
#endif
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        Assert.AreEqual(44, clockSeq1);
    }

    [TestMethod]
    public async Task SetStateStorageFileAsync_MemoryFileWithBothClockSeqFields_GetClockSeqFromFile()
    {
        var data = (byte[])[];
        using (var tempFile = this.CreateTempFile(out var fileName))
        {
            this.WriteStateFieldsToFile(fileName, version: 4122, phyClkSeq: 42, randClkSeq: 44);
            data = File.ReadAllBytes(fileName);
        }
        var memoryFile = new MemoryFile(data);
        var exceptionBox = this.CatchStateLoadExceptionAsBox();
        var loadResult = await GuidGenerator.SetStateStorageFileAsync(string.Empty, memoryFile.OpenStream);
        Assert.IsTrue(loadResult);
        Assert.IsNull(exceptionBox.Value);
        var guid0 = GuidGenerator.Version1.NewGuid();
        _ = guid0.TryGetClockSequence(out var clockSeq0);
        Assert.AreEqual(42, clockSeq0);
#if !UUIDREV_DISABLE
        var guid1 = GuidGenerator.Version6R.NewGuid();
#else
        var guid1 = GuidGenerator.Version1R.NewGuid();
#endif
        _ = guid1.TryGetClockSequence(out var clockSeq1);
        Assert.AreEqual(44, clockSeq1);
    }
}
