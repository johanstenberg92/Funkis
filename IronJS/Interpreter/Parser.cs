using System;
using System.Collections.Generic;
using System.IO;

namespace IronJS.Interpreter
{
    class Parser
    {
        private Token[] _tokens;

        private Position[] _positions;

        private int _position = 0;

        public Parser(Scanner scanner)
        {
            Lexer lexer = new Lexer(scanner);
            Tuple<Token, Position>[] tokensAndPositions = lexer.Tokenize();

            var tokens = new List<Token>();
            var positions = new List<Position>();

            foreach (Tuple<Token, Position> tuple in tokensAndPositions)
            {
                var token = tuple.Item1;
                var position = tuple.Item2;

                tokens.Add(token);
                positions.Add(position);
            }

            _tokens = tokens.ToArray();
            _positions = positions.ToArray();
        }

        public Expression Parse()
        {
            return program();
        }

        private ProgramExpression program()
        {
            var statements = new List<StatementExpression>();

            var s = statement();

            while (s != null)
            {
                statements.Add(s);
                s = statement();
            }

            return new ProgramExpression(statements.ToArray());
        }

        private StatementExpression statement()
        {
            if (Found(KeywordToken.Var))
            {
            }
            else if (Found(KeywordToken.Function))
            {
            }
            else if (Found(KeywordToken.If))
            {
            }
            else if (Found(KeywordToken.While))
            {
            }

            return null;
        }

        private bool Found(Token token)
        {
            if (token == _tokens[_position])
            {
                _position++;

                return true;
            }

            return false;
        }

        private void Expect(Token token)
        {
            if (token != _tokens[_position])
            {
                var position = _positions[_position];
                throw new InvalidOperationException($"Parsing failed at row {position.Row}, column {position.Column}");
            }

            _position++;
        }
    }
}
