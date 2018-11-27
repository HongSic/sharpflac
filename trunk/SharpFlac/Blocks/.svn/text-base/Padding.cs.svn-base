
using SharpFlac.Utils;
namespace SharpFlac.Blocks
{
    public class Padding : MetadataBlock
    {
        public Padding(BlockHeader header)
            : base(header)
        {
        }

        public bool Parse(BitStream stream)
        {
            stream.readByteBlockAlignedNoCRC(null, Header.ContentSize);

            return true;
        }
    }
}
