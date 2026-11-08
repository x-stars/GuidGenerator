using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
        _ = GuidGenerator.SetStateStorageFile(null);
    }

    private ref readonly Exception? CatchStateLoadException()
    {
        return ref this.CatchStateLoadExceptionAsBox().Value;
    }

    private StrongBox<Exception?> CatchStateLoadExceptionAsBox()
    {
        var catchBox = new StrongBox<Exception?>();
        GuidGenerator.StateStorageException += (sender, e) =>
        {
            if (e.OperationType == FileAccess.Read)
            {
                catchBox.Value = e.Exception;
            }
        };
        return catchBox;
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

    private sealed class MemoryFile(byte[] data)
    {
        public Stream OpenStream(string fileName, FileAccess fileAccess)
        {
            return fileAccess switch
            {
                FileAccess.Read => new MemoryStream(data, writable: false),
                FileAccess.Write => new MemoryStream(data, writable: true),
                _ => throw new ArgumentOutOfRangeException(nameof(fileAccess)),
            };
        }

        public Stream OpenSingleByteReadStream(string fileName, FileAccess fileAccess)
        {
            return fileAccess switch
            {
                FileAccess.Read => new SingleByteReadMemoryStream(data),
                FileAccess.Write => new MemoryStream(data, writable: true),
                _ => throw new ArgumentOutOfRangeException(nameof(fileAccess)),
            };
        }
    }

    private sealed class SingleByteReadMemoryStream(byte[] data)
        : MemoryStream(data, writable: false)
    {
        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, (count > 1) ? 1 : count);
        }

        public override Task<int> ReadAsync(
            byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return base.ReadAsync(
                buffer, offset, (count > 1) ? 1 : count, cancellationToken);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public override int Read(Span<byte> buffer)
        {
            return base.Read(buffer[..((buffer.Length > 1) ? 1 : buffer.Length)]);
        }

        public override ValueTask<int> ReadAsync(
            Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return base.ReadAsync(
                buffer[..((buffer.Length > 1) ? 1 : buffer.Length)], cancellationToken);
        }
#endif
    }
}
