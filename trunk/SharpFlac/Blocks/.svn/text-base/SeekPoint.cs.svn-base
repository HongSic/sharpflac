
using System;
namespace SharpFlac.Blocks
{
    public class SeekPoint
    {
        public uint Index { get; set; }
        public ulong FirstFrameSampleNumber { get; set; }
        public ulong ByteOffset { get; set; }
        public uint SamplesInTargetFrame { get; set; }

        public override string ToString()
        {
            return String.Format("point {0}: sampple_number={1}, stream_offset={2}, frame_sample={3}",
                Index, FirstFrameSampleNumber, ByteOffset, SamplesInTargetFrame);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            SeekPoint point = obj as SeekPoint;
            if (point == null)
                return false;

            bool success = Index == point.Index;
            success &= FirstFrameSampleNumber == point.FirstFrameSampleNumber;
            success &= ByteOffset == point.ByteOffset;
            success &= SamplesInTargetFrame == point.SamplesInTargetFrame;

            return success;
        }

        public override int GetHashCode()
        {
            if (fHashCode == 0)
            {
                fHashCode = 17 + 19 * (int)Index;
                fHashCode += 5 * (int)FirstFrameSampleNumber;
                fHashCode += 7 * (int)ByteOffset;
                fHashCode += 11 * (int)SamplesInTargetFrame;
            }

            return fHashCode;
        }

        private int fHashCode;
    }
}
