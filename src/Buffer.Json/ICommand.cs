namespace Buffer.Json
{
    public interface ICommand
    {
        string Verb { get; }

        void Stop();
    }
}
