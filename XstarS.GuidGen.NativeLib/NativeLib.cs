﻿using System;
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
    static NativeLib()
    {
        NativeLib.ConfigureStateStorage();
    }

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

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha3D256))]
    internal static HResult GuidCreateV8NSha3D256(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha3D256, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha3D384))]
    internal static HResult GuidCreateV8NSha3D384(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha3D384, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NSha3D512))]
    internal static HResult GuidCreateV8NSha3D512(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NSha3D512, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NShake128))]
    internal static HResult GuidCreateV8NShake128(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NShake128, guid, nsId, name, nameLen);
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(GuidCreateV8NShake256))]
    internal static HResult GuidCreateV8NShake256(
        [Out] Guid* guid, [In] Guid* nsId, [In] byte* name, nuint nameLen)
    {
        return GuidCreateNameBased(GuidGenerator.Version8NShake256, guid, nsId, name, nameLen);
    }
#endif

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
        var storageDir = NativeLib.GetStateStorageDirectory();
        var storageFile = "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24.state.bin";
        var storagePath = Path.Combine(storageDir, storageFile);
        _ = GuidGenerator.SetStateStorageFile(storagePath);
    }

    private static string GetStateStorageDirectory()
    {
#if ILC_DISABLE_REFLECTION
        static string GetEnvPathOrThrowNotFound(string name) =>
            Environment.GetEnvironmentVariable(name) ??
            throw new DirectoryNotFoundException($"Environment directory not found: {name}");
        var storageDir = 0 switch
        {
            _ when OperatingSystem.IsWindows() =>
                GetEnvPathOrThrowNotFound("LOCALAPPDATA"),
            _ when OperatingSystem.IsLinux() =>
                Environment.GetEnvironmentVariable("XDG_DATA_HOME") ??
                Path.Combine(GetEnvPathOrThrowNotFound("HOME"), ".local", "share"),
            _ when OperatingSystem.IsMacOS() =>
                Path.Combine(GetEnvPathOrThrowNotFound("HOME"), "Library", "Application Support"),
            _ => Environment.GetEnvironmentVariable("HOME") ??
                throw new PlatformNotSupportedException(
                    "Unable to get the environment directory on an unknown operating system."),
        };
        return Directory.CreateDirectory(storageDir).FullName;
#else
        return Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData,
            Environment.SpecialFolderOption.Create);
#endif
    }
}
