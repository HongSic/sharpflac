
using System;
using SharpFlac.Blocks;
using SharpFlac.DataModel;
using System.IO;
using SharpFlac.Utils;
using System.Diagnostics;
namespace SharpFlac.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            FlacParser p = new FlacParser();
            FlacMetaData meta = p.ReadMetadata(@"Data\testFile.flac");
            VorbisComments comments = meta.VorbisComments;

            foreach (VorbisComment c in comments)
            {
                Console.WriteLine(c);
            }
        }

        private static bool ReadBlockHeader(BitStream bitStream)
        {
            uint isLast = bitStream.readRawUInt(1);
            uint blockType = bitStream.readRawUInt(7);
            uint blockSize = bitStream.readRawUInt(24);

            Console.WriteLine(String.Format("{0},{1},{2}", isLast, blockType, blockSize));

            bitStream.readByteBlockAlignedNoCRC(null, (int)blockSize);

            return isLast == 1;
        }
    }


}
