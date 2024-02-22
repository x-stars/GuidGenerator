// This minimal API is modified to get the user SID and the primary group SID
// that is not supported by `System.Security.Principal.Windows` public APIs.

// Original file license:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Modified part license:
// Copyright (c) 2024 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

using System;
using System.Runtime.InteropServices;
using System.Security;
using Internal.Security.Principal;
using Internal.Win32.SafeHandles;
#if !NET7_0_OR_GREATER
using System.ComponentModel;
#endif

// WindowsIdentity.cs
namespace Internal.Security.Principal
{
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    internal static class WindowsIdentity
    {
        public static SafeAccessTokenHandle? GetCurrentToken()
        {
            TokenAccessLevels desiredAccess = TokenAccessLevels.MaximumAllowed;
            bool threadOnly = false;

            SafeAccessTokenHandle safeTokenHandle = GetCurrentToken(
                desiredAccess, threadOnly, out bool isImpersonating, out int hr);
            if (safeTokenHandle == null || safeTokenHandle.IsInvalid)
            {
                safeTokenHandle?.Dispose();

                // either we wanted only ThreadToken - return null
                if (threadOnly && !isImpersonating)
                {
                    return null;
                }

                // or there was an error
                throw new SecurityException(
#if NET7_0_OR_GREATER
                    Marshal.GetPInvokeErrorMessage(hr)
#else
                    new Win32Exception(hr).Message
#endif
                    );
            }

            return safeTokenHandle;
        }

        private static int GetHRForWin32Error(int dwLastError)
        {
            if ((dwLastError & 0x80000000) == 0x80000000)
                return dwLastError;
            else
                return (dwLastError & 0x0000FFFF) | unchecked((int)0x80070000);
        }

        private static SafeAccessTokenHandle GetCurrentToken(
            TokenAccessLevels desiredAccess, bool threadOnly, out bool isImpersonating, out int hr)
        {
            isImpersonating = true;
            hr = 0;
            bool success = Interop.Advapi32.OpenThreadToken(
                desiredAccess, WinSecurityContext.Both, out SafeAccessTokenHandle safeTokenHandle);
            if (!success)
            {
                hr = Marshal.GetHRForLastWin32Error();
                if (hr == GetHRForWin32Error(Interop.Errors.ERROR_NO_TOKEN))
                {
                    // No impersonation
                    isImpersonating = false;
                    if (!threadOnly)
                    {
                        safeTokenHandle.Dispose();
                        safeTokenHandle = GetCurrentProcessToken(desiredAccess, out hr);
                    }
                }
            }

            return safeTokenHandle;
        }

        private static SafeAccessTokenHandle GetCurrentProcessToken(TokenAccessLevels desiredAccess, out int hr)
        {
            hr = 0;
            if (!Interop.Advapi32.OpenProcessToken(
                Interop.Kernel32.GetCurrentProcess(), desiredAccess, out SafeAccessTokenHandle safeTokenHandle))
                hr = GetHRForWin32Error(
#if NET7_0_OR_GREATER
                    Marshal.GetLastPInvokeError()
#else
                    Marshal.GetLastWin32Error()
#endif
                    );
            return safeTokenHandle;
        }

        public static byte[]? GetUserSid(SafeAccessTokenHandle? safeTokenHandle)
        {
            return GetSidOfClass(safeTokenHandle, TokenInformationClass.TokenUser);
        }

        public static byte[]? GetPrimaryGroupSid(SafeAccessTokenHandle? safeTokenHandle)
        {
            return GetSidOfClass(safeTokenHandle, TokenInformationClass.TokenPrimaryGroup);
        }

        private static byte[]? GetSidOfClass(
            SafeAccessTokenHandle? safeTokenHandle, TokenInformationClass tokenInfoClass)
        {
            // special case the anonymous identity.
            if (safeTokenHandle is null || safeTokenHandle.IsInvalid)
                return null;

            using SafeLocalAllocHandle tokenInfo = GetTokenInformation(
                safeTokenHandle, tokenInfoClass, nullOnInvalidParam: false)!;
            return Win32.ConvertIntPtrSidToByteArraySid(tokenInfo.Read<nint>(0));
        }

        private static SafeLocalAllocHandle? GetTokenInformation(
            SafeAccessTokenHandle tokenHandle, TokenInformationClass tokenInformationClass,
            bool nullOnInvalidParam = false)
        {
            SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
            try
            {
                Interop.Advapi32.GetTokenInformation(
                    tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, 0, out uint dwLength);
                int dwErrorCode =
#if NET7_0_OR_GREATER
                    Marshal.GetLastPInvokeError();
#else
                    Marshal.GetLastWin32Error();
#endif
                switch (dwErrorCode)
                {
                    case Interop.Errors.ERROR_BAD_LENGTH: // special case for TokenSessionId. Falling through
                    case Interop.Errors.ERROR_INSUFFICIENT_BUFFER:
                        safeLocalAllocHandle.Dispose();
                        safeLocalAllocHandle = SafeLocalAllocHandle.LocalAlloc(checked((int)dwLength));

                        bool result = Interop.Advapi32.GetTokenInformation(
                            tokenHandle, (uint)tokenInformationClass, safeLocalAllocHandle, dwLength, out _);
                        if (!result)
                        {
                            throw new SecurityException(
#if NET7_0_OR_GREATER
                                Marshal.GetLastPInvokeErrorMessage()
#else
                                new Win32Exception().Message
#endif
                                );
                        }
                        break;

                    case Interop.Errors.ERROR_INVALID_HANDLE:
                        throw new ArgumentException(
                            "Invalid token for impersonation - it cannot be duplicated.");

                    case Interop.Errors.ERROR_INVALID_PARAMETER:
                        if (nullOnInvalidParam)
                        {
                            safeLocalAllocHandle.Dispose();
                            return null;
                        }

                        // Throw the exception.
                        goto default;
                    default:
                        throw new SecurityException(
#if NET7_0_OR_GREATER
                            Marshal.GetPInvokeErrorMessage(dwErrorCode)
#else
                            new Win32Exception(dwErrorCode).Message
#endif
                            );
                }

                return safeLocalAllocHandle;
            }
            catch
            {
                safeLocalAllocHandle.Dispose();
                throw;
            }
        }
    }
}

