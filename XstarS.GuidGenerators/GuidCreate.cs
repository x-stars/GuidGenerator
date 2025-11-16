using System;
using System.Security.Cryptography;
using System.Text;
using XNetEx.Guids.Generators;

namespace XNetEx.Guids;

static partial class Uuid
{
    extension(Guid)
    {
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 1.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 1.</returns>
        public static Guid NewVersion1()
        {
            return GuidGenerator.Version1.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 1
        /// using a non-volatile random node ID.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 1
        /// using a non-volatile random node ID.</returns>
        public static Guid NewVersion1R()
        {
            return GuidGenerator.Version1R.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 2
        /// based on the specified DCE Security domain and local ID.
        /// </summary>
        /// <param name="domain">The DCE Security domain used to generate the <see cref="Guid"/>.</param>
        /// <param name="localId">The local ID used to generate the <see cref="Guid"/>,
        /// or <see langword="null"/> to get the local user or group ID from the system.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 2
        /// generated based on <paramref name="domain"/> and <paramref name="localId"/>.</returns>
        /// <exception cref="PlatformNotSupportedException">
        /// The current operating system does not support getting the local user or group ID.</exception>
        public static Guid NewVersion2(DceSecurityDomain domain, int? localId = null)
        {
            return GuidGenerator.Version2.NewGuid(domain, localId);
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte array used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <see langword="null"/>.</exception>
        public static Guid NewVersion3(Guid nsId, byte[] name)
        {
            return GuidGenerator.Version3.NewGuid(nsId, name);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte span used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        public static Guid NewVersion3(Guid nsId, ReadOnlySpan<byte> name)
        {
            return GuidGenerator.Version3.NewGuid(nsId, name);
        }
#endif

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name string used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <see langword="null"/>.</exception>
        public static Guid NewVersion3(Guid nsId, string name, Encoding? encoding = null)
        {
            return GuidGenerator.Version3.NewGuid(nsId, name, encoding);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name character span used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 3
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        public static Guid NewVersion3(Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
        {
            return GuidGenerator.Version3.NewGuid(nsId, name, encoding);
        }
#endif

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 4.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 4.</returns>
        public static Guid NewVersion4()
        {
            return GuidGenerator.Version4.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte array used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <see langword="null"/>.</exception>
        public static Guid NewVersion5(Guid nsId, byte[] name)
        {
            return GuidGenerator.Version5.NewGuid(nsId, name);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte span used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        public static Guid NewVersion5(Guid nsId, ReadOnlySpan<byte> name)
        {
            return GuidGenerator.Version5.NewGuid(nsId, name);
        }
#endif

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name string used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is <see langword="null"/>.</exception>
        public static Guid NewVersion5(Guid nsId, string name, Encoding? encoding = null)
        {
            return GuidGenerator.Version5.NewGuid(nsId, name, encoding);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// based on the specified namespace ID and name.
        /// </summary>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name character span used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 5
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        public static Guid NewVersion5(Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
        {
            return GuidGenerator.Version5.NewGuid(nsId, name, encoding);
        }
#endif

#if !UUIDREV_DISABLE
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 6.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 6.</returns>
        public static Guid NewVersion6()
        {
            return GuidGenerator.Version6.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 6
        /// using a physical (IEEE 802 MAC) address node ID.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 6
        /// using a physical (IEEE 802 MAC) address node ID.</returns>
        public static Guid NewVersion6P()
        {
            return GuidGenerator.Version6P.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 6
        /// using a non-volatile random node ID.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 6
        /// using a non-volatile random node ID.</returns>
        public static Guid NewVersion6R()
        {
            return GuidGenerator.Version6R.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 7.
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 7.</returns>
        public static Guid NewVersion7()
        {
            return GuidGenerator.Version7.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// example implementation (RFC 9562 Appendix B.1).
        /// </summary>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// example implementation (RFC 9562 Appendix B.1).</returns>
        public static Guid NewVersion8()
        {
            return GuidGenerator.Version8.NewGuid();
        }

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// using the specified hash algorithm based on the specified namespace ID and name.
        /// </summary>
        /// <param name="hashing">The hash algorithm used to transform the input data.</param>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte array used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashing"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
        public static Guid NewVersion8N(HashAlgorithm hashing, Guid nsId, byte[] name)
        {
            return GuidGenerator.GetVersion8NOf(hashing).NewGuid(nsId, name);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// using the specified hash algorithm based on the specified namespace ID and name.
        /// </summary>
        /// <param name="hashing">The hash algorithm used to transform the input data.</param>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name byte span used to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
        public static Guid NewVersion8N(HashAlgorithm hashing, Guid nsId, ReadOnlySpan<byte> name)
        {
            return GuidGenerator.GetVersion8NOf(hashing).NewGuid(nsId, name);
        }
#endif

        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// using the specified hash algorithm based on the specified namespace ID and name.
        /// </summary>
        /// <param name="hashing">The hash algorithm used to transform the input data.</param>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name string used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 9562 UUID version 8
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashing"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
        public static Guid NewVersion8N(
            HashAlgorithm hashing, Guid nsId, string name, Encoding? encoding = null)
        {
            return GuidGenerator.GetVersion8NOf(hashing).NewGuid(nsId, name, encoding);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Generates a new <see cref="Guid"/> instance of RFC 4122 UUID version 8
        /// using the specified hash algorithm based on the specified namespace ID and name.
        /// </summary>
        /// <param name="hashing">The hash algorithm used to transform the input data.</param>
        /// <param name="nsId">The namespace ID used to generate the <see cref="Guid"/>.</param>
        /// <param name="name">The name character span used to generate the <see cref="Guid"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/> used to encode the name string.</param>
        /// <returns>A new <see cref="Guid"/> instance of RFC 4122 UUID version 8
        /// generated based on <paramref name="nsId"/> and <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="hashing"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The hash size of <paramref name="hashing"/> is less than 128 bits.</exception>
        public static Guid NewVersion8N(
            HashAlgorithm hashing, Guid nsId, ReadOnlySpan<char> name, Encoding? encoding = null)
        {
            return GuidGenerator.GetVersion8NOf(hashing).NewGuid(nsId, name, encoding);
        }
#endif
#endif
    }
}
