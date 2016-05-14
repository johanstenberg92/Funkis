using IronJS.Interpreter;
using System;
using System.IO;

namespace IronJS
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 0 || !args[0].EndsWith(".js"))
            {
                Console.WriteLine("You need to provide one and only one js file (*.js) to be interpreted.");
                Console.WriteLine("This file can later reference other files if needed.");
                return -1;
            }

            var path = args[0];


            try
            {
                var text = File.ReadAllText(path);
                var scanner = new Scanner(text);

                Parser parser = new Parser();
                var expressions = parser.Parse(scanner);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't open file {path}, exception: {ex.ToString()}");
                return -1;
            }

            return 0;
        }
    }
}