// SID.cs
namespace Internal.Security.Principal
{
    internal static class SecurityIdentifier
    {
        // Identifier authority must be at most six bytes long
        internal const long MaxIdentifierAuthority = 0xFFFFFFFFFFFF;

        // Maximum number of subauthorities in a SID
        internal const int MaxSubAuthorities = 15;

        // Minimum length of a binary representation of a SID
        public static readonly int MinBinaryLength =
            1 + 1 + 6; // Revision (1) + subauth count (1) + identifier authority (6)

        // Maximum length of a binary representation of a SID
        public static readonly int MaxBinaryLength =
            1 + 1 + 6 + MaxSubAuthorities * 4; // 4 bytes for each subauth

        // Revision is always '1'
        internal static byte Revision => 1;
    }
}

// Win32.cs
namespace Internal.Security.Principal
{
    internal static class Win32
    {
        internal static byte[] ConvertIntPtrSidToByteArraySid(nint binaryForm)
        {
            byte[] ResultSid;

            // Verify the revision (just sanity, should never fail to be 1)
            byte Revision = Marshal.ReadByte(binaryForm, 0);

            if (Revision != SecurityIdentifier.Revision)
            {
                throw new ArgumentException(
                    "SIDs with revision other than '1' are not supported.",
                    nameof(binaryForm));
            }

            // Need the subauthority count in order to figure out how many bytes to read
            byte SubAuthorityCount = Marshal.ReadByte(binaryForm, 1);

            if (SubAuthorityCount < 0 ||
                SubAuthorityCount > SecurityIdentifier.MaxSubAuthorities)
            {
                throw new ArgumentException(
                    string.Format(
                        "The number of sub-authorities must not exceed {0}.",
                        SecurityIdentifier.MaxSubAuthorities),
                    nameof(binaryForm));
            }

            // Compute the size of the binary form of this SID and allocate the memory
            int BinaryLength = 1 + 1 + 6 + SubAuthorityCount * 4;
            ResultSid = new byte[BinaryLength];

            // Extract the data from the returned pointer
            Marshal.Copy(binaryForm, ResultSid, 0, BinaryLength);

            return ResultSid;
        }
    }
}

