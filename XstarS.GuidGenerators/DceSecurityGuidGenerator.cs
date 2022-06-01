using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class DceSecurityGuidGenerator : TimeBasedGuidGenerator
    {
        private const int DefaultLocalID = 0;

        private readonly Lazy<int> LazyLocalUserID;

        private readonly Lazy<int> LazyLocalGroupID;

        private DceSecurityGuidGenerator()
        {
            this.LazyLocalUserID = new Lazy<int>(this.GetLocalUserID);
            this.LazyLocalGroupID = new Lazy<int>(this.GetLocalGroupID);
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

        private int LocalUserID => this.LazyLocalUserID.Value;

        private int LocalGroupID => this.LazyLocalGroupID.Value;

        public override Guid NewGuid()
        {
            return this.NewGuid(DceSecurityDomain.Person, null);
        }

        public override Guid NewGuid(DceSecurityDomain domain, int? localId = null)
        {
            var guid = base.NewGuid();
            this.FillLocalIDField(ref guid, domain, localId);
            guid.ClkSeqHi_Var() = guid.ClkSeqLow();
            this.FillVariantField(ref guid);
            guid.ClkSeqLow() = (byte)domain;
            return guid;
        }

        protected abstract int GetLocalUserID();

        protected abstract int GetLocalGroupID();

        private void FillLocalIDField(ref Guid guid, DceSecurityDomain domain, int? localID)
        {
            var finalLocalID = localID ?? domain switch
            {
                DceSecurityDomain.Person => this.LocalUserID,
                DceSecurityDomain.Group => this.LocalGroupID,
                DceSecurityDomain.Org => DceSecurityGuidGenerator.DefaultLocalID,
                _ => throw new ArgumentOutOfRangeException(nameof(domain))
            };
            guid.TimeLow() = (uint)finalLocalID;
        }

        private sealed class WindowsUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.WindowsUID Value =
                    new DceSecurityGuidGenerator.WindowsUID();
            }

            private WindowsUID() { }

            internal static new DceSecurityGuidGenerator.WindowsUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.WindowsUID.Singleton.Value;
            }

            protected override int GetLocalUserID()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/USER /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                })!;
                var firstUserID = string.Empty;
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var userSID = e.Data.Replace("SID:", "").Trim();
                        var sidFields = userSID.Split('-');
                        if (firstUserID.Length == 0)
                        {
                            firstUserID = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var userID = (firstUserID.Length > 0) ? firstUserID : "0";
                return (int)ulong.Parse(userID);
            }

            protected override int GetLocalGroupID()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/GROUPS /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                })!;
                var commonGroupID = string.Empty;
                var userGroupID = string.Empty;
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    const int maxCommonFields = 5;
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var groupSID = e.Data.Replace("SID:", "").Trim();
                        var sidFields = groupSID.Split('-');
                        if (sidFields.Length <= maxCommonFields)
                        {
                            if (commonGroupID.Length == 0)
                            {
                                commonGroupID = sidFields[^1];
                            }
                        }
                        else if (userGroupID.Length == 0)
                        {
                            userGroupID = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var groupID = (userGroupID.Length > 0) ? userGroupID :
                    (commonGroupID.Length > 0) ? commonGroupID : "0";
                return (int)ulong.Parse(groupID);
            }
        }

        private sealed class UnixLikeUID : DceSecurityGuidGenerator
        {
            private static class Singleton
            {
                internal static readonly DceSecurityGuidGenerator.UnixLikeUID Value =
                    new DceSecurityGuidGenerator.UnixLikeUID();
            }

            private UnixLikeUID() { }

            internal static new DceSecurityGuidGenerator.UnixLikeUID Instance
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get => DceSecurityGuidGenerator.UnixLikeUID.Singleton.Value;
            }

            protected override int GetLocalUserID() => this.GetLocalIDByType("-u");

            protected override int GetLocalGroupID() => this.GetLocalIDByType("-g");

            private int GetLocalIDByType(string arguments)
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
                return (int)ulong.Parse(userID);
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

            protected override int GetLocalUserID() => throw new NotImplementedException();
            protected override int GetLocalGroupID() => throw new NotImplementedException();
        }
    }
}
