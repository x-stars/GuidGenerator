using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Internal.Security.Principal;
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
        var platform = Environment.OSVersion.Platform;
        return
#if NET5_0_OR_GREATER
            OperatingSystem.IsWindows() ?
#else
            (platform is PlatformID.Win32NT) ?
#endif
                new LocalIdProvider.Windows() :
#if NET5_0_OR_GREATER
            !OperatingSystem.IsBrowser() &&
#endif
            (platform is PlatformID.Unix or PlatformID.MacOSX) ?
                new LocalIdProvider.UnixLike() :
                new LocalIdProvider.Unknown();
    }

    protected abstract int GetLocalUserId();

    protected abstract int GetLocalGroupId();

#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    private sealed class Windows : LocalIdProvider
    {
        internal Windows() { }

        protected override int GetLocalUserId()
        {
            var token = WindowsIdentity.GetCurrentToken();
            var userSid = WindowsIdentity.GetUserSid(token);
            return this.GetLocalIdFromSid(userSid);
        }

        protected override int GetLocalGroupId()
        {
            var token = WindowsIdentity.GetCurrentToken();
            var groupSid = WindowsIdentity.GetPrimaryGroupSid(token);
            return this.GetLocalIdFromSid(groupSid);
        }

        private unsafe int GetLocalIdFromSid(byte[]? sidBytes)
        {
            if (sidBytes is null) { return 0; }
            Debug.Assert(sidBytes.Length >= 8);
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
    }

#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.UnsupportedOSPlatform("windows")]
    [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
    private sealed class UnixLike : LocalIdProvider
    {
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("libc", EntryPoint = "getuid", ExactSpelling = true)]
            internal static extern uint GetUserId();

            [DllImport("libc", EntryPoint = "getgid", ExactSpelling = true)]
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
