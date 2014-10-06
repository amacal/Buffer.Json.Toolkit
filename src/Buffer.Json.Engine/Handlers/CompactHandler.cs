using System.IO;

namespace Buffer.Json.Engine.Handlers
{
    public class CompactHandler : IHandler
    {
        private int length;
        private int position;

        private readonly byte[] buffer;
        private readonly Stream stream;

        public CompactHandler(Stream stream, int bufferSize)
        {
            this.stream = stream;
            this.buffer = new byte[bufferSize];

            this.length = bufferSize;
            this.position = 0;
        }

        public void HandleBegin()
        {
        }

        public void HandleArrayBegin()
        {
            this.Write(0x5b); // [
        }

        public void HandleArraySeparator()
        {
            this.Write(0x2c); // ,
        }

        public void HandleArrayEnd()
        {
            this.Write(0x5d); // ]
        }

        public void HandleObjectBegin()
        {
            this.Write(0x7b); // {
        }

        public void HandleObjectProperty()
        {
        }

        public void HandleObjectPropertySeparator()
        {
            this.Write(0x3a); // :
        }

        public void HandleObjectValue()
        {
        }

        public void HandleObjectSeparator()
        {
            this.Write(0x2c); // ,
        }

        public void HandleObjectEnd(int count)
        {
            this.Write(0x7d); // }
        }

        public void HandleStringBegin()
        {
            this.Write(0x22);
        }

        public void HandleStringEscapeCharacter()
        {
            this.Write(0x5c);
        }

        public void HandleStringEscapeValue(byte value)
        {
            this.Write(value);
        }

        public void HandleStringEscapeUnicode(byte[] value)
        {
            foreach (byte item in value)
            {
                this.Write(item);
            }
        }

        public void HandleStringCharacter(byte value)
        {
            this.Write(value);
        }

        public void HandleStringEnd()
        {
            this.Write(0x22);
        }

        public void HandleNumberSign()
        {
            this.Write(0x2d);
        }

        public void HandleNumberValue(byte value)
        {
            this.Write(value);
        }

        public void HandleNumberSeparator()
        {
            this.Write(0x2e);
        }

        public void HandleNumberDecimal(byte value)
        {
            this.Write(value);
        }

        public void HandleNumberExponent(byte value)
        {
            this.Write(value);
        }

        public void HandleNumberExponentSign(byte value)
        {
            this.Write(value);
        }

        public void HandleNumberExponentValue(byte value)
        {
            this.Write(value);
        }

        public void HandleNullValue()
        {
            this.Write(0x6e);
            this.Write(0x75);
            this.Write(0x6c);
            this.Write(0x6c);
        }

        public void HandleTrueValue()
        {
            this.Write(0x74);
            this.Write(0x72);
            this.Write(0x75);
            this.Write(0x65);
        }

        public void HandleFalseValue()
        {
            this.Write(0x66);
            this.Write(0x61);
            this.Write(0x6c);
            this.Write(0x73);
            this.Write(0x65);
        }

        public void HandleWhiteCharacter(byte value)
        {
        }

        public void HandleEnd()
        {
            this.Flush();
        }

        private void Write(byte value)
        {
            if (this.position == this.length)
            {
                this.stream.Write(this.buffer, 0, this.length);
                this.position = 0;
            }

            this.buffer[this.position++] = value;
        }

        private void Flush()
        {
            if (this.position > 0)
            {
                this.stream.Write(this.buffer, 0, this.position);
                this.position = 0;
            }
        }
    }
}
