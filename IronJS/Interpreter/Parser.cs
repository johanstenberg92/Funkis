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

        public ASTNode Parse()
        {
            return program();
        }

        private ProgramNode program()
        {
            var statements = new List<StatementNode>();

            var s = Statement();

            while (s != null)
            {
                statements.Add(s);
                s = Statement();
            }

            return new ProgramNode(statements.ToArray());
        }

        private StatementNode Statement()
        {
            if (Found(KeywordToken.Var))
            {
                var identifier = ExpectIdentifier();
                Expect(SymbolToken.Assign);

                var expression = Expression();

                return new VarStatementNode(identifier, expression);
            }
            else if (Found(KeywordToken.Function))
            {
            }

            var isIf = Found(KeywordToken.If);

            if (isIf || Found(KeywordToken.While))
            {
                Expect(SymbolToken.Parenthesis);

                var expression = Expression();

                Expect(SymbolToken.ClosingParenthesis);

                var brackets = Found(SymbolToken.Bracket);

                var statement = Statement();

                if (brackets) Expect(SymbolToken.ClosingBracket);

                if (isIf) return new IfStatementNode(expression, statement);
                else return new WhileStatementNode(expression, statement);
            }

            var maybeIdentifier = FoundIdentifier();
            
            if (maybeIdentifier != null)
            {
                Expect(SymbolToken.Assign);
                
                var expression = Expression();

                return new AssignmentStatementNode(maybeIdentifier, expression);
            }

            return null;
        }

        private ExpressionNode Expression()
        {
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

        private string FoundIdentifier()
        {
            var token = _tokens[_position] as IdentifierToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private void Expect(Token token)
        {
            if (token != _tokens[_position])
            {
                ThrowParserError();
            }

            _position++;
        }

        private string ExpectIdentifier()
        {
            var token = _tokens[_position] as IdentifierToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            ThrowParserError();
            return null;
        }

        private void ThrowParserError()
        {
            var position = _positions[_position];
            throw new InvalidOperationException($"Parsing failed at row {position.Row}, column {position.Column}");
        }
    }
}
