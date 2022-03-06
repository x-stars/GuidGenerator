using System;

namespace XstarS.GuidGenerators
{
    /// <summary>
    /// 提供 <see cref="Guid"/> 的扩展方法。
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 表示 <see cref="Guid"/> 使用的基准时间戳。
        /// </summary>
        internal static readonly DateTime BaseTimestamp =
            new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取当前 <see cref="Guid"/> 的版本。
        /// </summary>
        /// <param name="guid">要获取版本的 <see cref="Guid"/>。</param>
        /// <returns>当前 <see cref="Guid"/> 的版本。</returns>
        public static GuidVersion GetVersion(this Guid guid) =>
            (GuidVersion)((guid.ToByteArray()[7] & 0xF0) >> 4);

        /// <summary>
        /// 尝试获取当前 <see cref="Guid"/> 表示的时间戳。
        /// </summary>
        /// <param name="guid">要获取时间戳的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">当前 <see cref="Guid"/> 表示的时间戳。
        /// 如果 <see cref="Guid"/> 的版本不包含真实的时间戳，则为默认值。</param>
        /// <returns>若当前 <see cref="Guid"/> 的版本为
        /// <see cref="GuidVersion.Version1"/> 或 <see cref="GuidVersion.Version2"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool TryGetTimestamp(this Guid guid, out DateTime timestamp)
        {
            var tsField = 0L;
            if (guid.GetVersion() == GuidVersion.Version1)
            {
                unsafe
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        tsField = ((long*)&guid)[0];
                    }
                    else
                    {
                        var uuidBytes = guid.ToUuidByteArray();
                        fixed (byte* pUuidBytes = &uuidBytes[0])
                        {
                            tsField = ((long*)pUuidBytes)[0];
                        }
                    }
                }
            }
            else if (guid.GetVersion() == GuidVersion.Version2)
            {
                unsafe
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        tsField = ((int*)&guid)[1];
                    }
                    else
                    {
                        var uuidBytes = guid.ToUuidByteArray();
                        fixed (byte* pUuidBytes = &uuidBytes[0])
                        {
                            tsField = ((int*)pUuidBytes)[1];
                        }
                    }
                }
                tsField = tsField << (4 * 8);
            }
            else
            {
                timestamp = default(DateTime);
                return false;
            }
            tsField &= ~((long)0xF0 << (7 * 8));
            var baseTicks = GuidExtensions.BaseTimestamp.Ticks;
            var tsTicks = tsField + baseTicks;
            timestamp = new DateTime(tsTicks, DateTimeKind.Utc);
            return true;
        }

        /// <summary>
        /// 尝试获取当前 <see cref="Guid"/> 表示的节点 ID。
        /// </summary>
        /// <param name="guid">要获取时间戳的 <see cref="Guid"/>。</param>
        /// <param name="nodeID">当前 <see cref="Guid"/> 表示的节点 ID。
        /// 如果 <see cref="Guid"/> 的版本不包含真实的节点 ID，则为默认值。</param>
        /// <returns>若当前 <see cref="Guid"/> 的版本为
        /// <see cref="GuidVersion.Version1"/> 或 <see cref="GuidVersion.Version2"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool TryGetNodeID(this Guid guid, out byte[] nodeID)
        {
            nodeID = new byte[6];
            if (!guid.GetVersion().ContainsNodeID())
            {
                return false;
            }
            var guidBytes = guid.ToByteArray();
            Array.Copy(guidBytes, 10, nodeID, 0, 6);
            return true;
        }

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
