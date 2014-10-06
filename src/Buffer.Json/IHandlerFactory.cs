namespace Buffer.Json
{
    public interface IHandlerFactory
    {
        IHandler Create(IConfigured configured);
    }
}
