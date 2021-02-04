using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAMCIS.GeoJSON.Wkb
{
    public class EndianAwareBinaryWriter : BinaryWriter
    {
        #region Private Fields

        private readonly Endianness _endianness = Endianness.LITTLE;

        #endregion

        #region Constructors

        public EndianAwareBinaryWriter(Stream input) : base(input)
        {
        }

        public EndianAwareBinaryWriter(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public EndianAwareBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public EndianAwareBinaryWriter(Stream input, Endianness endianness) : base(input)
        {
            _endianness = endianness;
        }

        public EndianAwareBinaryWriter(Stream input, Encoding encoding, Endianness endianness) : base(input, encoding)
        {
            _endianness = endianness;
        }

        public EndianAwareBinaryWriter(Stream input, Encoding encoding, bool leaveOpen, Endianness endianness) : base(input, encoding, leaveOpen)
        {
            _endianness = endianness;
        }

        #endregion

        #region Public Override Methods

        public override void Write(byte value) => Write(value, this._endianness);

        public override void Write(byte[] buffer) => Write(buffer, this._endianness);

        public override void Write(char ch) => Write(ch, this._endianness);

        public override void Write(double value) => Write(value, this._endianness);

        public override void Write(float value) => Write(value, this._endianness);

        public override void Write(int value) => Write(value, this._endianness);

        public override void Write(long value) => Write(value, this._endianness);

        public override void Write(short value) => Write(value, this._endianness);

        public override void Write(uint value) => Write(value, this._endianness);

        public override void Write(ushort value) => Write(value, this._endianness);

        public override void Write(ulong value) => Write(value, this._endianness);

        public override void Write(sbyte value) => Write(value, this._endianness);

        public override void Write(bool value) => Write(value, this._endianness);

        public override void Write(char[] chars) => Write(chars, this._endianness);
        #endregion

        #region Public Methods

        public void Write(byte value, Endianness endianness) => this.WriteForEndianness(new byte[1] { value }, endianness);

        public void Write(byte[] value, Endianness endianness) => this.WriteForEndianness(value, endianness);

        public void Write(short value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(int value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(long value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(float value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(double value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(char value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(char[] chars, Endianness endianness) => this.WriteForEndianness(chars.Select(x => (byte)x).ToArray(), endianness);

        public void Write(UInt16 value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(UInt32 value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(UInt64 value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(bool value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void Write(sbyte value, Endianness endianness) => this.WriteForEndianness(BitConverter.GetBytes(value), endianness);

        public void WriteEndianness() => WriteEndianness(this._endianness);

        public void WriteEndianness(Endianness endianness) => this.Write((byte)endianness);

        #endregion

        #region Private Methods

        private EndianAwareBinaryWriter WriteForEndianness(byte[] bytesToWrite, Endianness endianness)
        {
            if ((endianness == Endianness.LITTLE && !BitConverter.IsLittleEndian)
                || (endianness == Endianness.BIG && BitConverter.IsLittleEndian))
            {
                Array.Reverse(bytesToWrite);
            }

            this.BaseStream.Write(bytesToWrite, 0, bytesToWrite.Length);

            return this;
        }

        #endregion
    }
}
