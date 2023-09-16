#if !UUIDREV_DISABLE
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Collections.Concurrent;

namespace XNetEx.Guids.Generators;

internal sealed class GuidGeneratorPool : IGuidGenerator, IDisposable
{
    private readonly Func<IBlockingGuidGenerator> GeneratorFactory;

    private readonly BoundedCollection<IBlockingGuidGenerator> Generators;

    private volatile IBlockingGuidGenerator? DefaultGeneratorValue;

    private volatile int DisposeState;

    internal GuidGeneratorPool(Func<IBlockingGuidGenerator> factory, int capacity = -1)
    {
        this.GeneratorFactory = factory ??
            throw new ArgumentNullException(nameof(factory));
        this.Generators = new BoundedCollection<IBlockingGuidGenerator>(
            (capacity == -1) ? int.MaxValue : (capacity + ~(capacity >> 31)));
        this.DefaultGeneratorValue = null;
        this.DisposeState = 0;
    }

    public GuidVersion Version => this.DefaultGenerator.Version;

    public GuidVariant Variant => this.DefaultGenerator.Variant;

    private IBlockingGuidGenerator DefaultGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            IBlockingGuidGenerator Initialize()
            {
                lock (this.Generators)
                {
                    if (this.DisposeState != 0)
                    {
                        throw new ObjectDisposedException(nameof(GuidGeneratorPool));
                    }

                    return this.DefaultGeneratorValue ??= this.CreateGenerator();
                }
            }

            return this.DefaultGeneratorValue ?? Initialize();
        }
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref this.DisposeState, 1, 0) == 0)
        {
            this.DisposeGenerators();
            this.DisposeState = 2;
        }
    }

    public Guid NewGuid()
    {
        if (this.DefaultGenerator.TryNewGuid(out var guid))
        {
            return guid;
        }
        if (this.TryNewGuidByPool(out guid))
        {
            return guid;
        }

        var generator = this.CreateGenerator();
        try
        {
            return generator.NewGuid();
        }
        finally
        {
            this.ReturnGenerator(generator);
        }
    }

    private bool TryNewGuidByPool(out Guid result)
    {
        if (this.Generators.TryTake(out var generator))
        {
            try
            {
                if (generator.TryNewGuid(out result))
                {
                    return true;
                }
            }
            finally
            {
                this.ReturnGenerator(generator);
            }
        }
        result = default(Guid);
        return false;
    }

    private IBlockingGuidGenerator CreateGenerator()
    {
        return this.GeneratorFactory.Invoke() ??
            throw new InvalidOperationException(
                "The GUID generator factory returns null.");
    }

    private void ReturnGenerator(IBlockingGuidGenerator generator)
    {
        if (this.DisposeState != 0)
        {
            generator.Dispose();
            return;
        }
        if (!this.Generators.TryAdd(generator))
        {
            generator.Dispose();
        }
    }

    private void DisposeGenerators()
    {
        var generators = this.Generators;
        while (generators.TryTake(out var generator))
        {
            generator.Dispose();
        }
        this.DefaultGeneratorValue?.Dispose();
        this.DefaultGeneratorValue = null;
    }
}
#endif
