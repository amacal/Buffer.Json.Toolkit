using System;

namespace Buffer.Json
{
    public class ParserException : Exception
    {
        public ParserException(string message, int index)
            : base(message)
        {
        }
    }
}
