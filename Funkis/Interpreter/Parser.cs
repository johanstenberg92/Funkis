using System;
using System.Collections.Generic;
using System.Linq;

namespace IronJS.Interpreter
{
    public class Parser
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

        public ProgramNode Parse()
        {
            return program();
        }

        private ProgramNode program()
        {
            PropertyNode ns = null;

            if (Found(KeywordToken.Namespace))
            {
                ns = ExpectProperty();
            }

            var declarations = Declarations();

            return new ProgramNode(ns, declarations);
        }

        private DeclarationNode[] Declarations()
        {
            var declarations = new List<DeclarationNode>();

            var declaration = Declaration();

            while (declaration != null)
            {
                declarations.Add(declaration);
                declaration = Declaration();
            }

            return declarations.ToArray();
        }

        private DeclarationNode Declaration()
        {
            var position = Position();

            Expect(KeywordToken.Let);

            if (Found(KeywordToken.Func))
            {
                var name = ExpectIdentifier();

                var parameters = ParameterIdentifiers();

                Expect(SymbolToken.Equal);

                var expression = Expression();

                return new FuncDeclarationNode(name, parameters, expression, position);
            }
            else
            {
                var ident = FoundIdentifier();

                if (ident == null) Expect(SymbolToken.Unit);

                var expression = Expression();

                return new LetDeclarationNode(ident, expression, position);
            }
        }

        private string[] ParameterIdentifiers(int min = 0)
        {
            var parameters = new List<string>();

            var parameter = FoundIdentifier();

            if (parameter != null)
            {
                parameters.Add(parameter);

                while (Found(SymbolToken.Comma))
                {
                    parameter = ExpectIdentifier();
                    parameters.Add(parameter);
                }
            }

            if (parameters.Count < min) ThrowParserError();

            return parameters.ToArray();
        }

        private ExpressionNode Expression()
        {
            var position = Position();

            if (Found(SymbolToken.SquareBracket))
            {
                var expressions = Expressions();

                Expect(SymbolToken.ClosingSquareBracket);

                return new ListExpressionNode(expressions, position);
            }
            else if (Found(SymbolToken.Parenthesis))
            {
                var expressions = Expressions(1);
                
                Expect(SymbolToken.ClosingParenthesis);

                if (expressions.Length > 1)
                {
                    return new ListExpressionNode(expressions, position);
                }
                else
                {
                    return new TermExpressionNode(
                        new TermNode(
                            new ExpressionFactorNode(
                                expressions[0],
                                position
                            )
                        )
                    );
                }
            }
            else if (Found(KeywordToken.Match))
            {
                var matchExpression = Expression();

                var patterns = new List<PatternNode>();

                var patternExpressions = new List<ExpressionNode>();

                while (Found(SymbolToken.Pipe))
                {
                    var pattern = Pattern();

                    patterns.Add(pattern);

                    Expect(SymbolToken.Arrow);

                    var expression = Expression();

                    patternExpressions.Add(expression);
                }

                if (patterns.Count == 0)
                {
                    ThrowParserError();
                }

                return new MatchExpressionNode(
                    matchExpression,
                    patterns.ToArray(),
                    patternExpressions.ToArray(),
                    position
                );
            }
            else if (Found(KeywordToken.If))
            {
                var ifExpression = Expression();

                Expect(KeywordToken.Then);

                var thenExpression = Expression();

                Expect(KeywordToken.Else);

                var elseExpression = Expression();

                return new IfExpressionNode(
                    ifExpression,
                    thenExpression,
                    elseExpression,
                    position
                );
            }
            else if (Found(KeywordToken.Func))
            {
                var identifiers = ParameterIdentifiers();

                Expect(SymbolToken.Arrow);

                var expr = Expression();

                return new LambdaExpressionNode(identifiers, expr, position);
            }
            else if (Found(KeywordToken.Let))
            {
                var identifiers = ParameterIdentifiers();

                Expect(SymbolToken.Equal);

                var bodyExpr = Expression();

                Expect(KeywordToken.In);

                var restExpr = Expression();

                return new LetInExpressionNode(identifiers, bodyExpr, restExpr, position);
            }
            else
            {
                Token[] addOrSubtract = { SymbolToken.Add, SymbolToken.Subtract };

                var idx = FoundOneOf(addOrSubtract);
                char optionalOp = idx != -1 ? (idx == 0 ? '+' : '-') : unchecked((char)-1);

                var term = Term();

                return new TermExpressionNode(optionalOp, term, position);
            }
        }
        
        private PatternNode Pattern()
        {
            return null; // TODO
        }
        
