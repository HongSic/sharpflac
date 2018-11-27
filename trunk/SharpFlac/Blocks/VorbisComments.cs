using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SharpFlac.Utils;

namespace SharpFlac.Blocks
{
    public class VorbisComments : MetadataBlock, IEnumerable<VorbisComment>
    {
        public string Vendor { get; set; }
        public IList<VorbisComment> Comments { get; set; }

        public VorbisComments(BlockHeader header)
            :base(header)
        {
            Comments = new List<VorbisComment>();
        }

        public bool Parse(BitStream stream)
        {
            bool result = false;

            try
            {
                uint vendorStringLength = stream.readRawIntLittleEndian();
                byte[] vendorStringBytes = new byte[vendorStringLength];

                stream.readByteBlockAlignedNoCRC(vendorStringBytes, (int)vendorStringLength);

                Vendor = Encoding.UTF8.GetString(vendorStringBytes);


                uint commentsCount = stream.readRawIntLittleEndian();

                for (uint i = 0; i < commentsCount; ++i)
                {
                    uint commentLength = stream.readRawIntLittleEndian();
                    byte[] commentBytes = new byte[commentLength];

                    stream.readByteBlockAlignedNoCRC(commentBytes, (int)commentLength);

                    string commentString = Encoding.UTF8.GetString(commentBytes);

                    Match m = commentsRegex.Match(commentString);
                    if (m.Success)
                    {
                        VorbisComment comment = new VorbisComment()
                        {
                            Key = m.Groups[1].Value,
                            Value = m.Groups[2].Value
                        };

                        Comments.Add(comment);
                    }
                }

                result = true;
            }
            catch { }

            return result;
        }

        #region IEnumerable<VorbisComment> Members

        public IEnumerator<VorbisComment> GetEnumerator()
        {
            return Comments.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Comments.GetEnumerator();
        }

        #endregion

        private static readonly Regex commentsRegex = new Regex(@"(.+)=(.+)");
    }

}
