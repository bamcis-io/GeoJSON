using System;
using System.IO;
using System.Text;

namespace BAMCIS.GeoJSON.Wkb
{
    /// <summary>
    /// An extension to the binary reader class that allows you to 
    /// specify the endianess of the binary data you are reading
    /// </summary>
    public class EndianAwareBinaryReader : BinaryReader
    {
        #region Private Fields

        private Endianness _endianness = Endianness.LITTLE;

        #endregion

        #region Constructors

        public EndianAwareBinaryReader(Stream input) : base(input)
        {
        }

        public EndianAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public EndianAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public EndianAwareBinaryReader(Stream input, Endianness endianness) : base(input)
        {
            _endianness = endianness;
        }

        public EndianAwareBinaryReader(Stream input, Encoding encoding, Endianness endianness) : base(input, encoding)
        {
            _endianness = endianness;
        }

        public EndianAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness) : base(input, encoding, leaveOpen)
        {
            _endianness = endianness;
        }

        #endregion

        #region Public Override Methods

        public override short ReadInt16() => ReadInt16(_endianness);

        public override int ReadInt32() => ReadInt32(_endianness);

        public override long ReadInt64() => ReadInt64(_endianness);

        public override ushort ReadUInt16() => ReadUInt16(_endianness);

        public override uint ReadUInt32() => ReadUInt32(_endianness);

        public override ulong ReadUInt64() => ReadUInt64(_endianness);

        public override double ReadDouble() => ReadDouble(_endianness);

        public override float ReadSingle() => ReadSingle(_endianness);

        public override bool ReadBoolean() => ReadBoolean(_endianness);

        public override char ReadChar() => ReadChar(_endianness);

        #endregion

        #region Public Methods

        public void SetEndianness(Endianness endianness)
        {
            this._endianness = endianness;
        }

        public short ReadInt16(Endianness endianness) => BitConverter.ToInt16(ReadForEndianness(sizeof(short), endianness), 0);

        public int ReadInt32(Endianness endianness) => BitConverter.ToInt32(ReadForEndianness(sizeof(int), endianness), 0);

        public long ReadInt64(Endianness endianness) => BitConverter.ToInt64(ReadForEndianness(sizeof(long), endianness), 0);

        public ushort ReadUInt16(Endianness endianness) => BitConverter.ToUInt16(ReadForEndianness(sizeof(ushort), endianness), 0);

        public uint ReadUInt32(Endianness endianness) => BitConverter.ToUInt32(ReadForEndianness(sizeof(uint), endianness), 0);

        public ulong ReadUInt64(Endianness endianness) => BitConverter.ToUInt64(ReadForEndianness(sizeof(ulong), endianness), 0);

        public float ReadSingle(Endianness endianness) => BitConverter.ToSingle(ReadForEndianness(sizeof(float), endianness), 0);

        public double ReadDouble(Endianness endianness)
        {
            byte[] temp = ReadForEndianness(sizeof(double), endianness);

            try
            {
               
                return BitConverter.ToDouble(temp, 0);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public char ReadChar(Endianness endianness) => BitConverter.ToChar(ReadForEndianness(sizeof(char), endianness), 0);

        public bool ReadBoolean(Endianness endianness) => BitConverter.ToBoolean(ReadForEndianness(sizeof(bool), endianness), 0);

        #endregion

        #region Private Methods

        private byte[] ReadForEndianness(int bytesToRead, Endianness endianness)
        {
            try
            {
                byte[] bytesRead = this.ReadBytes(bytesToRead);

                if ((endianness == Endianness.LITTLE && !BitConverter.IsLittleEndian)
                    || (endianness == Endianness.BIG && BitConverter.IsLittleEndian))
                {
                    Array.Reverse(bytesRead);
                }

                return bytesRead;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