        private static Dictionary<Token, string> infixOps = new Dictionary<Token, string>()
        {
            { SymbolToken.Multiply, "*" },
            { SymbolToken.Divide, "/" },
            { SymbolToken.Add, "+" },
            { SymbolToken.Subtract, "-" },
            { SymbolToken.Equal, "==" },
            { SymbolToken.LargerThanOrEqual, ">=" },
            { SymbolToken.LessThanOrEqual, "<=" },
            { SymbolToken.NotEqual, "!=" },
            { SymbolToken.LessThan, "<" },
            { SymbolToken.LargerThan, ">" },
            { SymbolToken.Or, "||" },
            { SymbolToken.And, "&&" },
            { SymbolToken.Cons, "::" }
        };

        private TermNode Term()
        {
            var position = Position();
            var factor = Factor();

            var infixOpsKeys = infixOps.Keys.ToArray();

            var optionalOps = new List<string>();

            var optionalFactors = new List<FactorNode>();

            var idx = FoundOneOf(infixOpsKeys);

            while (idx != -1)
            {
                var token = infixOpsKeys[idx];
                var op = infixOps[token];

                optionalOps.Add(op);
                optionalFactors.Add(Factor());

                idx = FoundOneOf(infixOpsKeys);
            }

            return new TermNode(factor, optionalOps.ToArray(), optionalFactors.ToArray());
        }

        private FactorNode Factor()
        {
            var position = Position();

            var maybeProperty = FoundProperty();

            if (maybeProperty != null)
            {
                var expressions = Expressions();

                return new PropertyFactorNode(maybeProperty, expressions);
            }
            else if (Found(SymbolToken.Parenthesis))
            {
                var expression = Expression();
                Expect(SymbolToken.ClosingParenthesis);

                return new ExpressionFactorNode(expression, position);
            }

            return Literal();
        }

        private FactorNode Literal()
        {
            var position = Position();

            if (Found(SymbolToken.Unit)) return new UnitLiteralNode(position);

            var maybeInt = FoundInt();

            if (maybeInt.HasValue)
            {
                return new IntLiteralNode(maybeInt.Value, position);
            }

            var maybeFloat = FoundFloat();

            if (maybeFloat.HasValue)
            {
                return new FloatLiteralNode(maybeFloat.Value, position);
            }

            var maybeBool = FoundBool();

            if (maybeBool.HasValue)
            {
                return new BoolLiteralNode(maybeBool.Value, position);
            }

            var maybeChar = FoundChar();

            if (maybeChar.HasValue)
            {
                return new CharLiteralNode(maybeChar.Value, position);
            }

            var maybeString = FoundString();

            if (maybeString != null)
            {
                return new StringLiteralNode(maybeString, position);
            }

            ThrowParserError();
            return null;
        }

        private ExpressionNode[] Expressions(int min = 0)
        {
            var expressions = new List<ExpressionNode>();

            var expression = Expression();

            if (expression != null)
            {
                expressions.Add(expression);

                while (Found(SymbolToken.Comma))
                {
                    expression = Expression();

                    if (expression == null)
                    {
                        ThrowParserError();
                    }

                    expressions.Add(expression);
                }
            }

            if (expressions.Count < min) ThrowParserError();

            return expressions.ToArray();
        }

        private PropertyNode FoundProperty()
        {
            var position = Position();

            var identifier = FoundIdentifier();

            if (identifier != null)
            {
                var identifiers = new List<string>();

                identifiers.Add(identifier);

                while (Found(SymbolToken.Dot))
                {
                    identifier = ExpectIdentifier();
                    identifiers.Add(identifier);
                }

                return new PropertyNode(identifiers.ToArray(), position);
            }

            return null;
        }

        private PropertyNode ExpectProperty()
        {
            var property = FoundProperty();

            if (property == null)
            {
                ThrowParserError();
            }

            return property;
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

        private long? FoundInt()
        {
            var token = _tokens[_position] as IntToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private double? FoundFloat()
        {
            var token = _tokens[_position] as FloatToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private bool? FoundBool()
        {
            var token = _tokens[_position] as BoolToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private char? FoundChar()
        {
            var token = _tokens[_position] as CharToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private string FoundString()
        {
            var token = _tokens[_position] as StringToken;

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
            var maybeIdentifier = FoundIdentifier();
            if (maybeIdentifier == null) ThrowParserError();
            return maybeIdentifier;
        }

        private void ThrowParserError()
        {
            var position = _positions[_position];
            throw new InvalidOperationException($"Parsing failed at row {position.Row}, column {position.Column}");
        }

        private Position Position()
        {
            return _positions[_position];
        }

        private int FoundOneOf(Token[] tokens)
        {
            var t = _tokens[_position];
            for (int i = 0; i < tokens.Length; ++i)
            {
                if (tokens[i] == t)
                {
                    _position++;
                    return i;
                }
            }
            
            return -1;
        }

        private int ExpectOneOf(Token[] tokens)
        {
            var res = FoundOneOf(tokens);

            if (res == -1)
            {
                ThrowParserError();
            }

            return res;
        }
    }
}
