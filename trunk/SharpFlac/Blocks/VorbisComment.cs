﻿
namespace SharpFlac.Blocks
{
    public class VorbisComment
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Key + "=" + Value;
        }
    }
}
