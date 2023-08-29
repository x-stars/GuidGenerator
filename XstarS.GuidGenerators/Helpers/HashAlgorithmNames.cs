#if !FEATURE_DISABLE_UUIDREV
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Security.Cryptography
{
    internal static class HashAlgorithmNames
    {
        // These are accepted by CNG
        public const string MD5 = "MD5";
        public const string SHA1 = "SHA1";
        public const string SHA256 = "SHA256";
        public const string SHA384 = "SHA384";
        public const string SHA512 = "SHA512";
#if NET8_0_OR_GREATER
        public const string SHA3_256 = "SHA3-256";
        public const string SHA3_384 = "SHA3-384";
        public const string SHA3_512 = "SHA3-512";
        public const string SHAKE128 = "SHAKE128";
        public const string SHAKE256 = "SHAKE256";
        public const string CSHAKE128 = "CSHAKE128";
        public const string CSHAKE256 = "CSHAKE256";
#endif
    }
}
#endif