// TokenAccessLevels.cs
namespace Internal.Security.Principal
{
    [Flags]
    internal enum TokenAccessLevels
    {
        AssignPrimary = 0x00000001,
        Duplicate = 0x00000002,
        Impersonate = 0x00000004,
        Query = 0x00000008,
        QuerySource = 0x00000010,
        AdjustPrivileges = 0x00000020,
        AdjustGroups = 0x00000040,
        AdjustDefault = 0x00000080,
        AdjustSessionId = 0x00000100,

        Read = 0x00020000 | Query,

        Write = 0x00020000 | AdjustPrivileges | AdjustGroups | AdjustDefault,

        AllAccess = 0x000F0000 | AssignPrimary | Duplicate | Impersonate |
                    Query | QuerySource | AdjustPrivileges | AdjustGroups |
                    AdjustDefault | AdjustSessionId,

        MaximumAllowed = 0x02000000,
    }
}

// WindowsIdentity.cs
namespace Internal.Security.Principal
{
    internal enum WinSecurityContext
    {
        Thread = 1, // OpenAsSelf = false
        Process = 2, // OpenAsSelf = true
        Both = 3, // OpenAsSelf = true, then OpenAsSelf = false
    }

    internal enum TokenType : int
    {
        TokenPrimary = 1,
        TokenImpersonation,
    }

    internal enum TokenInformationClass : int
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin,
        TokenElevationType,
        TokenLinkedToken,
        TokenElevation,
        TokenHasRestrictions,
        TokenAccessInformation,
        TokenVirtualizationAllowed,
        TokenVirtualizationEnabled,
        TokenIntegrityLevel,
        TokenUIAccess,
        TokenMandatoryPolicy,
        TokenLogonSid,
        TokenIsAppContainer,
        TokenCapabilities,
        TokenAppContainerSid,
        TokenAppContainerNumber,
        TokenUserClaimAttributes,
        TokenDeviceClaimAttributes,
        TokenRestrictedUserClaimAttributes,
        TokenRestrictedDeviceClaimAttributes,
        TokenDeviceGroups,
        TokenRestrictedDeviceGroups,
        MaxTokenInfoClass,  // MaxTokenInfoClass should always be the last enum
    }
}

namespace Internal.Win32.SafeHandles
{
    using SafeHandleZeroOrMinusOneIsInvalid =
        Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid;

    // SafeAccessTokenHandle.cs
    internal sealed class SafeAccessTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeAccessTokenHandle() : base(true) { }

        public SafeAccessTokenHandle(nint handle) : base(true) { SetHandle(handle); }

        public static SafeAccessTokenHandle InvalidHandle => new SafeAccessTokenHandle((nint)0);

        protected override bool ReleaseHandle()
        {
            return Interop.Kernel32.CloseHandle(handle);
        }
    }

    // SafeLocalAllocHandle.cs
    internal sealed class SafeLocalAllocHandle : SafeBuffer
    {
        public SafeLocalAllocHandle() : base(true) { }

        internal SafeLocalAllocHandle(nint handle) : base(true) { SetHandle(handle); }

        internal static SafeLocalAllocHandle InvalidHandle => new SafeLocalAllocHandle((nint)0);

        internal static SafeLocalAllocHandle LocalAlloc(int cb)
        {
            var h = new SafeLocalAllocHandle();
            h.SetHandle(Marshal.AllocHGlobal(cb));
            h.Initialize((ulong)cb);
            return h;
        }

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }
    }
}

