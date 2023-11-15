#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
using System;
using System.Security.Cryptography;

namespace XNetEx.Security.Cryptography;

internal abstract class Shake128D : HashAlgorithm
{
    protected Shake128D(int hashSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(hashSize);
        Shake128D.CheckHashSizeInBytes(hashSize);
        this.HashSizeValue = hashSize;
    }

    public static Shake128D Create(int hashSize)
    {
        return new Shake128D.Implementation(hashSize);
    }

    public static Shake128D Create256()
    {
        return Shake128D.Create(hashSize: 256);
    }

    private static void CheckHashSizeInBytes(int hashSize)
    {
        if (hashSize % 8 != 0)
        {
            throw new ArgumentException(
                "The algorithm's hash size must be multiple of 8.",
                nameof(hashSize));
        }
    }

    private sealed class Implementation : Shake128D
    {
        private readonly Shake128 HashProvider;

        internal Implementation(int hashSize) : base(hashSize)
        {
            this.HashProvider = new Shake128();
        }

        public override void Initialize()
        {
            this.HashProvider.GetHashAndReset(Span<byte>.Empty);
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.HashCore(new ReadOnlySpan<byte>(array, ibStart, cbSize));
        }

        protected override void HashCore(ReadOnlySpan<byte> source)
        {
            this.HashProvider.AppendData(source);
        }

        protected override byte[] HashFinal()
        {
            var hashSize = this.HashSizeValue / 8;
            return this.HashProvider.GetHashAndReset(hashSize);
        }

        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
        {
            var hashSize = this.HashSizeValue / 8;
            if (destination.Length < hashSize)
            {
                bytesWritten = 0;
                return false;
            }
            this.HashProvider.GetHashAndReset(destination[..hashSize]);
            bytesWritten = hashSize;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.HashProvider.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

internal abstract class Shake256D : HashAlgorithm
{
    protected Shake256D(int hashSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(hashSize);
        Shake256D.CheckHashSizeInBytes(hashSize);
        this.HashSizeValue = hashSize;
    }

    public static Shake256D Create(int hashSize)
    {
        return new Shake256D.Implementation(hashSize);
    }

    public static Shake256D Create512()
    {
        return Shake256D.Create(hashSize: 512);
    }

    private static void CheckHashSizeInBytes(int hashSize)
    {
        if (hashSize % 8 != 0)
        {
            throw new ArgumentException(
                "The algorithm's hash size must be multiple of 8.",
                nameof(hashSize));
        }
    }

    private sealed class Implementation : Shake256D
    {
        private readonly Shake256 HashProvider;

        internal Implementation(int hashSize) : base(hashSize)
        {
            this.HashProvider = new Shake256();
        }

        public override void Initialize()
        {
            this.HashProvider.GetHashAndReset(Span<byte>.Empty);
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.HashCore(new ReadOnlySpan<byte>(array, ibStart, cbSize));
        }

        protected override void HashCore(ReadOnlySpan<byte> source)
        {
            this.HashProvider.AppendData(source);
        }

        protected override byte[] HashFinal()
        {
            var hashSize = this.HashSizeValue / 8;
            return this.HashProvider.GetHashAndReset(hashSize);
        }

        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
        {
            var hashSize = this.HashSizeValue / 8;
            if (destination.Length < hashSize)
            {
                bytesWritten = 0;
                return false;
            }
            this.HashProvider.GetHashAndReset(destination[..hashSize]);
            bytesWritten = hashSize;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.HashProvider.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
#endif
