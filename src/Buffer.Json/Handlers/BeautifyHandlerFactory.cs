using Buffer.Json.Engine.Handlers;

namespace Buffer.Json.Handlers
{
    public class BeautifyHandlerFactory : IHandlerFactory
    {
        public IHandler Create(IConfigured configured)
        {
            return new BeautifyHandler(configured.OpenOutput(), configured.GetBufferSize());
        }
    }
}
