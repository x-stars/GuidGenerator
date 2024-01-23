using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using XNetEx.Threading;

namespace XNetEx.Guids.Generators;

internal abstract class LocalIdProvider
{
    private static volatile LocalIdProvider? Singleton;

    private readonly AutoRefreshCache<int> LocalUserIdCache;

    private readonly AutoRefreshCache<int> LocalGroupIdCache;

    private LocalIdProvider()
    {
        const int sleepAfter = 10;
        this.LocalUserIdCache = new AutoRefreshCache<int>(
            this.GetLocalUserId, this.RefreshPeriod, sleepAfter);
        this.LocalGroupIdCache = new AutoRefreshCache<int>(
            this.GetLocalGroupId, this.RefreshPeriod, sleepAfter);
    }

    internal static LocalIdProvider Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            static LocalIdProvider Initialize()
            {
                return LocalIdProvider.Singleton ??=
                    LocalIdProvider.Create();
            }

            return LocalIdProvider.Singleton ?? Initialize();
        }
    }

    public int LocalUserId => this.LocalUserIdCache.Value;

    public int LocalGroupId => this.LocalGroupIdCache.Value;

    protected virtual int RefreshPeriod => 1 * 1000;

    private static LocalIdProvider Create()
    {
        return Environment.OSVersion.Platform switch
        {
#if NET5_0_OR_GREATER
            PlatformID.Win32NT => OperatingSystem.IsWindows() ?
                new LocalIdProvider.Windows() : new LocalIdProvider.Unknown(),
#else
            PlatformID.Win32NT => new LocalIdProvider.Windows(),
#endif
            PlatformID.Unix => new LocalIdProvider.UnixLike(),
            PlatformID.MacOSX => new LocalIdProvider.UnixLike(),
            _ => new LocalIdProvider.Unknown(),
        };
    }

    protected abstract int GetLocalUserId();

    protected abstract int GetLocalGroupId();

#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    private sealed class Windows : LocalIdProvider
    {
        private static readonly WellKnownSidType[] SidTypes =
            Windows.CreateSidTypes();

        internal Windows() { }

        private static WellKnownSidType[] CreateSidTypes()
        {
            const int bound = 256;
            var sidTypes = new WellKnownSidType[bound];
            foreach (var value in ..bound)
            {
                sidTypes[value] = (WellKnownSidType)value;
            }
            return sidTypes;
        }

        protected override int GetLocalUserId()
        {
            var userSid = WindowsIdentity.GetCurrent().User;
            return this.GetLocalIdFromSid(userSid);
        }

        protected override int GetLocalGroupId()
        {
            var groupSid = this.GetFirstGroupSid();
            return this.GetLocalIdFromSid(groupSid);
        }

        private unsafe int GetLocalIdFromSid(SecurityIdentifier? sid)
        {
            if (sid is null) { return 0; }
            var sidBytes = new byte[sid.BinaryLength];
            sid.GetBinaryForm(sidBytes, 0);
#if UNSAFE_HELPERS || NETCOREAPP3_0_OR_GREATER
            return Unsafe.ReadUnaligned<int>(ref sidBytes[^4]);
#elif NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return MemoryMarshal.Read<int>(
                ((ReadOnlySpan<byte>)sidBytes)[^4..]);
#else
            fixed (byte* pLastSubAuth = &sidBytes[^4])
            {
                return *(int*)pLastSubAuth;
            }
#endif
        }

        private SecurityIdentifier? GetFirstGroupSid()
        {
            var groupIdRefs = WindowsIdentity.GetCurrent().Groups;
            if (groupIdRefs is null) { return null; }
            var anyGroupSid = default(SecurityIdentifier);
            var normalGroupSid = default(SecurityIdentifier);
            foreach (var groupIdRef in groupIdRefs)
            {
                if (groupIdRef is SecurityIdentifier groupSid)
                {
                    anyGroupSid ??= groupSid;
                    if (!this.IsWellKnownSid(groupSid))
                    {
                        normalGroupSid ??= groupSid;
                    }
                }
            }
            return normalGroupSid ?? anyGroupSid;
        }

        private bool IsWellKnownSid(SecurityIdentifier sid)
        {
            if (sid.BinaryLength <= 16)
            {
                return true;
            }
            var sidTypes = Windows.SidTypes;
            foreach (var sidType in sidTypes)
            {
                if (sid.IsWellKnown(sidType))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private sealed class UnixLike : LocalIdProvider
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("libc", EntryPoint = "getuid")]
            internal static extern uint GetUserId();

            [DllImport("libc", EntryPoint = "getgid")]
            internal static extern uint GetGroupId();
        }

        internal UnixLike() { }

        protected override int GetLocalUserId()
        {
            return (int)SafeNativeMethods.GetUserId();
        }

        protected override int GetLocalGroupId()
        {
            return (int)SafeNativeMethods.GetGroupId();
        }
    }

    private sealed class Unknown : LocalIdProvider
    {
        internal Unknown() { }

        protected override int RefreshPeriod => Timeout.Infinite;

        protected override int GetLocalUserId()
        {
            throw new PlatformNotSupportedException(
                "The current operating system does not support " +
                "getting the local user ID.");
        }

        protected override int GetLocalGroupId()
        {
            throw new PlatformNotSupportedException(
                "The current operating system does not support " +
                "getting the local group ID.");
        }
    }
}
