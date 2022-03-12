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
            (GuidVersion)((guid.TimeHi_Ver() & 0xF000) >> (3 * 4));

        /// <summary>
        /// 尝试获取当前 <see cref="Guid"/> 表示的时间戳。
        /// </summary>
        /// <param name="guid">要获取时间戳的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">当前 <see cref="Guid"/> 表示的时间戳。
        /// 如果 <see cref="Guid"/> 的版本不包含真实的时间戳，则为默认值。</param>
        /// <returns>若当前 <see cref="Guid"/> 的版本为
        /// <see cref="GuidVersion.Version1"/> 或 <see cref="GuidVersion.Version2"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static unsafe bool TryGetTimestamp(this Guid guid, out DateTime timestamp)
        {
            var version = guid.GetVersion();
            if (!version.IsTimeBased())
            {
                timestamp = default(DateTime);
                return false;
            }
            var tsField = ((long)guid.TimeLow()) |
                ((long)guid.TimeMid() << (4 * 8)) |
                ((long)guid.TimeHi_Ver() << (6 * 8));
            if (version.ContainsUserID())
            {
                tsField &= ~0xFFFFFFFFL;
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
        public static unsafe bool TryGetNodeID(this Guid guid, out byte[] nodeID)
        {
            nodeID = new byte[6];
            if (!guid.GetVersion().ContainsNodeID())
            {
                return false;
            }
            var pGuidNodeID = guid.NodeID();
            fixed (byte* pNodeID = &nodeID[0])
            {
                Buffer.MemoryCopy(pGuidNodeID, pNodeID, 6, 6);
            }
            return true;
        }

        /// <summary>
        /// 返回包含当前 <see cref="Guid"/> 的值的字节数组，其字节序符合 RFC 4122 UUID 标准。
        /// </summary>
        /// <param name="guid">要获取字节数组的 <see cref="Guid"/>。</param>
        /// <returns>包含 <paramref name="guid"/> 的值的 16 元素字节数组，
        /// 其 0-3、4-5 以及 6-7 字节均按照大端序排列，以符合 RFC 4122 UUID 标准。</returns>
        public static unsafe byte[] ToUuidByteArray(this Guid guid)
        {
            var uuidBytes = new byte[16];
            fixed (byte* pUuidBytes = &uuidBytes[0])
            {
                guid.WriteUuidBytes(pUuidBytes);
            }
            return uuidBytes;
        }

        /// <summary>
        /// 将当前 <see cref="Guid"/> 的值按字节写入指定地址，其字节序符合 RFC 4122 UUID 标准。
        /// </summary>
        /// <param name="guid">作为字节来源的 <see cref="Guid"/>。</param>
        /// <param name="destination">要写入 <see cref="Guid"/> 的字节的地址。</param>
        internal static unsafe void WriteUuidBytes(this Guid guid, byte* destination)
        {
            *(Guid*)destination = guid;
            if (BitConverter.IsLittleEndian)
            {
                var pGuidBytes = (byte*)&guid;
                destination[0] = pGuidBytes[3];
                destination[1] = pGuidBytes[2];
                destination[2] = pGuidBytes[1];
                destination[3] = pGuidBytes[0];
                destination[4] = pGuidBytes[5];
                destination[5] = pGuidBytes[4];
                destination[6] = pGuidBytes[7];
                destination[7] = pGuidBytes[6];
            }
        }
    }
}
