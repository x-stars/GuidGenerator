using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XNetEx.Guids;
using XNetEx.Guids.Generators;
using HResult = System.Int32;

namespace XstarS.GuidGen;

internal static unsafe class NativeLib
{
    static NativeLib() { ConfigureStateStorage(); }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV1))]
    internal static HResult GuidCreateV1([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version1, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV1R))]
    internal static HResult GuidCreateV1R([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version1R, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV2))]
    internal static HResult GuidCreateV2([Out] Guid* guid, DceSecurityDomain domain)
    {
        return GuidCreateDceSecurity(GuidGenerator.Version2, guid, domain);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV2Org))]
    internal static HResult GuidCreateV2Org([Out] Guid* guid, uint localId)
    {
        return GuidCreateDceSecurity(
            GuidGenerator.Version2, guid, DceSecurityDomain.Org, localId);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV2Other))]
    internal static HResult GuidCreateV2Other(
        [Out] Guid* guid, DceSecurityDomain domain, uint localId)
    {
        return GuidCreateDceSecurity(GuidGenerator.Version2, guid, domain, localId);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV3))]
    internal static HResult GuidCreateV3(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version3, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV4))]
    internal static HResult GuidCreateV4([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version4, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV5))]
    internal static HResult GuidCreateV5(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version5, guid, nsId, name, nameLen);
    }

#if !UUIDREV_DISABLE
    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV6))]
    internal static HResult GuidCreateV6([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version6, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV6P))]
    internal static HResult GuidCreateV6P([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version6P, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV6R))]
    internal static HResult GuidCreateV6R([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version6R, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV7))]
    internal static HResult GuidCreateV7([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version7, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8))]
    internal static HResult GuidCreateV8([Out] Guid* guid)
    {
        return GuidCreate(GuidGenerator.Version8, guid);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha256))]
    internal static HResult GuidCreateV8NSha256(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha256, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha384))]
    internal static HResult GuidCreateV8NSha384(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha384, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha512))]
    internal static HResult GuidCreateV8NSha512(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha512, guid, nsId, name, nameLen);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static HResult GuidCreate(IGuidGenerator guidGen, Guid* guid)
    {
        try
        {
            *guid = guidGen.NewGuid();
            return default(HResult);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return ex.HResult;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static HResult GuidCreateNameBased(
        INameBasedGuidGenerator guidGen,
        Guid* guid, Guid* nsId, byte* name, nuint nameLen)
    {
        try
        {
            *guid = guidGen.NewGuid(*nsId,
                new ReadOnlySpan<byte>(name, checked((int)nameLen)));
            return default(HResult);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return ex.HResult;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static HResult GuidCreateDceSecurity(
        IDceSecurityGuidGenerator guidGen,
        Guid* guid, DceSecurityDomain domain, uint? localId = null)
    {
        try
        {
            *guid = guidGen.NewGuid(domain,
                Unsafe.As<uint?, int?>(ref localId));
            return default(HResult);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return ex.HResult;
        }
    }

    private static void ConfigureStateStorage()
    {
        GuidGenerator.StateStorageException += (sender, e) =>
        {
            if ((e.OperationType != FileAccess.Read) ||
                (e.Exception is not FileNotFoundException))
            {
                Trace.WriteLine(e.Exception);
            }
        };
        var storageDir = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.Create);
        var storageFile = "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24.state.bin";
        var storagePath = Path.Combine(storageDir, storageFile);
        _ = GuidGenerator.SetStateStorageFile(storagePath);
    }
}
