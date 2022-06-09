using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace XstarS.GuidGenerators
{
    internal abstract class LocalIdProvider
    {
        private static class Singleton
        {
            internal static readonly LocalIdProvider Value =
                LocalIdProvider.Singleton.Create();

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
        }

        private LocalIdProvider() { }

        internal static LocalIdProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => LocalIdProvider.Singleton.Value;
        }

        public abstract int GetLocalUserId();

        public abstract int GetLocalGroupId();

#if NET5_0_OR_GREATER
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
        private sealed class Windows : LocalIdProvider
        {
            internal Windows() { }

            public override unsafe int GetLocalUserId()
            {
                var userSid = WindowsIdentity.GetCurrent().User;
                if (userSid is null) { return 0; }
                var sidBytes = new byte[userSid.BinaryLength];
                userSid.GetBinaryForm(sidBytes, 0);
                fixed (byte* pLastSubAuth = &sidBytes[^4])
                {
                    return *(int*)pLastSubAuth;
                }
            }

            public override unsafe int GetLocalGroupId()
            {
                var groupIdRefs = WindowsIdentity.GetCurrent().Groups;
                if (groupIdRefs is null) { return 0; }
                var anyGroupSid = default(SecurityIdentifier);
                var commonGroupSid = default(SecurityIdentifier);
                foreach (var groupIdRef in groupIdRefs)
                {
                    if (groupIdRef is SecurityIdentifier groupSid)
                    {
                        anyGroupSid ??= groupSid;
                        if (!this.IsWellKnownSid(groupSid))
                        {
                            commonGroupSid ??= groupSid;
                        }
                    }
                }
                var finalGroupSid = commonGroupSid ?? anyGroupSid;
                if (finalGroupSid is null) { return 0; }
                var sidBytes = new byte[finalGroupSid.BinaryLength];
                finalGroupSid.GetBinaryForm(sidBytes, 0);
                fixed (byte* pLastSubAuth = &sidBytes[^4])
                {
                    return *(int*)pLastSubAuth;
                }
            }

            private bool IsWellKnownSid(SecurityIdentifier sid)
            {
                if (sid.BinaryLength <= 16)
                {
                    return true;
                }
                var enumValues = Enum.GetValues(typeof(WellKnownSidType));
                var sidTypes = (WellKnownSidType[])enumValues;
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
            private static class SafeNativeMethods
            {
                [DllImport("libc", EntryPoint = "getuid")]
                internal static extern int GetUserId();

                [DllImport("libc", EntryPoint = "getgid")]
                internal static extern int GetGroupId();
            }

            internal UnixLike() { }

            public override int GetLocalUserId()
            {
                return SafeNativeMethods.GetUserId();
            }

            public override int GetLocalGroupId()
            {
                return SafeNativeMethods.GetGroupId();
            }
        }

        private sealed class Unknown : LocalIdProvider
        {
            internal Unknown() { }

            public override int GetLocalUserId()
            {
                throw new PlatformNotSupportedException();
            }

            public override int GetLocalGroupId()
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
