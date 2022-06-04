using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class DceSecurityGuidGenerator : TimeBasedGuidGenerator, IDceSecurityGuidGenerator
    {
        private static class Singleton
        {
            internal static readonly DceSecurityGuidGenerator Value =
                DceSecurityGuidGenerator.IsSupportedWindows ?
                new DceSecurityGuidGenerator.WindowsUID() :
                DceSecurityGuidGenerator.IsSupportedUnixLike ?
                new DceSecurityGuidGenerator.UnixLikeUID() :
                new DceSecurityGuidGenerator.UnknownUID();
        }

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
            get => DceSecurityGuidGenerator.Singleton.Value;
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

        public override Guid NewGuid(DceSecurityDomain domain, int? localID = null)
        {
            var guid = base.NewGuid();
            var iLocalID = this.GetLocalID(domain, localID);
            guid.TimeLow() = (uint)iLocalID;
            guid.ClkSeqHi_Var() = guid.ClkSeqLow();
            this.FillVariantField(ref guid);
            guid.ClkSeqLow() = (byte)domain;
            return guid;
        }

        protected abstract int GetLocalUserID();

        protected abstract int GetLocalGroupID();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLocalID(DceSecurityDomain domain, int? localID) => domain switch
        {
            DceSecurityDomain.Person => localID ?? this.LocalUserID,
            DceSecurityDomain.Group => localID ?? this.LocalGroupID,
            DceSecurityDomain.Org => localID ?? default(int),
            _ => throw new ArgumentOutOfRangeException(nameof(domain))
        };

        private sealed class WindowsUID : DceSecurityGuidGenerator
        {
            internal WindowsUID() { }

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
                var firstUserID = default(string);
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var userSID = e.Data.Replace("SID:", "").Trim();
                        var sidFields = userSID.Split('-');
                        if (firstUserID is null)
                        {
                            firstUserID = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var userID = firstUserID ?? "0";
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
                var commonGroupID = default(string);
                var userGroupID = default(string);
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    const int maxCommonFields = 5;
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var groupSID = e.Data.Replace("SID:", "").Trim();
                        var sidFields = groupSID.Split('-');
                        if (sidFields.Length <= maxCommonFields)
                        {
                            if (commonGroupID is null)
                            {
                                commonGroupID = sidFields[^1];
                            }
                        }
                        else if (userGroupID is null)
                        {
                            userGroupID = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var groupID = userGroupID ?? commonGroupID ?? "0";
                return (int)ulong.Parse(groupID);
            }
        }

        private sealed class UnixLikeUID : DceSecurityGuidGenerator
        {
            internal UnixLikeUID() { }

            protected override int GetLocalUserID()
            {
                return this.GetLocalIDByType("-ru");
            }

            protected override int GetLocalGroupID()
            {
                return this.GetLocalIDByType("-rg");
            }

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
                var readID = default(string);
                idProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        readID = e.Data.Trim();
                    }
                };
                idProc.BeginOutputReadLine();
                idProc.WaitForExit();
                var localID = readID ?? "0";
                return (int)ulong.Parse(localID);
            }
        }

        private sealed class UnknownUID : DceSecurityGuidGenerator
        {
            internal UnknownUID() { }

            protected override int GetLocalUserID()
            {
                throw new PlatformNotSupportedException();
            }

            protected override int GetLocalGroupID()
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
