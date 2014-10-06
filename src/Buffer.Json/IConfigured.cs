using System.IO;

namespace Buffer.Json
{
    public interface IConfigured
    {
        Stream OpenInput();

        Stream OpenOutput();

        int GetBufferSize();
    }
}
