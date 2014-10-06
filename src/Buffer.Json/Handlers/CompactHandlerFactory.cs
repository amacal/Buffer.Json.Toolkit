using Buffer.Json.Engine.Handlers;

namespace Buffer.Json.Handlers
{
    public class CompactHandlerFactory : IHandlerFactory
    {
        public IHandler Create(IConfigured configured)
        {
            return new CompactHandler(configured.OpenOutput(), configured.GetBufferSize());
        }
    }
}
