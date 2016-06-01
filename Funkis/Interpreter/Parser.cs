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

        public Parser(string text)
        {
            Lexer lexer = new Lexer(new Scanner(text));
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

            var imports = Imports();

            if (Found(KeywordToken.Namespace))
            {
                ns = ExpectProperty();
            }

            var declarations = Declarations();

            return new ProgramNode(imports, ns, declarations);
        }

        private ImportNode[] Imports()
        {
            var imports = new List<ImportNode>();

            var pos = Position();

            while (Found(KeywordToken.Using))
            {
                var property = ExpectProperty();
                imports.Add(new ImportNode(property, pos));
                pos = Position();
            }

            return imports.ToArray();
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

            if (!Found(KeywordToken.Let)) return null;

            if (Found(KeywordToken.Func))
            {
                var name = ExpectIdentifier().Identifier;

                var parameters = ParameterIdentifiers();

                Expect(SymbolToken.Assign);

                var expression = Expression();

                return new FuncDeclarationNode(name, parameters, expression, position);
            }
            else
            {
                var ident = FoundIdentifier();

                if (ident == null) Expect(SymbolToken.Unit);

                var name = ident == null ? null : ident.Identifier;

                Expect(SymbolToken.Assign);

                var expression = Expression();

                return new LetDeclarationNode(name, expression, position);
            }
        }

        private IdentifierNode[] ParameterIdentifiers(int min = 0)
        {
            var parameters = new List<IdentifierNode>();

            var position = Position();
            var parameter = FoundIdentifier();

            if (parameter != null)
            {
                parameters.Add(new IdentifierNode(parameter.Identifier, position));

                while (Found(SymbolToken.Comma))
                {
                    position = Position();
                    parameter = ExpectIdentifier();
                    parameters.Add(new IdentifierNode(parameter.Identifier, position));
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
                var name = ExpectIdentifier().Identifier;

                var identifiers = ParameterIdentifiers();

                Expect(SymbolToken.Assign);

                var bodyExpr = Expression();

                Expect(KeywordToken.In);

                var restExpr = Expression();

                return new LetInExpressionNode(
                    name, 
                    identifiers, 
                    bodyExpr, 
                    restExpr, 
                    position
                );
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
            var maybeProperty = FoundProperty();

            if (maybeProperty != null) return new PatternLiteralNode(maybeProperty);

            var literal = (Literal() as PatternLiteral);

            if (literal != null) return new PatternLiteralNode(literal);

            ThrowParserError();
            return null;
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
                if (Found(SymbolToken.Parenthesis))
                {
                    var expressions = Expressions();

                    Expect(SymbolToken.ClosingParenthesis);

                    return new FunctionCallNode(maybeProperty, expressions);
                }
                else if (Found(SymbolToken.Unit))
                {
                    return new FunctionCallNode(maybeProperty);
                }
                else
                {
                    return maybeProperty;
                }
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

            var maybeLong = FoundLong();

            if (maybeLong.HasValue)
            {
                return new LongLiteralNode(maybeLong.Value, position);
            }

            var maybeFloat = FoundFloat();

            if (maybeFloat.HasValue)
            {
                return new FloatLiteralNode(maybeFloat.Value, position);
            }

            var maybeDouble = FoundDouble();

            if (maybeDouble.HasValue)
            {
                return new DoubleLiteralNode(maybeDouble.Value, position);
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

            var node = FoundIdentifier();

            if (node != null)
            {
                var identifiers = new List<string>();

                identifiers.Add(node.Identifier);

                while (Found(SymbolToken.Dot))
                {
                    node = ExpectIdentifier();
                    identifiers.Add(node.Identifier);
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

        private IdentifierNode FoundIdentifier()
        {
            var token = _tokens[_position] as IdentifierToken;

            if (token != null)
            {
                var pos = Position();
                _position++;
                return new IdentifierNode(token.Value, pos);
            }

            return null;
        }

        private int? FoundInt()
        {
            var token = _tokens[_position] as IntToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private long? FoundLong()
        {
            var token = _tokens[_position] as LongToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private float? FoundFloat()
        {
            var token = _tokens[_position] as FloatToken;

            if (token != null)
            {
                _position++;
                return token.Value;
            }

            return null;
        }

        private double? FoundDouble()
        {
            var token = _tokens[_position] as DoubleToken;

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

        private IdentifierNode ExpectIdentifier()
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
