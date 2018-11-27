
using System;
using System.Collections.Generic;
using SharpFlac.Utils;
namespace SharpFlac.Blocks
{
    public class SeekTable : MetadataBlock, IEnumerable<SeekPoint>
    {
        public IList<SeekPoint> SeekPoints { get; set; }

        public SeekTable(BlockHeader header)
            : base(header)
        {
            SeekPoints = new List<SeekPoint>();
        }

        public bool Parse(BitStream stream)
        {
            int pointsCount = Header.ContentSize / PointLength;

            int offset = 0;

            for (int i = 0; i < pointsCount; ++i)
            {
                SeekPoint sp = new SeekPoint()
                {
                    Index = (uint)i
                };

                sp.FirstFrameSampleNumber = stream.readRawULong(64);
                sp.ByteOffset = stream.readRawULong(64);
                sp.SamplesInTargetFrame = stream.readRawUInt(16);


                SeekPoints.Add(sp);
            }

            return true;
        }

        #region IEnumerable<SeekPoint> Members

        public IEnumerator<SeekPoint> GetEnumerator()
        {
            return SeekPoints.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return SeekPoints.GetEnumerator();
        }

        #endregion

        private const int PointLength = 18;
    }
}
