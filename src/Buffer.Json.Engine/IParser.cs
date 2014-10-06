using System.IO;

namespace Buffer.Json
{
    public interface IParser
    {
        void Parse(Stream input, IHandler handler);
    }
}
