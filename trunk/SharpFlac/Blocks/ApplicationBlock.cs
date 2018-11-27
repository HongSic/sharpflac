
using System;
using SharpFlac.Utils;
namespace SharpFlac.Blocks
{
    public class ApplicationBlock : MetadataBlock
    {
        public byte[] ApplicationID { get; set; }
        public byte[] ApplicationData { get; set; }

        public ApplicationBlock(BlockHeader header)
            : base(header)
        {
        }

        public bool Parse(BitStream stream)
        {
            stream.readByteBlockAlignedNoCRC(ApplicationID, 32);

            int dataLength = Header.ContentSize - 4;

            if (dataLength >= 0)
            {
                byte[] data = new byte[dataLength];
                stream.readByteBlockAlignedNoCRC(data, dataLength);
                ApplicationData = data;
            }

            return true;
        }
    }
}
