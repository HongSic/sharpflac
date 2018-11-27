using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace SharpFlac.Utils
{
    public sealed class CRC16
    {

        // CRC-16, poly = x^16 + x^15 + x^2 + x^0, init = 0
        private static readonly ushort[] CRC16_TABLE =
            new ushort[] {
            0x0000,
            0x8005,
            0x800f,
            0x000a,
            0x801b,
            0x001e,
            0x0014,
            0x8011,
            0x8033,
            0x0036,
            0x003c,
            0x8039,
            0x0028,
            0x802d,
            0x8027,
            0x0022,
            0x8063,
            0x0066,
            0x006c,
            0x8069,
            0x0078,
            0x807d,
            0x8077,
            0x0072,
            0x0050,
            0x8055,
            0x805f,
            0x005a,
            0x804b,
            0x004e,
            0x0044,
            0x8041,
            0x80c3,
            0x00c6,
            0x00cc,
            0x80c9,
            0x00d8,
            0x80dd,
            0x80d7,
            0x00d2,
            0x00f0,
            0x80f5,
            0x80ff,
            0x00fa,
            0x80eb,
            0x00ee,
            0x00e4,
            0x80e1,
            0x00a0,
            0x80a5,
            0x80af,
            0x00aa,
            0x80bb,
            0x00be,
            0x00b4,
            0x80b1,
            0x8093,
            0x0096,
            0x009c,
            0x8099,
            0x0088,
            0x808d,
            0x8087,
            0x0082,
            0x8183,
            0x0186,
            0x018c,
            0x8189,
            0x0198,
            0x819d,
            0x8197,
            0x0192,
            0x01b0,
            0x81b5,
            0x81bf,
            0x01ba,
            0x81ab,
            0x01ae,
            0x01a4,
            0x81a1,
            0x01e0,
            0x81e5,
            0x81ef,
            0x01ea,
            0x81fb,
            0x01fe,
            0x01f4,
            0x81f1,
            0x81d3,
            0x01d6,
            0x01dc,
            0x81d9,
            0x01c8,
            0x81cd,
            0x81c7,
            0x01c2,
            0x0140,
            0x8145,
            0x814f,
            0x014a,
            0x815b,
            0x015e,
            0x0154,
            0x8151,
            0x8173,
            0x0176,
            0x017c,
            0x8179,
            0x0168,
            0x816d,
            0x8167,
            0x0162,
            0x8123,
            0x0126,
            0x012c,
            0x8129,
            0x0138,
            0x813d,
            0x8137,
            0x0132,
            0x0110,
            0x8115,
            0x811f,
            0x011a,
            0x810b,
            0x010e,
            0x0104,
            0x8101,
            0x8303,
            0x0306,
            0x030c,
            0x8309,
            0x0318,
            0x831d,
            0x8317,
            0x0312,
            0x0330,
            0x8335,
            0x833f,
            0x033a,
            0x832b,
            0x032e,
            0x0324,
            0x8321,
            0x0360,
            0x8365,
            0x836f,
            0x036a,
            0x837b,
            0x037e,
            0x0374,
            0x8371,
            0x8353,
            0x0356,
            0x035c,
            0x8359,
            0x0348,
            0x834d,
            0x8347,
            0x0342,
            0x03c0,
            0x83c5,
            0x83cf,
            0x03ca,
            0x83db,
            0x03de,
            0x03d4,
            0x83d1,
            0x83f3,
            0x03f6,
            0x03fc,
            0x83f9,
            0x03e8,
            0x83ed,
            0x83e7,
            0x03e2,
            0x83a3,
            0x03a6,
            0x03ac,
            0x83a9,
            0x03b8,
            0x83bd,
            0x83b7,
            0x03b2,
            0x0390,
            0x8395,
            0x839f,
            0x039a,
            0x838b,
            0x038e,
            0x0384,
            0x8381,
            0x0280,
            0x8285,
            0x828f,
            0x028a,
            0x829b,
            0x029e,
            0x0294,
            0x8291,
            0x82b3,
            0x02b6,
            0x02bc,
            0x82b9,
            0x02a8,
            0x82ad,
            0x82a7,
            0x02a2,
            0x82e3,
            0x02e6,
            0x02ec,
            0x82e9,
            0x02f8,
            0x82fd,
            0x82f7,
            0x02f2,
            0x02d0,
            0x82d5,
            0x82df,
            0x02da,
            0x82cb,
            0x02ce,
            0x02c4,
            0x82c1,
            0x8243,
            0x0246,
            0x024c,
            0x8249,
            0x0258,
            0x825d,
            0x8257,
            0x0252,
            0x0270,
            0x8275,
            0x827f,
            0x027a,
            0x826b,
            0x026e,
            0x0264,
            0x8261,
            0x0220,
            0x8225,
            0x822f,
            0x022a,
            0x823b,
            0x023e,
            0x0234,
            0x8231,
            0x8213,
            0x0216,
            0x021c,
            0x8219,
            0x0208,
            0x820d,
            0x8207,
            0x0202 };

        /**
         * Update the CRC with the byte data.
         * 
         * @param data  The byte data
         * @param crc   The starting CRC value
         * @return      The updated CRC value
         */
        public static short update(byte data, short crc)
        {
            crc = (short)((crc << 8) ^ CRC16_TABLE[((crc >> 8) ^ data) & 0xff]);
            return crc;
        }

        /**
         * Update the CRC with the byte array data.
         * 
         * @param data  The byte array data
         * @param len   The byte array length
         * @param crc   The starting CRC value
         * @return      The updated CRC value
         */
        public static short updateBlock(byte[] data, int len, short crc)
        {
            for (int i = 0; i < len; i++)
                crc = (short)((crc << 8) ^ CRC16_TABLE[(crc >> 8) ^ data[i]]);
            return crc;
        }


        /**
         * Calculate the CRC over a byte array.
         * 
         * @param data  The byte data
         * @param len   The byte array length
         * @return      The calculated CRC value
         */
        public static short calc(byte[] data, int len)
        {
            short crc = 0;

            for (int i = 0; i < len; i++)
                crc = (short)((crc << 8) ^ CRC16_TABLE[(crc >> 8) ^ data[i]]);

            return crc;
        }
    }

    public class ByteData
    {
        private const int DEFAULT_BUFFER_SIZE = 256;

        /** The byte array where data is stored. */
        private byte[] data;

        /** The number of bytes stored in the array. */
        private int len;

        /**
         * The default constructor.
         * @param maxSpace  The maximum space in the internal byte array.
         */
        public ByteData(int maxSpace)
        {
            if (maxSpace <= 0) maxSpace = DEFAULT_BUFFER_SIZE;
            data = new byte[maxSpace];
            len = 0;
        }

        /**
         * Append byte to storage.
         * @param b byte to extend
         */
        public void append(byte b)
        {
            data[len++] = b;
        }

        /**
         * @return Returns the data.
         */
        public byte[] getData()
        {
            return data;
        }

        /**
         * Return a data byte.
         * @param idx   The data byte to return
         * @return Returns the data.
         */
        public byte getData(int idx)
        {
            return data[idx];
        }

        /**
         * @return Returns the len.
         */
        public int getLen()
        {
            return len;
        }

        /**
         * Set the length of this ByteData object without re-allocating the underlying array.
         * It is not possible to set the length larger than the underlying byte array. 
         */
        public void setLen(int len)
        {
            if (len > data.Length)
            {
                len = data.Length;
            }
            this.len = len;
        }
    }

    public class BitStream
    {
        private const int BITS_PER_BLURB = 8;
        private const int BITS_PER_BLURB_LOG2 = 3;
        private const byte BLURB_TOP_BIT_ONE = ((byte)0x80);

        private const int BUFFER_CHUNK_SIZE = 1024;
        private byte[] buffer = new byte[BUFFER_CHUNK_SIZE];
        private int putByte = 0;
        private int getByte = 0;
        private int getBit = 0;
        private int availBits = 0;
        private int totalBitsRead = 0;

        private short readCRC16 = 0;

        private Stream inStream;

        public BitStream(Stream istream)
        {
            this.inStream = istream;
        }

        private int readFromStream()
        {
            // first shift the unconsumed buffer data toward the front as much as possible
            if (getByte > 0 && putByte > getByte)
            {
                Array.Copy(buffer, getByte, buffer, 0, putByte - getByte);
                //System.arraycopy(buffer, getByte, buffer, 0, putByte - getByte);
            }
            putByte -= getByte;
            getByte = 0;

            // set the target for reading, taking into account blurb alignment
            // blurb == byte, so no gyrations necessary:
            int bytes = buffer.Length - putByte;

            // finally, read in some data
            bytes = inStream.Read(buffer, putByte, bytes);
            if (bytes <= 0)
                throw new Exception();

            // now we have to handle partial blurb cases:
            // blurb == byte, so no gyrations necessary:
            putByte += bytes;
            availBits += bytes << 3;
            return bytes;
        }

        /**
         * Reset the bit stream.
         */
        public void reset()
        {
            getByte = 0;
            getBit = 0;
            putByte = 0;
            availBits = 0;
        }

        /**
         * Reset the read CRC-16 value.
         * @param seed  The initial CRC-16 value
         */
        public void resetReadCRC16(short seed)
        {
            readCRC16 = seed;
        }

        /**
         * return the read CRC-16 value.
         * @return  The read CRC-16 value
         */
        public short getReadCRC16()
        {
            return readCRC16;
        }

        /**
         * Test if the Bit Stream consumed bits is byte aligned.
         * @return  True of bit stream consumed bits is byte aligned
         */
        public bool isConsumedByteAligned()
        {
            return ((getBit & 7) == 0);
        }

        /**
         * return the number of bits to read to align the byte.
         * @return  The number of bits to align the byte
         */
        public int bitsLeftForByteAlignment()
        {
            return 8 - (getBit & 7);
        }

        /**
         * return the number of bytes left to read.
         * @return  The number of bytes left to read
         */
        public int getInputBytesUnconsumed()
        {
            return availBits >> 3;
        }

        /**
         * skip over bits in bit stream without updating CRC.
         * @param bits  Number of bits to skip
         * @throws IOException  Thrown if error reading from input stream
         */
        public void skipBitsNoCRC(int bits)
        {
            if (bits == 0) return;
            int bitsToAlign = getBit & 7;
            if (bitsToAlign != 0)
            {
                int bitsToTake = Math.Min(8 - bitsToAlign, bits);
                readRawUInt(bitsToTake);
                bits -= bitsToTake;
            }
            int bytesNeeded = bits / 8;
            if (bytesNeeded > 0)
            {
                readByteBlockAlignedNoCRC(null, bytesNeeded);
                bits %= 8;
            }
            if (bits > 0)
            {
                readRawUInt(bits);
            }
        }

        /**
         * read a single bit.
         * @return  The bit
         * @throws IOException  Thrown if error reading input stream
         */
        public int readBit()
        {
            while (true)
            {
                if (availBits > 0)
                {
                    int val = ((buffer[getByte] & (0x80 >> getBit)) != 0) ? 1 : 0;
                    getBit++;
                    if (getBit == BITS_PER_BLURB)
                    {
                        readCRC16 = CRC16.update(buffer[getByte], readCRC16);
                        getByte++;
                        getBit = 0;
                    }
                    availBits--;
                    totalBitsRead++;
                    return val;
                }
                else
                {
                    readFromStream();
                }
            }
        }

        /**
         * read a bit into an integer value.
         * The bits of the input integer are shifted left and the 
         * read bit is placed into bit 0.
         * @param val   The integer to shift and add read bit
         * @return      The updated integer value
         * @throws IOException  Thrown if error reading input stream
         */
        public uint readBitToInt(uint val)
        {
            while (true)
            {
                if (availBits > 0)
                {
                    val <<= 1;
                    val |= (uint)(((buffer[getByte] & (0x80 >> getBit)) != 0) ? 1 : 0);
                    getBit++;
                    if (getBit == BITS_PER_BLURB)
                    {
                        readCRC16 = CRC16.update(buffer[getByte], readCRC16);
                        getByte++;
                        getBit = 0;
                    }
                    availBits--;
                    totalBitsRead++;
                    return val;
                }
                else
                {
                    readFromStream();
                }
            }
        }

        /**
         * peek at the next bit and add it to the input integer.
         * The bits of the input integer are shifted left and the 
         * read bit is placed into bit 0.
         * @param val   The input integer
         * @param bit   The bit to peek at
         * @return      The updated integer value
         * @throws IOException  Thrown if error reading input stream
         */
        public uint peekBitToInt(uint val, int bit)
        {
            while (true)
            {
                if (bit < availBits)
                {
                    val <<= 1;
                    if ((getBit + bit) >= BITS_PER_BLURB)
                    {
                        bit = (getBit + bit) % BITS_PER_BLURB;
                        val |= (uint)(((buffer[getByte + 1] & (0x80 >> bit)) != 0) ? 1 : 0);
                    }
                    else
                    {
                        val |= (uint)(((buffer[getByte] & (0x80 >> (getBit + bit))) != 0) ? 1 : 0);
                    }
                    return val;
                }
                else
                {
                    readFromStream();
                }
            }
        }

        /**
         * read a bit into a long value.
         * The bits of the input long are shifted left and the 
         * read bit is placed into bit 0.
         * @param val   The long to shift and add read bit
         * @return      The updated long value
         * @throws IOException  Thrown if error reading input stream
         */
        public ulong readBitToLong(ulong val)
        {
            while (true)
            {
                if (availBits > 0)
                {
                    val <<= 1;
                    val |= (ulong)(((buffer[getByte] & (0x80 >> getBit)) != 0) ? 1 : 0);
                    getBit++;
                    if (getBit == BITS_PER_BLURB)
                    {
                        readCRC16 = CRC16.update(buffer[getByte], readCRC16);
                        getByte++;
                        getBit = 0;
                    }
                    availBits--;
                    totalBitsRead++;
                    return val;
                }
                else
                {
                    readFromStream();
                }
            }
        }

        /**
         * read bits into an unsigned integer.
         * @param bits  The number of bits to read
         * @return      The bits as an unsigned integer
         * @throws IOException  Thrown if error reading input stream
         */
        public uint readRawUInt(int bits)
        {
            uint val = 0;
            for (int i = 0; i < bits; i++)
            {
                val = readBitToInt(val);
            }
            return val;
        }

        /**
         * peek at bits into an unsigned integer without advancing the input stream.
         * @param bits  The number of bits to read
         * @return      The bits as an unsigned integer
         * @throws IOException  Thrown if error reading input stream
         */
        public uint peekRawUInt(int bits)
        {
            uint val = 0;
            for (int i = 0; i < bits; i++)
            {
                val = peekBitToInt(val, i);
            }
            return val;
        }

        /**
         * read bits into a signed integer.
         * @param bits  The number of bits to read
         * @return      The bits as a signed integer
         * @throws IOException  Thrown if error reading input stream
         */
        public uint readRawInt(int bits)
        {
            if (bits == 0) { return 0; }
            uint uval = 0;
            for (int i = 0; i < bits; i++)
            {
                uval = readBitToInt(uval);
            }

            // fix the sign
            uint val;
            int bitsToleft = 32 - bits;
            if (bitsToleft != 0)
            {
                uval <<= bitsToleft;
                val = uval;
                val >>= bitsToleft;
            }
            else
            {
                val = uval;
            }
            return val;
        }

        /**
         * read bits into an unsigned long.
         * @param bits  The number of bits to read
         * @return      The bits as an unsigned long
         * @throws IOException  Thrown if error reading input stream
         */
        public ulong readRawULong(int bits)
        {
            ulong val = 0;
            for (int i = 0; i < bits; i++)
            {
                val = readBitToLong(val);
            }
            return val;
        }

        /**
         * read bits into an unsigned little endian integer.
         * @return      The bits as an unsigned integer
         * @throws IOException  Thrown if error reading input stream
         */
        public uint readRawIntLittleEndian()
        {
            uint x32 = readRawUInt(8);
            uint x8 = readRawUInt(8);
            x32 |= (x8 << 8);
            x8 = readRawUInt(8);
            x32 |= (x8 << 16);
            x8 = readRawUInt(8);
            x32 |= (x8 << 24);
            return x32;
        }

        /**
         * Read a block of bytes (aligned) without updating the CRC value.
         * @param val   The array to receive the bytes. If null, no bytes are returned
         * @param nvals The number of bytes to read
         * @throws IOException  Thrown if error reading input stream
         */
        public void readByteBlockAlignedNoCRC(byte[] val, int nvals)
        {
            int destlength = nvals;
            while (nvals > 0)
            {
                int chunk = Math.Min(nvals, putByte - getByte);
                if (chunk == 0)
                {
                    readFromStream();
                }
                else
                {
                    if (val != null) Array.Copy(buffer, getByte, val, destlength - nvals, chunk);
                    nvals -= chunk;
                    getByte += chunk;
                    //totalConsumedBits = (getByte << BITS_PER_BLURB_LOG2);
                    availBits -= (chunk << BITS_PER_BLURB_LOG2);
                    totalBitsRead += (chunk << BITS_PER_BLURB_LOG2);
                }
            }
        }

        /**
         * Read and count the number of zero bits.
         * @return  The number of zero bits read
         * @throws IOException  Thrown if error reading input stream
         */
        public int readUnaryUnsigned()
        {
            int val = 0;
            while (true)
            {
                int bit = readBit();
                if (bit != 0) break;
                val++;
            }
            return val;
        }

        /**
         * Read a Rice Signal Block.
         * @param vals  The values to be returned
         * @param pos   The starting position in the vals array
         * @param nvals The number of values to return
         * @param parameter The Rice parameter
         * @throws IOException  On read error
         */
        public void readRiceSignedBlock(int[] vals, int pos, int nvals, int parameter)
        {
            int j, valI = 0;
            int cbits = 0, uval = 0, msbs = 0, lsbsLeft = 0;
            byte blurb, saveBlurb;
            int state = 0; // 0 = getting unary MSBs, 1 = getting binary LSBs
            if (nvals == 0) return;
            int i = getByte;

            long startBits = getByte * 8 + getBit;

            // We unroll the main loop to take care of partially consumed blurbs here.
            if (getBit > 0)
            {
                saveBlurb = blurb = buffer[i];
                cbits = getBit;
                blurb <<= cbits;
                while (true)
                {
                    if (state == 0)
                    {
                        if (blurb != 0)
                        {
                            for (j = 0; (blurb & BLURB_TOP_BIT_ONE) == 0; j++)
                                blurb <<= 1;
                            msbs += j;

                            // dispose of the unary end bit
                            blurb <<= 1;
                            j++;
                            cbits += j;
                            uval = 0;
                            lsbsLeft = parameter;
                            state++;
                            //totalBitsRead += msbs;
                            if (cbits == BITS_PER_BLURB)
                            {
                                cbits = 0;
                                readCRC16 = CRC16.update(saveBlurb, readCRC16);
                                break;
                            }
                        }
                        else
                        {
                            msbs += BITS_PER_BLURB - cbits;
                            cbits = 0;
                            readCRC16 = CRC16.update(saveBlurb, readCRC16);
                            //totalBitsRead += msbs;
                            break;
                        }
                    }
                    else
                    {
                        int availableBits = BITS_PER_BLURB - cbits;
                        if (lsbsLeft >= availableBits)
                        {
                            uval <<= availableBits;
                            uval |= ((blurb & 0xff) >> cbits);
                            cbits = 0;
                            readCRC16 = CRC16.update(saveBlurb, readCRC16);
                            //totalBitsRead += availableBits;
                            if (lsbsLeft == availableBits)
                            {
                                // compose the value
                                uval |= (msbs << parameter);
                                if ((uval & 1) != 0)
                                    vals[pos + valI++] = -((int)(uval >> 1)) - 1;
                                else
                                    vals[pos + valI++] = (int)(uval >> 1);
                                if (valI == nvals)
                                    break;
                                msbs = 0;
                                state = 0;
                            }
                            lsbsLeft -= availableBits;
                            break;
                        }
                        else
                        {
                            uval <<= lsbsLeft;
                            uval |= ((blurb & 0xff) >> (BITS_PER_BLURB - lsbsLeft));
                            blurb <<= lsbsLeft;
                            cbits += lsbsLeft;
                            //totalBitsRead += lsbsLeft;
                            // compose the value
                            uval |= (msbs << parameter);
                            if ((uval & 1) != 0)
                                vals[pos + valI++] = -((int)(uval >> 1)) - 1;
                            else
                                vals[pos + valI++] = (int)(uval >> 1);
                            if (valI == nvals)
                            {
                                // back up one if we exited the for loop because we
                                // read all nvals but the end came in the middle of
                                // a blurb
                                i--;
                                break;
                            }
                            msbs = 0;
                            state = 0;
                        }
                    }
                }
                i++;
                getByte = i;
                getBit = cbits;
                //totalConsumedBits = (i << BITS_PER_BLURB_LOG2) | cbits;
                //totalBitsRead += (BITS_PER_BLURB) | cbits;
            }

            // Now that we are blurb-aligned the logic is slightly simpler
            while (valI < nvals)
            {
                for (; i < putByte && valI < nvals; i++)
                {
                    saveBlurb = blurb = buffer[i];
                    cbits = 0;
                    while (true)
                    {
                        if (state == 0)
                        {
                            if (blurb != 0)
                            {
                                for (j = 0; (blurb & BLURB_TOP_BIT_ONE) == 0; j++) blurb <<= 1;
                                msbs += j;
                                // dispose of the unary end bit
                                blurb <<= 1;
                                j++;
                                cbits += j;
                                uval = 0;
                                lsbsLeft = parameter;
                                state++;
                                //totalBitsRead += msbs;
                                if (cbits == BITS_PER_BLURB)
                                {
                                    cbits = 0;
                                    readCRC16 = CRC16.update(saveBlurb, readCRC16);
                                    break;
                                }
                            }
                            else
                            {
                                msbs += BITS_PER_BLURB - cbits;
                                cbits = 0;
                                readCRC16 = CRC16.update(saveBlurb, readCRC16);
                                //totalBitsRead += msbs;
                                break;
                            }
                        }
                        else
                        {
                            int availableBits = BITS_PER_BLURB - cbits;
                            if (lsbsLeft >= availableBits)
                            {
                                uval <<= availableBits;
                                uval |= ((blurb & 0xff) >> cbits);
                                cbits = 0;
                                readCRC16 = CRC16.update(saveBlurb, readCRC16);
                                //totalBitsRead += availableBits;
                                if (lsbsLeft == availableBits)
                                {
                                    // compose the value
                                    uval |= (msbs << parameter);
                                    if ((uval & 1) != 0)
                                        vals[pos + valI++] = -((int)(uval >> 1)) - 1;
                                    else
                                        vals[pos + valI++] = (int)(uval >> 1);
                                    if (valI == nvals)
                                        break;
                                    msbs = 0;
                                    state = 0;
                                }
                                lsbsLeft -= availableBits;
                                break;
                            }
                            else
                            {
                                uval <<= lsbsLeft;
                                uval |= ((blurb & 0xff) >> (BITS_PER_BLURB - lsbsLeft));
                                blurb <<= lsbsLeft;
                                cbits += lsbsLeft;
                                //totalBitsRead += lsbsLeft;
                                // compose the value
                                uval |= (msbs << parameter);
                                if ((uval & 1) != 0)
                                    vals[pos + valI++] = -((int)(uval >> 1)) - 1;
                                else
                                    vals[pos + valI++] = (int)(uval >> 1);
                                if (valI == nvals)
                                {
                                    // back up one if we exited the for loop because
                                    // we read all nvals but the end came in the
                                    // middle of a blurb
                                    i--;
                                    break;
                                }
                                msbs = 0;
                                state = 0;
                            }
                        }
                    }
                }
                getByte = i;
                getBit = cbits;
                //totalConsumedBits = (i << BITS_PER_BLURB_LOG2) | cbits;
                //totalBitsRead += (BITS_PER_BLURB) | cbits;
                if (valI < nvals)
                {
                    long endBits = getByte * 8 + getBit;
                    int delta = (int)(endBits - startBits);
                        //System.out.println("SE0 "+startBits+" "+endBits);
                    totalBitsRead += delta;
                    availBits -= delta;
                    readFromStream();
                    // these must be zero because we can only get here if we got to
                    // the end of the buffer
                    i = 0;
                    startBits = getByte * 8 + getBit;

                    /*
                    totalBitsRead += endBits - startBits;
                    availBits -= endBits - startBits;
                    readFromStream();
                    // these must be zero because we can only get here if we got to
                    // the end of the buffer
                    i = 0;
                    startBits = getByte * 8 + getBit;*/
                }
            }

            long endBits2 = getByte * 8 + getBit;
            int delta2 = (int)(endBits2 - startBits);
            totalBitsRead += delta2;
            availBits -= delta2;
            //long endBits = getByte * 8 + getBit;
            //totalBitsRead += endBits - startBits;
            //availBits -= endBits - startBits;
        }

        /**
         * read UTF8 integer.
         * on return, if *val == 0xffffffff then the utf-8 sequence was invalid, but
         * the return value will be true
         * @param raw   The raw bytes read (output). If null, no bytes are returned
         * @return      The integer read
         * @throws IOException  Thrown if error reading input stream
         */
        public uint readUTF8Int(ByteData raw)
        {
            uint val;
            uint v = 0;
            uint x;
            uint i;
            x = readRawUInt(8);
            if (raw != null) raw.append((byte)x);
            if ((x & 0x80) == 0)
            { // 0xxxxxxx
                v = x;
                i = 0;
            }
            else if (((x & 0xC0) != 0) && ((x & 0x20) == 0))
            { // 110xxxxx
                v = x & 0x1F;
                i = 1;
            }
            else if (((x & 0xE0) != 0) && ((x & 0x10) == 0))
            { // 1110xxxx
                v = x & 0x0F;
                i = 2;
            }
            else if (((x & 0xF0) != 0) && ((x & 0x08) == 0))
            { // 11110xxx
                v = x & 0x07;
                i = 3;
            }
            else if (((x & 0xF8) != 0) && ((x & 0x04) == 0))
            { // 111110xx
                v = x & 0x03;
                i = 4;
            }
            else if (((x & 0xFC) != 0) && ((x & 0x02) == 0))
            { // 1111110x
                v = x & 0x01;
                i = 5;
            }
            else
            {
                val = 0xffffffff;
                return val;
            }
            for (; i > 0; i--)
            {
                x = peekRawUInt(8);
                if (((x & 0x80) == 0) || ((x & 0x40) != 0))
                { // 10xxxxxx
                    val = 0xffffffff;
                    return val;
                }
                x = readRawUInt(8);
                if (raw != null)
                    raw.append((byte)x);
                v <<= 6;
                v |= (x & 0x3F);
            }
            val = v;
            return val;
        }

        /**
         * read UTF long.
         * on return, if *val == 0xffffffffffffffff then the utf-8 sequence was
         * invalid, but the return value will be true
         * @param raw   The raw bytes read (output). If null, no bytes are returned
         * @return      The long read
         * @throws IOException  Thrown if error reading input stream
         */
        public ulong readUTF8Long(ByteData raw)
        {
            ulong v = 0;
            uint x;
            int i;
            ulong val;
            x = readRawUInt(8);
            if (raw != null)
                raw.append((byte)x);
            if (((x & 0x80) == 0))
            { // 0xxxxxxx
                v = x;
                i = 0;
            }
            else if (((x & 0xC0) != 0) && ((x & 0x20) == 0))
            { // 110xxxxx
                v = x & 0x1F;
                i = 1;
            }
            else if (((x & 0xE0) != 0) && ((x & 0x10) == 0))
            { // 1110xxxx
                v = x & 0x0F;
                i = 2;
            }
            else if (((x & 0xF0) != 0) && ((x & 0x08) == 0))
            { // 11110xxx
                v = x & 0x07;
                i = 3;
            }
            else if (((x & 0xF8) != 0) && ((x & 0x04) == 0))
            { // 111110xx
                v = x & 0x03;
                i = 4;
            }
            else if (((x & 0xFC) != 0) && ((x & 0x02) == 0))
            { // 1111110x
                v = x & 0x01;
                i = 5;
            }
            else if (((x & 0xFE) != 0) && ((x & 0x01) == 0))
            { // 11111110
                v = 0;
                i = 6;
            }
            else
            {
                val = 0xffffffffffffffffL;
                return val;
            }
            for (; i > 0; i--)
            {
                x = peekRawUInt(8);
                if (((x & 0x80) == 0) || ((x & 0x40) != 0))
                { // 10xxxxxx
                    val = 0xffffffffffffffffL;
                    return val;
                }
                x = readRawUInt(8);
                if (raw != null)
                    raw.append((byte)x);
                v <<= 6;
                v |= (x & 0x3F);
            }
            val = v;
            return val;
        }

        /**
         * Total Blurbs read.
         * @return Returns the total blurbs read.
         */
        public int getTotalBytesRead()
        {
            return ((totalBitsRead + 7) / 8);
        }
    }
}

