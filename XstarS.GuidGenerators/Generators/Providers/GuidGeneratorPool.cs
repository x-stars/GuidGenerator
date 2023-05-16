#if !FEATURE_DISABLE_UUIDREV
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace XNetEx.Guids.Generators;

internal sealed class GuidGeneratorPool : IGuidGenerator, IDisposable
{
    private readonly Func<IBlockingGuidGenerator> GeneratorFactory;

    private readonly BlockingCollection<IBlockingGuidGenerator> Generators;

    private volatile IBlockingGuidGenerator? DefaultGeneratorValue;

    private volatile bool IsDisposed;

    internal GuidGeneratorPool(Func<IBlockingGuidGenerator> factory, int capacity = -1)
    {
        this.GeneratorFactory = factory ??
            throw new ArgumentNullException(nameof(factory));
        this.Generators = (capacity == -1) ?
            new BlockingCollection<IBlockingGuidGenerator>() :
            new BlockingCollection<IBlockingGuidGenerator>(capacity);
        this.DefaultGeneratorValue = null;
        this.IsDisposed = false;
    }

    public GuidVersion Version => this.DefaultGenerator.Version;

    public GuidVariant Variant => this.DefaultGenerator.Variant;

    private IBlockingGuidGenerator DefaultGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            IBlockingGuidGenerator Initialize()
            {
                return this.DefaultGeneratorValue ??= this.CreateGenerator();
            }

            return this.DefaultGeneratorValue ?? Initialize();
        }
    }

    public void Dispose()
    {
        if (this.IsDisposed) { return; }
        lock (this.Generators)
        {
            if (this.IsDisposed) { return; }
            var generators = this.Generators;
            generators.CompleteAdding();
            while (generators.TryTake(out var generator))
            {
                generator.Dispose();
            }
            generators.Dispose();
            this.DefaultGenerator.Dispose();
            this.IsDisposed = true;
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
            if (!this.Generators.TryAdd(generator))
            {
                generator.Dispose();
            }
        }
    }

    private bool TryNewGuidByPool(out Guid result)
    {
        var generators = this.Generators;
        if (generators.TryTake(out var generator))
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
                if (!generators.TryAdd(generator))
                {
                    generator.Dispose();
                }
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
}
#endif