// Interop.Errors.cs
namespace Interop
{
    // As defined in winerror.h and https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes
    internal static partial class Errors
    {
        internal const int ERROR_SUCCESS = 0x0;
        internal const int ERROR_INVALID_FUNCTION = 0x1;
        internal const int ERROR_FILE_NOT_FOUND = 0x2;
        internal const int ERROR_PATH_NOT_FOUND = 0x3;
        internal const int ERROR_ACCESS_DENIED = 0x5;
        internal const int ERROR_INVALID_HANDLE = 0x6;
        internal const int ERROR_NOT_ENOUGH_MEMORY = 0x8;
        internal const int ERROR_INVALID_DATA = 0xD;
        internal const int ERROR_OUTOFMEMORY = 0xE;
        internal const int ERROR_INVALID_DRIVE = 0xF;
        internal const int ERROR_NO_MORE_FILES = 0x12;
        internal const int ERROR_NOT_READY = 0x15;
        internal const int ERROR_BAD_COMMAND = 0x16;
        internal const int ERROR_BAD_LENGTH = 0x18;
        internal const int ERROR_SHARING_VIOLATION = 0x20;
        internal const int ERROR_LOCK_VIOLATION = 0x21;
        internal const int ERROR_HANDLE_EOF = 0x26;
        internal const int ERROR_NOT_SUPPORTED = 0x32;
        internal const int ERROR_BAD_NETPATH = 0x35;
        internal const int ERROR_NETWORK_ACCESS_DENIED = 0x41;
        internal const int ERROR_BAD_NET_NAME = 0x43;
        internal const int ERROR_FILE_EXISTS = 0x50;
        internal const int ERROR_INVALID_PARAMETER = 0x57;
        internal const int ERROR_BROKEN_PIPE = 0x6D;
        internal const int ERROR_DISK_FULL = 0x70;
        internal const int ERROR_SEM_TIMEOUT = 0x79;
        internal const int ERROR_CALL_NOT_IMPLEMENTED = 0x78;
        internal const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
        internal const int ERROR_INVALID_NAME = 0x7B;
        internal const int ERROR_INVALID_LEVEL = 0x7C;
        internal const int ERROR_MOD_NOT_FOUND = 0x7E;
        internal const int ERROR_NEGATIVE_SEEK = 0x83;
        internal const int ERROR_DIR_NOT_EMPTY = 0x91;
        internal const int ERROR_BAD_PATHNAME = 0xA1;
        internal const int ERROR_LOCK_FAILED = 0xA7;
        internal const int ERROR_BUSY = 0xAA;
        internal const int ERROR_ALREADY_EXISTS = 0xB7;
        internal const int ERROR_BAD_EXE_FORMAT = 0xC1;
        internal const int ERROR_ENVVAR_NOT_FOUND = 0xCB;
        internal const int ERROR_FILENAME_EXCED_RANGE = 0xCE;
        internal const int ERROR_EXE_MACHINE_TYPE_MISMATCH = 0xD8;
        internal const int ERROR_FILE_TOO_LARGE = 0xDF;
        internal const int ERROR_PIPE_BUSY = 0xE7;
        internal const int ERROR_NO_DATA = 0xE8;
        internal const int ERROR_PIPE_NOT_CONNECTED = 0xE9;
        internal const int ERROR_MORE_DATA = 0xEA;
        internal const int ERROR_NO_MORE_ITEMS = 0x103;
        internal const int ERROR_DIRECTORY = 0x10B;
        internal const int ERROR_NOT_OWNER = 0x120;
        internal const int ERROR_TOO_MANY_POSTS = 0x12A;
        internal const int ERROR_PARTIAL_COPY = 0x12B;
        internal const int ERROR_ARITHMETIC_OVERFLOW = 0x216;
        internal const int ERROR_PIPE_CONNECTED = 0x217;
        internal const int ERROR_PIPE_LISTENING = 0x218;
        internal const int ERROR_MUTANT_LIMIT_EXCEEDED = 0x24B;
        internal const int ERROR_OPERATION_ABORTED = 0x3E3;
        internal const int ERROR_IO_INCOMPLETE = 0x3E4;
        internal const int ERROR_IO_PENDING = 0x3E5;
        internal const int ERROR_INVALID_FLAGS = 0x3EC;
        internal const int ERROR_NO_TOKEN = 0x3f0;
        internal const int ERROR_SERVICE_DOES_NOT_EXIST = 0x424;
        internal const int ERROR_EXCEPTION_IN_SERVICE = 0x428;
        internal const int ERROR_PROCESS_ABORTED = 0x42B;
        internal const int ERROR_FILEMARK_DETECTED = 0x44D;
        internal const int ERROR_NO_UNICODE_TRANSLATION = 0x459;
        internal const int ERROR_DLL_INIT_FAILED = 0x45A;
        internal const int ERROR_COUNTER_TIMEOUT = 0x461;
        internal const int ERROR_NO_ASSOCIATION = 0x483;
        internal const int ERROR_DDE_FAIL = 0x484;
        internal const int ERROR_DLL_NOT_FOUND = 0x485;
        internal const int ERROR_NOT_FOUND = 0x490;
        internal const int ERROR_INVALID_DOMAINNAME = 0x4BC;
        internal const int ERROR_CANCELLED = 0x4C7;
        internal const int ERROR_NETWORK_UNREACHABLE = 0x4CF;
        internal const int ERROR_NON_ACCOUNT_SID = 0x4E9;
        internal const int ERROR_NOT_ALL_ASSIGNED = 0x514;
        internal const int ERROR_UNKNOWN_REVISION = 0x519;
        internal const int ERROR_INVALID_OWNER = 0x51B;
        internal const int ERROR_INVALID_PRIMARY_GROUP = 0x51C;
        internal const int ERROR_NO_LOGON_SERVERS = 0x51F;
        internal const int ERROR_NO_SUCH_LOGON_SESSION = 0x520;
        internal const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
        internal const int ERROR_PRIVILEGE_NOT_HELD = 0x522;
        internal const int ERROR_INVALID_ACL = 0x538;
        internal const int ERROR_INVALID_SECURITY_DESCR = 0x53A;
        internal const int ERROR_INVALID_SID = 0x539;
        internal const int ERROR_BAD_IMPERSONATION_LEVEL = 0x542;
        internal const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;
        internal const int ERROR_NO_SECURITY_ON_OBJECT = 0x546;
        internal const int ERROR_NO_SUCH_DOMAIN = 0x54B;
        internal const int ERROR_CANNOT_IMPERSONATE = 0x558;
        internal const int ERROR_CLASS_ALREADY_EXISTS = 0x582;
        internal const int ERROR_NO_SYSTEM_RESOURCES = 0x5AA;
        internal const int ERROR_TIMEOUT = 0x5B4;
        internal const int ERROR_EVENTLOG_FILE_CHANGED = 0x5DF;
        internal const int RPC_S_OUT_OF_RESOURCES = 0x6B9;
        internal const int RPC_S_SERVER_UNAVAILABLE = 0x6BA;
        internal const int RPC_S_CALL_FAILED = 0x6BE;
        internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x6FD;
        internal const int ERROR_RESOURCE_TYPE_NOT_FOUND = 0x715;
        internal const int ERROR_RESOURCE_LANG_NOT_FOUND = 0x717;
        internal const int RPC_S_CALL_CANCELED = 0x71A;
        internal const int ERROR_NO_SITENAME = 0x77F;
        internal const int ERROR_NOT_A_REPARSE_POINT = 0x1126;
        internal const int ERROR_DS_NAME_UNPARSEABLE = 0x209E;
        internal const int ERROR_DS_UNKNOWN_ERROR = 0x20EF;
        internal const int ERROR_DS_DRA_BAD_DN = 0x20F7;
        internal const int ERROR_DS_DRA_OUT_OF_MEM = 0x20FE;
        internal const int ERROR_DS_DRA_ACCESS_DENIED = 0x2105;
        internal const int DNS_ERROR_RCODE_NAME_ERROR = 0x232B;
        internal const int ERROR_EVT_QUERY_RESULT_STALE = 0x3AA3;
        internal const int ERROR_EVT_QUERY_RESULT_INVALID_POSITION = 0x3AA4;
        internal const int ERROR_EVT_INVALID_EVENT_DATA = 0x3A9D;
        internal const int ERROR_EVT_PUBLISHER_METADATA_NOT_FOUND = 0x3A9A;
        internal const int ERROR_EVT_CHANNEL_NOT_FOUND = 0x3A9F;
        internal const int ERROR_EVT_MESSAGE_NOT_FOUND = 0x3AB3;
        internal const int ERROR_EVT_MESSAGE_ID_NOT_FOUND = 0x3AB4;
        internal const int ERROR_EVT_PUBLISHER_DISABLED = 0x3ABD;
    }
}

