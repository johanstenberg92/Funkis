using System.Collections.Generic;
using System.IO;

namespace IronJS.Interpreter
{
    class Parser
    {
        public JSExpression[] Parse(Scanner scanner)
        {
            Lexer lexer = new Lexer(scanner);
            var tokens = lexer.Tokenize();

            var expressions = new List<JSExpression>();

            return null;
        }


    }
}
