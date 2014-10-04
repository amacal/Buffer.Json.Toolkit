using System;

namespace Buffer.Json.Toolkit
{
    public class ProcessorException : Exception
    {
        public ProcessorException(string message, int index)
            : base(message)
        {
        }
    }
}
