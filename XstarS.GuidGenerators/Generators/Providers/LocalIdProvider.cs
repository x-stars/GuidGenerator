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
                return Volatile.Read(ref field) ?? Volatile.WriteValue(ref field,
                    LocalIdProvider.Create());
            }

            return Volatile.Read(ref field) ?? Initialize();
        }
    }

    public int LocalUserId => this.LocalUserIdCache.Value;

    public int LocalGroupId => this.LocalGroupIdCache.Value;

    protected virtual int RefreshPeriod => 1 * 1000;

    private static LocalIdProvider Create()
    {
        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT
#if NET5_0_OR_GREATER
            when OperatingSystem.IsWindows()
#endif
              => new LocalIdProvider.Windows(),
            PlatformID.Unix or
            PlatformID.MacOSX
#if NET5_0_OR_GREATER
            when !OperatingSystem.IsWindows()
              && !OperatingSystem.IsBrowser()
#endif
              => new LocalIdProvider.UnixLike(),
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
        internal Windows() { }

        protected override int GetLocalUserId()
        {
            using var token = WindowsIdentity.GetCurrentToken();
            var userSid = WindowsIdentity.GetUserSid(token);
            return this.GetLocalIdFromSid(userSid);
        }

        protected override int GetLocalGroupId()
        {
            using var token = WindowsIdentity.GetCurrentToken();
            var groupSid = WindowsIdentity.GetPrimaryGroupSid(token);
            return this.GetLocalIdFromSid(groupSid);
        }

        private unsafe int GetLocalIdFromSid(byte[]? sidBytes)
        {
            if (sidBytes is null) { return 0; }
            Debug.Assert(sidBytes.Length >= 8);
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
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
        internal UnixLike() { }

        protected override int GetLocalUserId()
        {
            return (int)SafeNativeMethods.GetUserId();
        }

        protected override int GetLocalGroupId()
        {
            return (int)SafeNativeMethods.GetGroupId();
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            [DllImport("libc", EntryPoint = "getuid", ExactSpelling = true)]
            internal static extern uint GetUserId();

            [DllImport("libc", EntryPoint = "getgid", ExactSpelling = true)]
            internal static extern uint GetGroupId();
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
