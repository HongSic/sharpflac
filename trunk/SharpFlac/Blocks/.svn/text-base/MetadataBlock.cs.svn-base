
using System;
namespace SharpFlac.Blocks
{
    public class MetadataBlock
    {
        public BlockHeader Header
        {
            get
            {
                return header;
            }
        }

        public MetadataBlock(BlockHeader header)
        {
            if (header == null)
                throw new NullReferenceException();

            this.header = header;
        }

        public byte[] GetEndianBytes(byte[] source, int start, int count)
        {
            byte[] result = new byte[count];
            Array.Copy(source, start, result, 0, count);
            Array.Reverse(result);

            return result;
        }

        public byte[] GetBytes(byte[] source, int start, int count)
        {
            byte[] result = new byte[count];
            Array.Copy(source, start, result, 0, count);


            return result;
        }

        private BlockHeader header;
    }
}
