using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using SharpFlac.DataModel;
using SharpFlac.Blocks;
using System.Text.RegularExpressions;
using SharpFlac.Enums;

namespace SharpFlac
{
    [TestFixture]
    public class MetadataTests
    {
        private const string filePath = @"Data\testFile.flac";
        private MemoryStream cachedFile;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            cachedFile = new MemoryStream(File.ReadAllBytes(filePath));
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            cachedFile.Close();
        }

        [SetUp]
        public void Setup()
        {
            cachedFile.Seek(0, SeekOrigin.Begin);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void NotExistingFile_Parse_Exception()
        {
            string path = "not_existing_file";
            Assert.False(File.Exists(path));

            FlacParser parser = new FlacParser();
            parser.ReadMetadata(path);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullStream_Parse_Exception()
        {
            Stream stream = null;

            FlacParser parser = new FlacParser();
            parser.ReadMetadata(stream);
        }

        [Test]
        public void ReadMetadataGeneral_Success()
        {
            var metadata = ReadMetadata();
            Assert.NotNull(metadata);

            var padding = metadata.Padding;
            Assert.NotNull(padding);
            Assert.AreEqual(2832, padding.Header.ContentSize);
        }

        [Test]
        public void ReadStreamInfo_Success()
        {
            var metadata = ReadMetadata();
            Assert.NotNull(metadata);

            var streamInfo = metadata.StreamInfo;
            Assert.NotNull(streamInfo);
            Assert.AreEqual(streamInfo.Header.ContentSize, 34);

            /*  
              type: 0 (STREAMINFO)
              is last: false
              length: 34
              minimum blocksize: 4096 samples
              maximum blocksize: 4096 samples
              minimum framesize: 1801 bytes
              maximum framesize: 13710 bytes
              sample_rate: 44100 Hz
              channels: 2
              bits-per-sample: 16
              total samples: 10071852
              MD5 signature: 94 63 4c b9 00 14 39 3c 5d d1 0b 1e a8 b6 60 dc
             */

            Assert.False(streamInfo.Header.IsLast);
            Assert.AreEqual(streamInfo.MinBlockSize, 4096);
            Assert.AreEqual(streamInfo.MaxBlockSize, 4096);
            Assert.AreEqual(streamInfo.MinFrameSize, 1801);
            Assert.AreEqual(streamInfo.MaxFrameSize, 13710);
            Assert.AreEqual(streamInfo.SampleRate, 44100);
            Assert.AreEqual(streamInfo.NumberOfChannels, 2);
            Assert.AreEqual(streamInfo.BitsPerSample, 16);
            Assert.AreEqual(streamInfo.TotalSamplesCount, 10071852);

            Assert.NotNull(streamInfo.MD5);
            Assert.AreEqual(streamInfo.MD5.Length, 16);
            Assert.True(
                streamInfo.MD5.SequenceEqual(
                new byte[] { 0x94, 0x63, 0x4c, 0xb9, 0x00, 0x14, 0x39, 0x3c, 0x5d, 0xd1, 0x0b, 0x1e, 0xa8, 0xb6, 0x60, 0xdc }));
        }

        [Test]
        public void ReadSeekTable_Success()
        {
            var metadata = ReadMetadata();
            Assert.NotNull(metadata);

            var seekTable = metadata.SeekTable;
            Assert.NotNull(seekTable);
            Assert.AreEqual(seekTable.Header.ContentSize, 414);

            /*
                type: 3 (SEEKTABLE)
                is last: false
                length: 414
                seek points: 23
                ...
             */

            SeekPoint[] seekPoints = GetSeekPoints();
            SeekPoint[] seekTablePoints = seekTable.ToArray();
            Assert.AreEqual(seekPoints.Length, seekTablePoints.Length);
            Assert.True(seekPoints.SequenceEqual(seekTablePoints));
        }

        [Test]
        public void ReadVorbisComment_Success()
        {
            var metadata = ReadMetadata();
            Assert.NotNull(metadata);

            var comments = metadata.VorbisComments;
            Assert.NotNull(comments);
            Assert.AreEqual(comments.Header.ContentSize, 216);

            /*
              type: 4 (VORBIS_COMMENT)
              is last: false
              length: 216
              vendor string: reference libFLAC 1.2.1 20070917
              comments: 7
              comment[0]: TITLE=Wanna Get Closer
              comment[1]: ARTIST=Jeff Kashiwa, Kim Waters, Steve Cole
              comment[2]: ALBUM=The Sax Pack, The Pack Is Back
              comment[3]: DATE=2009
              comment[4]: TRACKNUMBER=01
              comment[5]: TRACKTOTAL=10
              comment[6]: GENRE=Blues
             */

            Assert.AreEqual("reference libFLAC 1.2.1 20070917", comments.Vendor);
            Assert.AreEqual(7, comments.Count());

            var ca = comments.ToArray();
            Assert.AreEqual(ca[0].Key, "TITLE");
            Assert.AreEqual(ca[0].Value, "Wanna Get Closer");
            Assert.AreEqual(ca[1].Key, "ARTIST");
            Assert.AreEqual(ca[1].Value, "Jeff Kashiwa, Kim Waters, Steve Cole");
            Assert.AreEqual(ca[2].Key, "ALBUM");
            Assert.AreEqual(ca[2].Value, "The Sax Pack, The Pack Is Back");
            Assert.AreEqual(ca[3].Key, "DATE");
            Assert.AreEqual(ca[3].Value, "2009");
            Assert.AreEqual(ca[4].Key, "TRACKNUMBER");
            Assert.AreEqual(ca[4].Value, "01");
            Assert.AreEqual(ca[5].Key, "TRACKTOTAL");
            Assert.AreEqual(ca[5].Value, "10");
            Assert.AreEqual(ca[6].Key, "GENRE");
            Assert.AreEqual(ca[6].Value, "Blues");
        }

        [Test]
        public void ReadPicture_Success()
        {
            var metadata = ReadMetadata();
            Assert.NotNull(metadata);

            var picture = metadata.Picture;
            Assert.NotNull(picture);
            Assert.AreEqual(picture.Header.ContentSize, 5180);

            /*
              type: 6 (PICTURE)
              is last: false
              length: 5180
              type: 3 (Cover (front))
              MIME type: image/jpeg
              description: 
              width: 75
              height: 75
              depth: 24
              colors: 0 (unindexed)
              data length: 5138
             */

            Assert.AreEqual(PictureType.CoverFront, picture.PictureType);
            Assert.AreEqual("image/jpeg", picture.MimeType);
            Assert.AreEqual(String.Empty, picture.Description);
            Assert.AreEqual(75, picture.Height);
            Assert.AreEqual(75, picture.Width);
            Assert.AreEqual(24, picture.ColorDepth);
            Assert.AreEqual(0, picture.ColorsCount);
            Assert.NotNull(picture.Data);
            Assert.AreEqual(5138, picture.Data.Length);
        }

        private SeekPoint[] GetSeekPoints()
        {
            Regex seekRegex = new Regex(@"point (\d+): sample_number=(\d+), stream_offset=(\d+), frame_samples=(\d+)");

            List<string> seekPoints = new List<string>()
            {
                "point 0: sample_number=0, stream_offset=0, frame_samples=4096",
                "point 1: sample_number=438272, stream_offset=1147177, frame_samples=4096",
                "point 2: sample_number=880640, stream_offset=2328989, frame_samples=4096",
                "point 3: sample_number=1318912, stream_offset=3531434, frame_samples=4096",
                "point 4: sample_number=1761280, stream_offset=4733408, frame_samples=4096",
                "point 5: sample_number=2203648, stream_offset=5929765, frame_samples=4096",
                "point 6: sample_number=2641920, stream_offset=7093116, frame_samples=4096",
                "point 7: sample_number=3084288, stream_offset=8259280, frame_samples=4096",
                "point 8: sample_number=3526656, stream_offset=9496955, frame_samples=4096",
                "point 9: sample_number=3964928, stream_offset=10736053, frame_samples=4096",
                "point 10: sample_number=4407296, stream_offset=11940726, frame_samples=4096",
                "point 11: sample_number=4849664, stream_offset=13170306, frame_samples=4096",
                "point 12: sample_number=5287936, stream_offset=14353505, frame_samples=4096",
                "point 13: sample_number=5730304, stream_offset=15533936, frame_samples=4096",
                "point 14: sample_number=6172672, stream_offset=16728336, frame_samples=4096",
                "point 15: sample_number=6610944, stream_offset=17942907, frame_samples=4096",
                "point 16: sample_number=7053312, stream_offset=19199603, frame_samples=4096",
                "point 17: sample_number=7495680, stream_offset=20433943, frame_samples=4096",
                "point 18: sample_number=7933952, stream_offset=21679179, frame_samples=4096",
                "point 19: sample_number=8376320, stream_offset=22866286, frame_samples=4096",
                "point 20: sample_number=8818688, stream_offset=24110961, frame_samples=4096",
                "point 21: sample_number=9256960, stream_offset=25317926, frame_samples=4096",
                "point 22: sample_number=9699328, stream_offset=26408116, frame_samples=4096"
            };

            List<SeekPoint> result = new List<SeekPoint>();

            foreach (var s in seekPoints)
            {
                Match m = seekRegex.Match(s);
                if (m.Success)
                {
                    result.Add(new SeekPoint()
                    {
                        Index = m.Groups[1].Value.ToUInt(),
                        FirstFrameSampleNumber = m.Groups[2].Value.ToULong(),
                        ByteOffset = m.Groups[3].Value.ToULong(),
                        SamplesInTargetFrame = m.Groups[4].Value.ToUInt()
                    });
                }
            }

            return result.ToArray();
        }

        private FlacMetaData ReadMetadata()
        {
            FlacParser parser = new FlacParser();
            return parser.ReadMetadata(cachedFile);
        }
    }

    public static class StringExtensions
    {
        public static uint ToUInt(this string str)
        {
            return UInt32.Parse(str);
        }

        public static ulong ToULong(this string str)
        {
            return UInt64.Parse(str);
        }
    }
}
