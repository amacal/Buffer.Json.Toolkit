namespace Buffer.Json
{
    public interface IConfigurable
    {
        void SetCommand(ICommand command);

        void SetInput(IParameter path);

        void SetOutput(IParameter path);

        void ShowUsage();
    }
}
