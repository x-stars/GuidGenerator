﻿#if !UUIDREV_DISABLE
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XNetEx.Collections.Concurrent;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

internal sealed class GuidGeneratorPool : IGuidGenerator, IDisposable
{
    private readonly Func<IBlockingGuidGenerator> GeneratorFactory;

    private readonly BoundedCollection<IBlockingGuidGenerator> Generators;

    private readonly ThreadLocal<IBlockingGuidGenerator>? LocalDefaultGenerator;

    private volatile IBlockingGuidGenerator? GlobalDefaultGenerator;

    private volatile int DisposeState;

    internal GuidGeneratorPool(Func<IBlockingGuidGenerator> factory, int capacity = -1)
    {
        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }
        if ((capacity <= 0) && (capacity != -1))
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity), "Value must be either positive or -1.");
        }

        this.GeneratorFactory = factory;
        this.Generators = new BoundedCollection<IBlockingGuidGenerator>(
            (capacity == -1) ? int.MaxValue : (capacity - 1));
        this.LocalDefaultGenerator = (capacity == -1) ?
            new ThreadLocal<IBlockingGuidGenerator>(
                this.CreateGenerator, trackAllValues: true) : null;
        this.GlobalDefaultGenerator = null;
        this.DisposeState = LatchStates.Initial;
    }

    public GuidVersion Version => this.DefaultGenerator.Version;

    public GuidVariant Variant => this.DefaultGenerator.Variant;

    private IBlockingGuidGenerator DefaultGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (this.LocalDefaultGenerator is not null)
            {
                return this.LocalDefaultGenerator.Value!;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            IBlockingGuidGenerator InitializeGlobal()
            {
                lock (this.Generators)
                {
                    if (this.DisposeState != LatchStates.Initial)
                    {
                        throw new ObjectDisposedException(nameof(GuidGeneratorPool));
                    }
                    return this.GlobalDefaultGenerator ??= this.CreateGenerator();
                }
            }

            return this.GlobalDefaultGenerator ?? InitializeGlobal();
        }
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(
            ref this.DisposeState, LatchStates.Entered,
            LatchStates.Initial) == LatchStates.Initial)
        {
            try
            {
                this.DisposeGenerators();
                this.DisposeState = LatchStates.Exited;
            }
            catch (Exception)
            {
                this.DisposeState = LatchStates.Failed;
                throw;
            }
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
        if (this.DisposeState != LatchStates.Initial)
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
        if (this.LocalDefaultGenerator is not null)
        {
            var localGenerators = this.LocalDefaultGenerator.Values;
            foreach (var generator in localGenerators)
            {
                generator.Dispose();
            }
            this.LocalDefaultGenerator.Dispose();
        }
        this.GlobalDefaultGenerator?.Dispose();
        this.GlobalDefaultGenerator = null;
    }
}
#endif
