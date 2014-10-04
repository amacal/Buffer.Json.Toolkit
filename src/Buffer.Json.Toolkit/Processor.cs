using System;
using System.IO;

namespace Buffer.Json.Toolkit
{
    public class Processor
    {
        private static readonly byte[] WhiteCharacters = new byte[]
        {
            0x09, 0x0a, 0x0b, 0x0d, 0x20
        };

        private static readonly byte[] HexdecimalDigits = new byte[] 
        { 
            0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 
            0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39
        };

        private static readonly byte[] Digits = new byte[]
        {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39
        };

        private readonly Stream inStream;
        private readonly Stream outStream;

        private int inLength;
        private int inPosition;
        private readonly byte[] inBuffer;

        private int outLength;
        private int outPosition;
        private readonly byte[] outBuffer;

        private int intent;

        public Processor(Stream input, Stream output, int bufferSize)
        {
            this.inStream = input;
            this.outStream = output;

            this.inLength = 0;
            this.inPosition = 0;
            this.inBuffer = new byte[bufferSize];

            this.outLength = bufferSize;
            this.outPosition = 0;
            this.outBuffer = new byte[bufferSize];

            this.intent = 0;
        }

        public void Beautify()
        {
            if (this.Any())
            {
                this.HandleValue();
                this.Flush();
            }
        }

        private void HandleValue()
        {
            this.Assert();

            switch (this.Peak())
            {
                case 0x5b: // [
                    this.HandleArray();
                    break;

                case 0x7b: // {
                    this.HandleObject();
                    break;

                case 0x22: // "
                    this.HandleString();
                    break;

                case 0x6e: // n
                    this.HandleNull();
                    break;

                case 0x74: // t
                    this.HandleTrue();
                    break;

                case 0x66: // f
                    this.HandleFalse();
                    break;

                case 0x30:
                case 0x31:
                case 0x32:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                case 0x39:
                case 0x2d: // -
                    this.HandleNumber();
                    break;
            }
        }

        private void HandleArray()
        {
            this.Read();
            this.Write(0x5b); // [

            this.HandleWhiteCharacters();
            this.HandleItems();

            this.WriteIntent();
            this.Assert(0x5d);
            this.Write(0x5d); // ]
            this.Read();
        }

        private void HandleItems()
        {
            bool isFirst = true;
            this.Assert();

            while (this.Peak() != 0x5d) // ]
            {
                if (isFirst == false)
                {
                    this.Assert(0x2c); // ,
                    this.Write(0x2c); // ,
                    this.Read();

                    this.HandleWhiteCharacters();
                    this.WriteWhiteCharacter();
                }

                this.HandleValue();
                this.HandleWhiteCharacters();

                isFirst = false;
            }
        }

        private void HandleObject()
        {
            this.Read();
            this.Write(0x7b); // {

            this.BeginIntent();
            this.HandleProperties();
            this.EndIntent();

            this.WriteNewLine();
            this.WriteIntent();

            this.Assert(0x7d);
            this.Write(0x7d); // }
            this.Read();
        }

        private void HandleProperties()
        {
            bool isFirst = true;
            this.Assert();

            while (this.Peak() != 0x7d) // }
            {
                if (isFirst == false)
                {
                    this.Assert(0x2c);
                    this.Write(0x2c); // ,
                    this.Read();
                }

                this.WriteNewLine();
                this.WriteIntent();

                this.HandleWhiteCharacters();
                this.HandleString();

                this.HandleWhiteCharacters();
                this.WriteWhiteCharacter();

                this.Assert(0x3a); // :
                this.Write(0x3a); // :
                this.Read();

                this.HandleWhiteCharacters();
                this.WriteWhiteCharacter();

                this.HandleValue();
                this.HandleWhiteCharacters();

                isFirst = false;
            }
        }

        private void HandleString()
        {
            this.Assert(0x22); // "
            this.Write(0x22);
            this.Read();

            while (true)
            {
                this.Assert();

                if (this.Peak() == 0x22)
                {
                    break;
                }

                this.HandleCharacter();
            }

            this.Assert(0x22); // "
            this.Write(0x22);
            this.Read();
        }

        private void HandleCharacter()
        {
            if (this.Peak() == 0x5c) // \
            {
                this.Write(0x5c);
                this.Read();

                if (this.Any())
                {
                    switch (this.Peak())
                    {
                        case 0x22: // "
                        case 0x5c: // \
                        case 0x2f: // /
                        case 0x62: // backspace
                        case 0x66: // form feed
                        case 0x6e: // new line
                        case 0x72: // carriage return
                        case 0x74: // horizontal tab
                            this.HandleControlCharacter();
                            break;

                        case 0x75: // unicode
                            break;
                    }
                }
            }
            else
            {
                this.Write(this.Read());
            }
        }

        private void HandleControlCharacter()
        {
            this.Write(0x5c);
            this.Write(this.Read());
        }

        private void HandleUnicodeCharacter()
        {
            this.Write(0x5c);
            this.Write(0x75);
            this.Read();

            this.HandleHexdecimalDigits(4);
        }

        private void HandleHexdecimalDigits(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Assert(Processor.HexdecimalDigits);
                this.Write(this.Read());
            }
        }

