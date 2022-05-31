using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class DceSecurityGuidGenerator : TimeBasedGuidGenerator
    {
        private readonly Lazy<byte> LazyIPAddressLastByte;

        private DceSecurityGuidGenerator()
        {
            this.LazyIPAddressLastByte = new Lazy<byte>(this.GetIPAddressLastByte);
        }

        internal static new DceSecurityGuidGenerator Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => DceSecurityGuidGenerator.IsSupportedWindows ?
                   DceSecurityGuidGenerator.WindowsUID.Instance :
                   DceSecurityGuidGenerator.IsSupportedUnixLike ?
                   DceSecurityGuidGenerator.UnixLikeUID.Instance :
                   DceSecurityGuidGenerator.UnknownUID.Instance;
        }

        private static bool IsSupportedWindows =>
            Environment.OSVersion.Platform == PlatformID.Win32NT;

        private static bool IsSupportedUnixLike =>
            Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX;

        public override GuidVersion Version => GuidVersion.Version2;

        private byte IPAddressLastByte => this.LazyIPAddressLastByte.Value;

        private byte GetIPAddressLastByte()
        {
            var upIface = this.UpNetworkInterface;
            if (upIface is null) { return (byte)0; }
            var ipAddrs = upIface.GetIPProperties().UnicastAddresses;
            if (ipAddrs.Count == 0) { return (byte)0; }
            var ipAddrBytes = ipAddrs[0].Address.GetAddressBytes();
            return ipAddrBytes[^1];
        }

        public override Guid NewGuid()
        {
            var guid = base.NewGuid();
            this.FillUserIDFields(ref guid);
            guid.ClkSeqHi_Var() = guid.ClkSeqLow();
            this.FillVariantField(ref guid);
            this.FillSiteIDField(ref guid);
            return guid;
        }

        protected abstract int GetUserID();

        private void FillUserIDFields(ref Guid guid)
        {
            var userID = this.GetUserID();
            guid.TimeLow() = (uint)userID;
        }

        private void FillSiteIDField(ref Guid guid)
        {
            var siteID = this.IPAddressLastByte;
            guid.ClkSeqLow() = (byte)siteID;
        }

        private sealed class WindowsUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.WindowsUID Value =
                    new DceSecurityGuidGenerator.WindowsUID();
            }

            private readonly Lazy<int> LazyUserDomainID;

            private WindowsUID()
            {
                this.LazyUserDomainID = new Lazy<int>(this.GetUserDomainID);
            }

            internal static new DceSecurityGuidGenerator.WindowsUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.WindowsUID.Singleton.Value;
            }

            private int GetUserDomainID()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/USER /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
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
                var userID = (int)ulong.Parse(userSIDs[^1]);
                var domainID = (byte)ulong.Parse(userSIDs[3]);
                return (userID << (1 * 8)) | domainID;
            }

            protected override int GetUserID() => this.LazyUserDomainID.Value;
        }

        private sealed class UnixLikeUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.UnixLikeUID Value =
                    new DceSecurityGuidGenerator.UnixLikeUID();
            }

            private readonly Lazy<int> LazyUserGroupID;

            private UnixLikeUID()
            {
                this.LazyUserGroupID = new Lazy<int>(this.GetUserGroupID);
            }

            internal static new DceSecurityGuidGenerator.UnixLikeUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.UnixLikeUID.Singleton.Value;
            }

            private int GetUserGroupID()
            {
                var userID = this.GetIDByType("-u");
                var groupID = this.GetIDByType("-g");
                return (userID << (2 * 8)) | groupID;
            }

            private ushort GetIDByType(string arguments)
            {
                var idProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "id",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
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
                return (ushort)ulong.Parse(userID);
            }

            protected override int GetUserID() => this.LazyUserGroupID.Value;
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
