namespace Buffer.Json
{
    public interface IStop
    {
        bool IsSet { get; }

        bool HasDescription();

        string GetDescription();
    }
}