        private void HandleNumber()
        {
            if (this.Peak() == 0x2d)
            {
                this.Read();
                this.Write(0x2d);
                this.Assert();
            }

            while (this.Any() && this.Is(Processor.Digits))
            {
                this.Write(this.Read());
            }

            if (this.Any() && this.Peak() == 0x2e)
            {
                this.Write(0x2e);
                this.Read();
                this.Assert();

                while (this.Any() && this.Is(Processor.Digits))
                {
                    this.Write(this.Read());
                }

                if (this.Any() && (this.Peak() == 0x66 || this.Peak() == 0x45))
                {
                    this.Write(this.Read());
                    this.Assert();

                    if (this.Peak() == 0x2b || this.Peak() == 0x2d)
                    {
                        this.Write(this.Read());
                        this.Assert();
                    }

                    while (this.Any() && this.Is(Processor.Digits))
                    {
                        this.Write(this.Read());
                    }
                }
            }

            this.HandleWhiteCharacters();
        }

        private void HandleNull()
        {
            this.Assert(0x6e);
            this.Write(0x6e);
            this.Read();

            this.Assert(0x75);
            this.Write(0x75);
            this.Read();

            this.Assert(0x6c);
            this.Write(0x6c);
            this.Read();

            this.Assert(0x6c);
            this.Write(0x6c);
            this.Read();

            this.HandleWhiteCharacters();
        }

        private void HandleTrue()
        {
            this.Assert(0x74);
            this.Write(0x74);
            this.Read();

            this.Assert(0x72);
            this.Write(0x72);
            this.Read();

            this.Assert(0x75);
            this.Write(0x75);
            this.Read();

            this.Assert(0x65);
            this.Write(0x65);
            this.Read();

            this.HandleWhiteCharacters();
        }

        private void HandleFalse()
        {
            this.Assert(0x66);
            this.Write(0x66);
            this.Read();

            this.Assert(0x61);
            this.Write(0x61);
            this.Read();

            this.Assert(0x6c);
            this.Write(0x6c);
            this.Read();

            this.Assert(0x73);
            this.Write(0x73);
            this.Read();

            this.Assert(0x65);
            this.Write(0x65);
            this.Read();

            this.HandleWhiteCharacters();
        }

        private void HandleWhiteCharacters()
        {
            while (this.Any())
            {
                if (Array.BinarySearch(Processor.WhiteCharacters, this.Peak()) < 0)
                {
                    break;
                }

                this.Read();
            }
        }

        private void BeginIntent()
        {
            this.intent++;
        }

        private void EndIntent()
        {
            this.intent--;
        }

        private void WriteIntent()
        {
            for (int i = 0; i < this.intent; i++)
            {
                this.Write(0x09);
            }
        }

        private void WriteNewLine()
        {
            this.Write(0x0d);
            this.Write(0x0a);
        }

        private void WriteWhiteCharacter()
        {
            this.Write(0x20);
        }

        private void Assert()
        {
            if (this.Any() == false)
            {
                throw new ProcessorException(
                    String.Format(
                        "JSON is invalid. End of data."), 0);
            }
        }

        private void Assert(byte value)
        {
            if (this.Any() == false)
            {
                throw new ProcessorException(
                    String.Format(
                        "JSON is invalid. End of data. Expected '{0}'.", value), 0);
            }

            if (this.Peak() != value)
            {
                throw new ProcessorException(
                    String.Format(
                        "JSON is invalid. Unexpected data. Expected '{0}' instead of '{1}'.", value, this.Peak()), 0);
            }
        }

        private void Assert(byte[] values)
        {
            if (this.Any() == false)
            {
                throw new ProcessorException(
                    String.Format(
                        "JSON is invalid. End of data. Expected '{0}'.", String.Join(";", values)), 0);
            }

            foreach (byte value in values)
            {
                if (this.Peak() == value)
                {
                    return;
                }
            }

            throw new ProcessorException(
                String.Format(
                    "JSON is invalid. Unexpected data. Expected '{0}' instead of '{1}'.", String.Join(";", values), this.Peak()), 0);
        }

        private bool Is(byte[] values)
        {
            foreach (byte value in values)
            {
                if (this.Peak() == value)
                {
                    return true;
                }
            }

            return false;
        }

        private bool Any()
        {
            this.Buffer();

            return this.inLength > 0;
        }

        private byte Peak()
        {
            this.Buffer();

            return this.inBuffer[this.inPosition];
        }

        private byte Read()
        {
            this.Buffer();

            return this.inBuffer[this.inPosition++];
        }

        private void Buffer()
        {
            if (this.inPosition == this.inLength)
            {
                this.inPosition = 0;
                this.inLength = this.inStream.Read(this.inBuffer, 0, this.inBuffer.Length);
            }
        }

        private void Write(byte value)
        {
            if (this.outPosition == this.outLength)
            {
                this.outStream.Write(this.outBuffer, 0, this.outLength);
                this.outPosition = 0;
            }

            this.outBuffer[this.outPosition++] = value;
        }

        private void Flush()
        {
            if (this.outPosition > 0)
            {
                this.outStream.Write(this.outBuffer, 0, this.outPosition);
                this.outPosition = 0;
            }
        }
    }
}
