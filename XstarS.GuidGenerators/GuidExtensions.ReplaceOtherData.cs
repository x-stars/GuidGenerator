using System;
using XNetEx.Guids.Components;

namespace XNetEx.Guids;

static partial class GuidExtensions
{
    /// <summary>
    /// Replaces the hash data of the current <see cref="Guid"/>
    /// with the specified byte array if the <see cref="Guid"/> is name-based.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="hashData">The hash data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the hash data replaced with <paramref name="hashData"/>,
    /// or the original value if the <see cref="Guid"/> is not name-based.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashData"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="hashData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceHashData(this Guid guid, byte[] hashData)
    {
        if (hashData is null)
        {
            throw new ArgumentNullException(nameof(hashData));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guid.ReplaceHashData((ReadOnlySpan<byte>)hashData);
#else
        if (hashData.Length != 16)
        {
            throw new ArgumentException(
                "Hash data for Guid must be exactly 16 bytes long.",
                nameof(hashData));
        }
        if (!guid.GetRfc4122Version().IsNameBased())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetHashData(ref result, hashData);
        return result;
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Replaces the hash data of the current <see cref="Guid"/>
    /// with the specified byte span if the <see cref="Guid"/> is name-based.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="hashData">The hash data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the hash data replaced with <paramref name="hashData"/>,
    /// or the original value if the <see cref="Guid"/> is not name-based.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="hashData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceHashData(this Guid guid, ReadOnlySpan<byte> hashData)
    {
        if (hashData.Length != 16)
        {
            throw new ArgumentException(
                "Hash data for Guid must be exactly 16 bytes long.",
                nameof(hashData));
        }
        if (!guid.GetRfc4122Version().IsNameBased())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetHashData(ref result, hashData);
        return result;
    }
#endif

    /// <summary>
    /// Replaces the random data of the current <see cref="Guid"/>
    /// with the specified byte array if the <see cref="Guid"/> is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="randomData">The random data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the random data replaced with <paramref name="randomData"/>,
    /// or the original value if the <see cref="Guid"/> is not generated randomly.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="randomData"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="randomData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceRandomData(this Guid guid, byte[] randomData)
    {
        if (randomData is null)
        {
            throw new ArgumentNullException(nameof(randomData));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guid.ReplaceRandomData((ReadOnlySpan<byte>)randomData);
#else
        if (randomData.Length != 16)
        {
            throw new ArgumentException(
                "Random data for Guid must be exactly 16 bytes long.",
                nameof(randomData));
        }
        if (!guid.GetRfc4122Version().IsRandomized())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        components.SetRandomData(ref result, randomData);
        return result;
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Replaces the random data of the current <see cref="Guid"/>
    /// with the specified byte span if the <see cref="Guid"/> is generated randomly.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="randomData">The random data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the random data replaced with <paramref name="randomData"/>,
    /// or the original value if the <see cref="Guid"/> is not generated randomly.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="randomData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceRandomData(this Guid guid, ReadOnlySpan<byte> randomData)
    {
        if (randomData.Length != 16)
        {
            throw new ArgumentException(
                "Random data for Guid must be exactly 16 bytes long.",
                nameof(randomData));
        }
        if (!guid.GetRfc4122Version().IsRandomized())
        {
            return guid;
        }

        var result = guid;
        var version = guid.GetVersion();
        var components = GuidComponents.OfVersion(version);
        components.SetRandomData(ref result, randomData);
        return result;
    }
#endif

#if !UUIDREV_DISABLE
    /// <summary>
    /// Replaces the custom data of the current <see cref="Guid"/>
    /// with the specified byte array if the <see cref="Guid"/> contains custom data.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="customData">The custom data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the custom data replaced with <paramref name="customData"/>,
    /// or the original value if the <see cref="Guid"/> does not contain custom data.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="customData"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="customData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceCustomData(this Guid guid, byte[] customData)
    {
        if (customData is null)
        {
            throw new ArgumentNullException(nameof(customData));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return guid.ReplaceCustomData((ReadOnlySpan<byte>)customData);
#else
        if (customData.Length != 16)
        {
            throw new ArgumentException(
                "Custom data for Guid must be exactly 16 bytes long.",
                nameof(customData));
        }
        if (!guid.GetRfc4122Version().IsCustomized())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetCustomData(ref result, customData);
        return result;
#endif
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Replaces the custom data of the current <see cref="Guid"/>
    /// with the specified byte span if the <see cref="Guid"/> contains custom data.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/>.</param>
    /// <param name="customData">The custom data to use as replacement.</param>
    /// <returns>A new <see cref="Guid"/> instance that is equivalent to the current <see cref="Guid"/>
    /// except that the custom data replaced with <paramref name="customData"/>,
    /// or the original value if the <see cref="Guid"/> does not contain custom data.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="customData"/> is not 16 bytes long.</exception>
    public static Guid ReplaceCustomData(this Guid guid, ReadOnlySpan<byte> customData)
    {
        if (customData.Length != 16)
        {
            throw new ArgumentException(
                "Custom data for Guid must be exactly 16 bytes long.",
                nameof(customData));
        }
        if (!guid.GetRfc4122Version().IsCustomized())
        {
            return guid;
        }

        var result = guid;
        var components = GuidComponents.FixedFormat;
        components.SetCustomData(ref result, customData);
        return result;
    }
#endif
#endif
}
