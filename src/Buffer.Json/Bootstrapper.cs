namespace Buffer.Json
{
    public static class Bootstrapper
    {
        public static void Main(string[] args)
        {
            IOptions options = new Options(args);
            IProgram program = new Program();

            if (program.Configure(options))
            {
                program.Execute();
            }
        }
    }
}