// Kernel32/Interop.CloseHandle.cs
namespace Interop
{
    internal static partial class Kernel32
    {
#if NET7_0_OR_GREATER
        [LibraryImport("kernel32.dll", SetLastError = true)]
#else
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
#endif
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static
#if NET7_0_OR_GREATER
        partial
#else
        extern
#endif
        bool CloseHandle(nint handle);
    }
}

namespace Interop
{
    internal static partial class Kernel32
    {
        // Kernel32/Interop.GetCurrentProcess.cs
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern nint GetCurrentProcess();

        // Kernel32/Interop.GetCurrentThread.cs
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern nint GetCurrentThread();
    }
}

// Advapi32/Interop.OpenProcessToken_SafeAccessTokenHandle.cs
namespace Interop
{
    internal static partial class Advapi32
    {
#if NET7_0_OR_GREATER
        [LibraryImport("advapi32.dll", SetLastError = true)]
#else
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
#endif
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static
#if NET7_0_OR_GREATER
        partial
#else
        extern
#endif
        bool OpenProcessToken(
            nint ProcessToken,
            TokenAccessLevels DesiredAccess,
            out SafeAccessTokenHandle TokenHandle);
    }
}

// Advapi32/Interop.OpenThreadToken.cs
namespace Interop
{
    internal static partial class Advapi32
    {
#if NET7_0_OR_GREATER
        [LibraryImport("advapi32.dll", SetLastError = true)]
#else
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
#endif
        [return: MarshalAs(UnmanagedType.Bool)]
        private static
#if NET7_0_OR_GREATER
        partial
#else
        extern
#endif
        bool OpenThreadToken(
            nint ThreadHandle,
            TokenAccessLevels dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bOpenAsSelf,
            out SafeAccessTokenHandle phThreadToken);

