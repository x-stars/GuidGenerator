using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace XstarS.GuidGenerators
{
    internal abstract class LocalIdProvider
    {
        private static class Singleton
        {
            internal static readonly LocalIdProvider Value =
                LocalIdProvider.IsSupportedWindows ?
                new LocalIdProvider.Windows() :
                LocalIdProvider.IsSupportedUnixLike ?
                new LocalIdProvider.UnixLike() :
                new LocalIdProvider.Unknown();
        }

        internal static LocalIdProvider Instance
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => LocalIdProvider.Singleton.Value;
        }

        private static bool IsSupportedWindows =>
            Environment.OSVersion.Platform == PlatformID.Win32NT;

        private static bool IsSupportedUnixLike =>
            Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX;

        public abstract int GetLocalUserId();

        public abstract int GetLocalGroupId();

        private sealed class Windows : LocalIdProvider
        {
            internal Windows() { }

            public override int GetLocalUserId()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/USER /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                })!;
                var firstUserId = default(string);
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var userSid = e.Data.Replace("SID:", "").Trim();
                        var sidFields = userSid.Split('-');
                        if (firstUserId is null)
                        {
                            firstUserId = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var userId = firstUserId ?? "0";
                return (int)ulong.Parse(userId);
            }

            public override int GetLocalGroupId()
            {
                var whoamiProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "WHOAMI.exe",
                    Arguments = "/GROUPS /FO LIST",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                })!;
                var commonGroupId = default(string);
                var userGroupId = default(string);
                whoamiProc.OutputDataReceived += (sender, e) =>
                {
                    const int maxCommonFields = 5;
                    if (e.Data?.StartsWith("SID:") ?? false)
                    {
                        var groupSid = e.Data.Replace("SID:", "").Trim();
                        var sidFields = groupSid.Split('-');
                        if (sidFields.Length <= maxCommonFields)
                        {
                            if (commonGroupId is null)
                            {
                                commonGroupId = sidFields[^1];
                            }
                        }
                        else if (userGroupId is null)
                        {
                            userGroupId = sidFields[^1];
                        }
                    }
                };
                whoamiProc.BeginOutputReadLine();
                whoamiProc.WaitForExit();
                var groupId = userGroupId ?? commonGroupId ?? "0";
                return (int)ulong.Parse(groupId);
            }
        }

        private sealed class UnixLike : LocalIdProvider
        {
            internal UnixLike() { }

            public override int GetLocalUserId()
            {
                return this.GetLocalIdByType("-ru");
            }

            public override int GetLocalGroupId()
            {
                return this.GetLocalIdByType("-rg");
            }

            private int GetLocalIdByType(string arguments)
            {
                var idProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "id",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                })!;
                var read = default(string);
                idProc.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        read = e.Data.Trim();
                    }
                };
                idProc.BeginOutputReadLine();
                idProc.WaitForExit();
                var localId = read ?? "0";
                return (int)ulong.Parse(localId);
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
