using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XstarS.GuidGenerators
{
    internal abstract class DceSecurityGuidGenerator : TimeBasedGuidGenerator
    {
        private static readonly byte LocalIPAddressLastByte =
            DceSecurityGuidGenerator.GetLocalIPAddressLastByte();

        private DceSecurityGuidGenerator() { }

        internal static new DceSecurityGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => DceSecurityGuidGenerator.IsSupportedWindows() ?
                   DceSecurityGuidGenerator.WindowsUID.Instance :
                   DceSecurityGuidGenerator.IsSupportedUnixLike() ?
                   DceSecurityGuidGenerator.UnixLikeUID.Instance :
                   DceSecurityGuidGenerator.UnknownUID.Instance;
        }

        private static bool IsSupportedWindows()
        {
            var platform = Environment.OSVersion.Platform;
            return platform == PlatformID.Win32NT;
        }

        private static bool IsSupportedUnixLike()
        {
            var platform = Environment.OSVersion.Platform;
            return platform == PlatformID.Unix ||
                   platform == PlatformID.MacOSX;
        }

        public override GuidVersion Version => GuidVersion.Version2;

        private static byte GetLocalIPAddressLastByte()
        {
            var target = default(NetworkInterface);
            var ifaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in ifaces)
            {
                if (iface.OperationalStatus == OperationalStatus.Up)
                {
                    target = iface;
                    break;
                }
            }
            if (target is null) { return (byte)0; }
            var ipAddrs = target.GetIPProperties().UnicastAddresses;
            if (ipAddrs.Count == 0) { return (byte)0; }
            var ipAddrBytes = ipAddrs[0].Address.GetAddressBytes();
            return ipAddrBytes[^1];
        }

        public override Guid NewGuid()
        {
            var guid = base.NewGuid();
            var guidBytes = guid.ToByteArray();
            this.FillUserIDFields(guidBytes);
            guidBytes[8] = guidBytes[9];
            this.FillVariantField(guidBytes);
            this.FillSiteIDField(guidBytes);
            return new Guid(guidBytes);
        }

        protected abstract int GetUserID();

        private void FillUserIDFields(byte[] guidBytes)
        {
            var userID = this.GetUserID();
            if (BitConverter.IsLittleEndian)
            {
                unsafe
                {
                    fixed (byte* pGuidBytes = &guidBytes[0])
                    {
                        *(int*)pGuidBytes = userID;
                    }
                }
            }
            else
            {
                foreach (var index in 0..4)
                {
                    var shifted = userID >> (index * 8);
                    guidBytes[index] = (byte)shifted;
                }
            }
        }

        private void FillSiteIDField(byte[] guidBytes)
        {
            guidBytes[9] = DceSecurityGuidGenerator.LocalIPAddressLastByte;
        }

        private sealed class WindowsUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.WindowsUID Value =
                    new DceSecurityGuidGenerator.WindowsUID();
            }

            private static Lazy<int> LazyUserDomainID =
                new Lazy<int>(DceSecurityGuidGenerator.WindowsUID.GetUserDomainID);

            private WindowsUID() { }

            internal static new DceSecurityGuidGenerator.WindowsUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.WindowsUID.Singleton.Value;
            }

            private static int GetUserDomainID()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/USER /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                })!;
                var userSID = string.Empty;
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        userSID = e.Data.Replace("SID:", "").Trim();
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var userSIDs = userSID.Split('-');
                var userID = ushort.Parse(userSIDs[^1]);
                var domainID = ushort.Parse(userSIDs[3]);
                return (userID << (2 * 8)) | domainID;
            }

            protected override int GetUserID()
            {
                return WindowsUID.LazyUserDomainID.Value;
            }
        }

        private sealed class UnixLikeUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.UnixLikeUID Value =
                    new DceSecurityGuidGenerator.UnixLikeUID();
            }

            private static Lazy<int> LazyUserGroupID =
                new Lazy<int>(DceSecurityGuidGenerator.UnixLikeUID.GetUserGroupID);

            private UnixLikeUID() { }

            internal static new DceSecurityGuidGenerator.UnixLikeUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.UnixLikeUID.Singleton.Value;
            }

            private static int GetUserGroupID()
            {
                var userID = UnixLikeUID.GetIDByType("-u");
                var groupID = UnixLikeUID.GetIDByType("-g");
                return (userID << (2 * 8)) | groupID;
            }

            private static ushort GetIDByType(string arguments)
            {
                var idProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "id",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                })!;
                var userID = string.Empty;
                idProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        userID = e.Data.Trim();
                    }
                };
                idProc.BeginOutputReadLine();
                idProc.WaitForExit();
                return ushort.Parse(userID);
            }

            protected override int GetUserID()
            {
                return UnixLikeUID.LazyUserGroupID.Value;
            }
        }

        private sealed class UnknownUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.UnknownUID Value =
                    new DceSecurityGuidGenerator.UnknownUID();
            }

            private UnknownUID() { }

            internal static new DceSecurityGuidGenerator.UnknownUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.UnknownUID.Singleton.Value;
            }

            protected override int GetUserID()
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
