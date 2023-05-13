#if !FEATURE_DISABLE_UUIDREV
using System;

namespace XNetEx.Guids;

/// <summary>
/// Provides <see cref="Guid"/> hashspace IDs specified in RFC4122bis UUIDREV.
/// </summary>
public static class GuidHashspaces
{
    /// <summary>
    /// Represents the hashspace ID for the SHA-224 hash algorithm.
    /// </summary>
    public static readonly Guid Sha224 = new Guid(
        // 59031ca3-fbdb-47fb-9f6c-0f30e2e83145
        0x59031ca3, 0xfbdb, 0x47fb,
        0x9f, 0x6c, 0x0f, 0x30, 0xe2, 0xe8, 0x31, 0x45);

    /// <summary>
    /// Represents the hashspace ID for the SHA-256 hash algorithm.
    /// </summary>
    public static readonly Guid Sha256 = new Guid(
        // 3fb32780-953c-4464-9cfd-e85dbbe9843d
        0x3fb32780, 0x953c, 0x4464,
        0x9c, 0xfd, 0xe8, 0x5d, 0xbb, 0xe9, 0x84, 0x3d);

    /// <summary>
    /// Represents the hashspace ID for the SHA-384 hash algorithm.
    /// </summary>
    public static readonly Guid Sha384 = new Guid(
        // e6800581-f333-484b-8778-601ff2b58da8
        0xe6800581, 0xf333, 0x484b,
        0x87, 0x78, 0x60, 0x1f, 0xf2, 0xb5, 0x8d, 0xa8);

    /// <summary>
    /// Represents the hashspace ID for the SHA-512 hash algorithm.
    /// </summary>
    public static readonly Guid Sha512 = new Guid(
        // 0fde22f2-e7ba-4fd1-9753-9c2ea88fa3f9
        0x0fde22f2, 0xe7ba, 0x4fd1,
        0x97, 0x53, 0x9c, 0x2e, 0xa8, 0x8f, 0xa3, 0xf9);

    /// <summary>
    /// Represents the hashspace ID for the SHA-512/224 hash algorithm.
    /// </summary>
    public static readonly Guid Sha512T224 = new Guid(
        // 003c2038-c4fe-4b95-a672-0c26c1b79542
        0x003c2038, 0xc4fe, 0x4b95,
        0xa6, 0x72, 0x0c, 0x26, 0xc1, 0xb7, 0x95, 0x42);

    /// <summary>
    /// Represents the hashspace ID for the SHA-512/256 hash algorithm.
    /// </summary>
    public static readonly Guid Sha512T256 = new Guid(
        // 9475ad00-3769-4c07-9642-5e7383732306
        0x9475ad00, 0x3769, 0x4c07,
        0x96, 0x42, 0x5e, 0x73, 0x83, 0x73, 0x23, 0x06);

    /// <summary>
    /// Represents the hashspace ID for the SHA3-224 hash algorithm.
    /// </summary>
    public static readonly Guid Sha3D224 = new Guid(
        // 9768761f-ac5a-419e-a180-7ca239e8025a
        0x9768761f, 0xac5a, 0x419e,
        0xa1, 0x80, 0x7c, 0xa2, 0x39, 0xe8, 0x02, 0x5a);

    /// <summary>
    /// Represents the hashspace ID for the SHA3-256 hash algorithm.
    /// </summary>
    public static readonly Guid Sha3D256 = new Guid(
        // 2034d66b-4047-4553-8f80-70e593176877
        0x2034d66b, 0x4047, 0x4553,
        0x8f, 0x80, 0x70, 0xe5, 0x93, 0x17, 0x68, 0x77);

    /// <summary>
    /// Represents the hashspace ID for the SHA3-384 hash algorithm.
    /// </summary>
    public static readonly Guid Sha3D384 = new Guid(
        // 872fb339-2636-4bdd-bda6-b6dc2a82b1b3
        0x872fb339, 0x2636, 0x4bdd,
        0xbd, 0xa6, 0xb6, 0xdc, 0x2a, 0x82, 0xb1, 0xb3);

    /// <summary>
    /// Represents the hashspace ID for the SHA3-512 hash algorithm.
    /// </summary>
    public static readonly Guid Sha3D512 = new Guid(
        // a4920a5d-a8a6-426c-8d14-a6cafbe64c7b
        0xa4920a5d, 0xa8a6, 0x426c,
        0x8d, 0x14, 0xa6, 0xca, 0xfb, 0xe6, 0x4c, 0x7b);

    /// <summary>
    /// Represents the hashspace ID for the SHAKE128 hash algorithm.
    /// </summary>
    public static readonly Guid Shake128 = new Guid(
        // 7ea218f6-629a-425f-9f88-7439d63296bb
        0x7ea218f6, 0x629a, 0x425f,
        0x9f, 0x88, 0x74, 0x39, 0xd6, 0x32, 0x96, 0xbb);

    /// <summary>
    /// Represents the hashspace ID for the SHAKE256 hash algorithm.
    /// </summary>
    public static readonly Guid Shake256 = new Guid(
        // 2e7fc6a4-2919-4edc-b0ba-7d7062ce4f0a
        0x2e7fc6a4, 0x2919, 0x4edc,
        0xb0, 0xba, 0x7d, 0x70, 0x62, 0xce, 0x4f, 0x0a);
}
#endif
