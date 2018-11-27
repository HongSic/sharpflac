using System;
using System.IO;
using SharpFlac.Blocks;
using SharpFlac.DataModel;
using SharpFlac.Enums;
using System.Linq;
using SharpFlac.Utils;

namespace SharpFlac
{
    public class FlacParser
    {
        const byte magicLength = 4;
        static readonly byte[] magic = new byte[magicLength] { 0x66, 0x4C, 0x61, 0x43 };

        private BitStream bitStream;

        public FlacMetaData ReadMetadata(Stream stream)
        {
            if (stream == null)
                throw new NullReferenceException("Illegal null-reference stream");

            FlacMetaData result = new FlacMetaData();

            bitStream = new BitStream(stream);

            if (!ValidateMagicNumber(bitStream))
                throw new Exception("Illegal input file. Magic number doesn't match to FLAC one");

            bool isLast = false;

            while (!isLast)
            {
                BlockHeader header = ReadBlockHeader(bitStream);

                switch (header.Type)
                {
                    case BlockType.STREAMINFO:
                        result.StreamInfo = ReadBlock(bitStream, header) as StreamInfo;
                        break;
                    case BlockType.SEEKTABLE:
                        result.SeekTable = ReadBlock(bitStream, header) as SeekTable;
                        break;
                    case BlockType.VORBIS_COMMENT:
                        result.VorbisComments = ReadBlock(bitStream, header) as VorbisComments;
                        break;
                    case BlockType.APPLICATION:
                        result.Application = ReadBlock(bitStream, header) as ApplicationBlock;
                        break;
                    case BlockType.PADDING:
                        result.Padding = ReadBlock(bitStream, header) as Padding;
                        break;
                    case BlockType.PICTURE:
                        result.Picture = ReadBlock(bitStream, header) as Picture;
                        break;
                    default:
                        SkipBlock(bitStream, header);
                        break;
                }

                isLast = header.IsLast;
            }

            return result;
        }

        public FlacMetaData ReadMetadata(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Specified path not found");

            FileStream stream = File.OpenRead(path);
            FlacMetaData metadata = null;

            try
            {
                 metadata = ReadMetadata(stream);
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return metadata;
        }

        public VorbisComments ReadVorbisComments(string path)
        {
            VorbisComments result = null;

            if (!File.Exists(path))
                throw new FileNotFoundException("Specified path not found");

            FileStream stream = File.OpenRead(path);
            bitStream = new BitStream(stream);

            try
            {
                if (!ValidateMagicNumber(bitStream))
                    return null;

                bool isLast = false;

                while (!isLast)
                {
                    BlockHeader header = ReadBlockHeader(bitStream);

                    switch (header.Type)
                    {
                        case BlockType.VORBIS_COMMENT:
                            result = ReadBlock(bitStream, header) as VorbisComments;
                            break;
                        default:
                            SkipBlock(bitStream, header);
                            break;
                    }

                    isLast = header.IsLast;
                }
            }
            catch { }
            finally
            {
                stream.Close();
            }

            return result;
        }

        private bool ValidateMagicNumber(BitStream stream)
        {
            if (stream == null)
                return false;

            byte[] buffer = new byte[magicLength];

            bitStream.readByteBlockAlignedNoCRC(buffer, 4);

            return buffer.SequenceEqual(magic);
        }

        private BlockHeader ReadBlockHeader(BitStream stream)
        {
            BlockHeader result = null;

            uint isLast = bitStream.readRawUInt(BlockHeader.LastFlagLength);
            uint blockType = bitStream.readRawUInt(BlockHeader.BlockTypeLength);
            uint blockSize = bitStream.readRawUInt(BlockHeader.BlockSizeLength);

            result = new BlockHeader()
            {
                IsLast = isLast == 1,
                Type = (BlockType)blockType,
                ContentSize = (int)blockSize
            };

            return result;
        }

        private void SkipBlock(BitStream stream, BlockHeader header)
        {
            if (stream == null || header == null)
                return;

            stream.readByteBlockAlignedNoCRC(null, header.ContentSize);
        }

        private MetadataBlock ReadBlock(BitStream stream, BlockHeader header)
        {
            if (stream == null || header == null)
                return null;

            MetadataBlock result = null;

            switch (header.Type)
            {
                case BlockType.STREAMINFO:
                    {
                        StreamInfo info = new StreamInfo(header);
                        if (info.Parse(stream))
                            result = info;
                    }
                    break;
                case BlockType.SEEKTABLE:
                    {
                        SeekTable st = new SeekTable(header);
                        if (st.Parse(stream))
                            result = st;
                    }
                    break;
                case BlockType.VORBIS_COMMENT:
                    {
                        VorbisComments comments = new VorbisComments(header);
                        if (comments.Parse(stream))
                            result = comments;
                    }
                    break;
                case BlockType.PADDING:
                    {
                        Padding padding = new Padding(header);
                        if (padding.Parse(stream))
                            result = padding;
                    }
                    break;
                case BlockType.PICTURE:
                    {
                        Picture picture = new Picture(header);
                        if (picture.Parse(stream))
                            result = picture;
                    }
                    break;
            }

            return result;
        }
    }
}
