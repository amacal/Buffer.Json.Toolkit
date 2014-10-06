namespace Buffer.Json
{
    public interface IProgram
    {
        bool Configure(IOptions options);

        void Execute();
    }
}
