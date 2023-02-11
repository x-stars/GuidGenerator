using System;

namespace XNetEx.Guids.Generators;

partial class CustomGuidGeneratorTest
{
    private sealed class TestCustomGuidGenerator : CustomGuidGenerator
    {
        public TestCustomGuidGenerator(
            DateTime epochDateTime = default,
            NodeIdSource nodeIdSource = default)
            : base(epochDateTime, nodeIdSource)
        {
        }

        public override Guid NewGuid()
        {
            var guid = Guid.Empty;
            this.FillVersionField(ref guid);
            this.FillVariantField(ref guid);
            return guid;
        }

        public new long GetCurrentTimestamp()
        {
            return base.GetCurrentTimestamp();
        }

        public new byte GetNodeIdByte(int index)
        {
            return base.GetNodeIdByte(index);
        }

        public new void GetNodeIdBytes(byte[] buffer)
        {
            base.GetNodeIdBytes(buffer);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public new void GetNodeIdBytes(Span<byte> buffer)
        {
            base.GetNodeIdBytes(buffer);
        }
#endif

        public new int GetRandomInt32()
        {
            return base.GetRandomInt32();
        }

        public new long GetRandomInt64()
        {
            return base.GetRandomInt64();
        }
    }
}
