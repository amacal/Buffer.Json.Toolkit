using System;

namespace Buffer.Json.Toolkit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Processor(Console.OpenStandardInput(), Console.OpenStandardOutput(), 16 * 1024).Beautify();
        }
    }
}
