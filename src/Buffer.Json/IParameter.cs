namespace Buffer.Json
{
    public interface IParameter
    {
        string Value { get; }

        void Stop();

        void StopWith(string message);
    }
}
