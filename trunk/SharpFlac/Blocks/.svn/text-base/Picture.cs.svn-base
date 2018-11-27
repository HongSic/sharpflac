
using System;
using System.Text;
using SharpFlac.Enums;
using SharpFlac.Utils;
namespace SharpFlac.Blocks
{
    public class Picture : MetadataBlock
    {
        public PictureType PictureType { get; set; }
        public string MimeType { get; set; }
        public string Description { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint ColorDepth { get; set; } // bits-per-pixel
        public uint ColorsCount { get; set; } // for indexed-color pictures (e.g. GIF) only
        public byte[] Data { get; set; }

        public Picture(BlockHeader header)
            : base(header)
        {
        }

        public bool Parse(BitStream stream)
        {
            PictureType = (PictureType)stream.readRawUInt(32);
            
            uint mimeTypeLength = stream.readRawUInt(32);
            byte[] mimeTypeBytes = new byte[mimeTypeLength];
            stream.readByteBlockAlignedNoCRC(mimeTypeBytes, (int)mimeTypeLength);
            MimeType = Encoding.ASCII.GetString(mimeTypeBytes);

            uint descriptionLength = stream.readRawUInt(32);
            byte[] descriptionBytes = new byte[descriptionLength];
            stream.readByteBlockAlignedNoCRC(descriptionBytes, (int)descriptionLength);
            Description = Encoding.UTF8.GetString(descriptionBytes);

            Width = stream.readRawUInt(32);
            Height = stream.readRawUInt(32);
            ColorDepth = stream.readRawUInt(32);
            ColorsCount = stream.readRawUInt(32);

            uint dataLength = stream.readRawUInt(32);
            Data = new byte[dataLength];
            stream.readByteBlockAlignedNoCRC(Data, (int)dataLength);

            return true;
        }
    }
}
