using Buffer.Json.Handlers;
using System;
using System.IO;

namespace Buffer.Json
{
    public class Program : IProgram, IConfigurable, IConfigured
    {
        private Stream input;
        private Stream output;
        private int bufferSize = 16 * 1024;

        private IHandlerFactory factory;

        public bool Configure(IOptions options)
        {
            return options.Apply(this);
        }

        public void SetCommand(ICommand command)
        {
            switch (command.Verb)
            {
                case "beautify":
                    this.factory = new BeautifyHandlerFactory();
                    break;

                case "compact":
                    this.factory = new CompactHandlerFactory();
                    break;

                default:
                    command.Stop();
                    break;
            }
        }

        public void SetInput(IParameter path)
        {
            try
            {
                this.input = File.OpenRead(path.Value);
            }
            catch (Exception ex)
            {
                path.StopWith(ex.Message);
            }
        }

        public void SetOutput(IParameter path)
        {
            try
            {
                this.output = File.Create(path.Value);
            }
            catch (Exception ex)
            {
                path.StopWith(ex.Message);
            }
        }

        public void Execute()
        {
            IHandler handler = this.factory.Create(this);
            IParser parser = new Parser(this.GetBufferSize());

            parser.Parse(this.OpenInput(), handler);
        }

        public Stream OpenInput()
        {
            return this.input ?? Console.OpenStandardInput();
        }

        public Stream OpenOutput()
        {
            return this.output ?? Console.OpenStandardOutput();
        }

        public int GetBufferSize()
        {
            return this.bufferSize;
        }

        public void ShowUsage()
        {
            Console.WriteLine("Buffer.Json 0.1.0.0");
            Console.WriteLine("Copyright 2014 Adrian Macal");
            Console.WriteLine();

            Console.WriteLine("Usage: Buffer.Json <command> [options]");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine("    beautify - beautifies by adding whitecharacters");
            Console.WriteLine("    compact  - removes all superfluous whitecharacter");
            Console.WriteLine();
            Console.WriteLine("Available options:");
            Console.WriteLine("    --input  - uses file instead of standard input");
            Console.WriteLine("    --output - uses file instead of standard output");
        }
    }
}
