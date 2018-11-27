using System;
using SharpFlac.Utils;

namespace SharpFlac.Blocks
{
    public class StreamInfo : MetadataBlock
    {
        public uint MinBlockSize { get; set; }
        public uint MaxBlockSize { get; set; }
        public uint MinFrameSize { get; set; }
        public uint MaxFrameSize { get; set; }
        public uint SampleRate { get; set; }
        public uint NumberOfChannels { get; set; }
        public uint BitsPerSample { get; set; }
        public ulong TotalSamplesCount { get; set; }
        public byte[] MD5 { get; set; }

        public StreamInfo(BlockHeader header)
            : base(header)
        {
        }

        public bool Parse(BitStream stream)
        {
            MinBlockSize = stream.readRawUInt(16);
            MaxBlockSize = stream.readRawUInt(16);
            MinFrameSize = stream.readRawUInt(24);
            MaxFrameSize = stream.readRawUInt(24);
            SampleRate = stream.readRawUInt(20);
            NumberOfChannels = stream.readRawUInt(3) + 1;
            BitsPerSample = stream.readRawUInt(5) + 1;
            TotalSamplesCount = stream.readRawUInt(36);

            MD5 = new byte[16];

            stream.readByteBlockAlignedNoCRC(MD5, 16);

            return true;
        }
    }
}
