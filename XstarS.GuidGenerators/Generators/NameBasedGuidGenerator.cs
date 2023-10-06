﻿using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;

namespace XNetEx.Guids.Generators;

internal abstract partial class NameBasedGuidGenerator : GuidGenerator, INameBasedGuidGenerator
{
    private readonly ThreadLocal<HashAlgorithm?> LocalHashing;

#if !UUIDREV_DISABLE
    private readonly Guid? HashspaceId;
#endif

#if !UUIDREV_DISABLE
    protected NameBasedGuidGenerator() : this(hashspaceId: null) { }

    protected NameBasedGuidGenerator(Guid? hashspaceId)
    {
        this.LocalHashing = new ThreadLocal<HashAlgorithm?>(
            this.CreateHashing, trackAllValues: true);
        this.HashspaceId = hashspaceId;
    }
#else
    protected NameBasedGuidGenerator()
    {
        this.LocalHashing = new ThreadLocal<HashAlgorithm?>(
            this.CreateHashing, trackAllValues: true);
    }
#endif

    protected virtual bool TrackHashing => false;

    public sealed override Guid NewGuid()
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return this.NewGuid(Guid.Empty, ReadOnlySpan<byte>.Empty);
#else
        return this.NewGuid(Guid.Empty, Array.Empty<byte>());
#endif
    }

    public sealed override Guid NewGuid(Guid nsId, byte[] name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return this.NewGuid(nsId, (ReadOnlySpan<byte>)name);
#else
        var input = this.CreateInput(nsId, name);
        var hash = this.ComputeHash(input);
        return this.HashToGuid(hash);
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public sealed override Guid NewGuid(Guid nsId, ReadOnlySpan<byte> name)
    {
        return this.ComputeHashToGuid(nsId, name);
    }
#endif

    protected abstract HashAlgorithm CreateHashing();

    protected virtual HashAlgorithm GetHashing()
    {
        var hashing = this.LocalHashing.Value!;
        if (this.TrackHashing)
        {
            this.LocalHashing.Value = null;
        }
        return hashing;
    }

    protected virtual void ReturnHashing(HashAlgorithm hashing)
    {
        if (this.TrackHashing)
        {
            this.LocalHashing.Value = hashing;
        }
    }

#if !UUIDREV_DISABLE
    protected void DisposeHashings()
    {
        var hashings = this.LocalHashing.Values;
        foreach (var hashing in hashings)
        {
            hashing?.Dispose();
        }
        this.LocalHashing.Dispose();
    }
#endif

    internal sealed class MD5Hashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.MD5Hashing? Singleton;

        private MD5Hashing() { }

        internal static NameBasedGuidGenerator.MD5Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.MD5Hashing Initialize()
                {
                    return NameBasedGuidGenerator.MD5Hashing.Singleton ??=
                        new NameBasedGuidGenerator.MD5Hashing();
                }

                return NameBasedGuidGenerator.MD5Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version3;

        protected override HashAlgorithm CreateHashing() => MD5.Create();
    }

    internal sealed class SHA1Hashing : NameBasedGuidGenerator
    {
        private static volatile NameBasedGuidGenerator.SHA1Hashing? Singleton;

        private SHA1Hashing() { }

        internal static NameBasedGuidGenerator.SHA1Hashing Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                static NameBasedGuidGenerator.SHA1Hashing Initialize()
                {
                    return NameBasedGuidGenerator.SHA1Hashing.Singleton ??=
                        new NameBasedGuidGenerator.SHA1Hashing();
                }

                return NameBasedGuidGenerator.SHA1Hashing.Singleton ?? Initialize();
            }
        }

        public override GuidVersion Version => GuidVersion.Version5;

        protected override HashAlgorithm CreateHashing() => SHA1.Create();
    }
}
