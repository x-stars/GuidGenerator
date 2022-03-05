using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供 <see cref="Guid"/> 的扩展方法。
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <param name="guid">要获取版本的 <see cref="Guid"/>。</param>
        /// <returns>当前 <see cref="Guid"/> 的版本。</returns>
        public static GuidVersion GetVersion(this Guid guid) =>
            (GuidVersion)((guid.ToByteArray()[7] & 0xF0) >> 4);

        /// <summary>
        /// 返回包含当前 <see cref="Guid"/> 的值的字节数组，其字节序符合 RFC 4122 UUID 标准。
        /// </summary>
        /// <param name="guid">要获取字节数组的 <see cref="Guid"/>。</param>
        /// <returns>包含 <paramref name="guid"/> 的值的 16 元素字节数组，
        /// 其 0-3、4-5 以及 6-7 字节均按照大端序排列，以符合 RFC 4122 UUID 标准。</returns>
        public static byte[] ToUuidByteArray(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            Array.Reverse(bytes, 0, 4);
            Array.Reverse(bytes, 4, 2);
            Array.Reverse(bytes, 6, 2);
            return bytes;
        }
    }
}
