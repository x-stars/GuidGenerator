using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class LocalIDProvider
    {
        private static class Singleton
        {
            internal static readonly LocalIDProvider Value =
                LocalIDProvider.IsSupportedWindows ?
                new LocalIDProvider.Windows() :
                LocalIDProvider.IsSupportedUnixLike ?
                new LocalIDProvider.UnixLike() :
                new LocalIDProvider.Unknown();
        }

        internal static LocalIDProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => LocalIDProvider.Singleton.Value;
        }

        private static bool IsSupportedWindows =>
            Environment.OSVersion.Platform == PlatformID.Win32NT;

        private static bool IsSupportedUnixLike =>
            Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX;

        public abstract int GetLocalUserID();

        public abstract int GetLocalGroupID();

        private sealed class Windows : LocalIDProvider
        {
            internal Windows() { }

            public override int GetLocalUserID()
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

            public override int GetLocalGroupID()
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

        private sealed class UnixLike : LocalIDProvider
        {
            internal UnixLike() { }

            public override int GetLocalUserID()
            {
                return this.GetLocalIDByType("-ru");
            }

            public override int GetLocalGroupID()
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

        private sealed class Unknown : LocalIDProvider
        {
            internal Unknown() { }

            public override int GetLocalUserID()
            {
                throw new PlatformNotSupportedException();
            }

            public override int GetLocalGroupID()
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
