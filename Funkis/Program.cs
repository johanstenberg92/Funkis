using Funkis.Compiler;
using System;
using System.IO;

namespace Funkis
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 0 || (args.Length == 1 && !args[0].EndsWith(".funk")))
            {
                Console.WriteLine("You need to provide one and only one funkis file (*.funk) to be interpreted.");
                Console.WriteLine("This file can later reference other files if needed.");
                return -1;
            }

            var path = args[0];
            string text = null;

            try
            {
                text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't open file {path}, exception: {ex.ToString()}");
                return -1;
            }

            Parser parser = new Parser(text);
            var ast = parser.Parse();

            return 0;
        }
    }
}