        internal static bool OpenThreadToken(
            TokenAccessLevels desiredAccess, WinSecurityContext openAs, out SafeAccessTokenHandle tokenHandle)
        {
            bool openAsSelf = true;
            if (openAs == WinSecurityContext.Thread)
                openAsSelf = false;

            if (OpenThreadToken(Kernel32.GetCurrentThread(), desiredAccess, openAsSelf, out tokenHandle))
                return true;

            if (openAs == WinSecurityContext.Both)
            {
                openAsSelf = false;
                tokenHandle.Dispose();
                if (OpenThreadToken(Kernel32.GetCurrentThread(), desiredAccess, openAsSelf, out tokenHandle))
                    return true;
            }

            return false;
        }
    }
}

// Advapi32/Interop.GetTokenInformation_SafeLocalAllocHandle.cs
namespace Interop
{
    internal static partial class Advapi32
    {
#if NET7_0_OR_GREATER
        [LibraryImport("advapi32.dll", SetLastError = true)]
#else
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
#endif
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static
#if NET7_0_OR_GREATER
        partial
#else
        extern
#endif
        bool GetTokenInformation(
            SafeAccessTokenHandle TokenHandle,
            uint TokenInformationClass,
            SafeLocalAllocHandle TokenInformation,
            uint TokenInformationLength,
            out uint ReturnLength);
    }
}
