using System;
using System.Collections.Generic;
using System.Linq;

namespace Funkis.Compiler
{
    public class Lexer
    {
        private Scanner _scanner;

        public Lexer(Scanner scanner)
        {
            _scanner = scanner;
        }

        public Tuple<Token, Position>[] Tokenize()
        {
            var tuples = new List<Tuple<Token, Position>>();
            
            Tuple<Token, Position> tuple = null;

            do
            {
                tuple = GetTokenAndPosition();
                tuples.Add(tuple);
            } while (tuple.Item1 != SymbolToken.EOF);
            
            return tuples.ToArray();
        }

        private Tuple<Token, Position> GetTokenAndPosition()
        {
            SkipWhitespace();

            var position = _scanner.PeekPosition();

            var token = GetToken();

            return new Tuple<Token, Position>(token, position);
        }

        private Token GetToken()
        {
            var c = _scanner.Peek();

            if (c == Scanner._eof) return SymbolToken.EOF;

            char[] next = { c, _scanner.PeekWithOffset(2) };

            var possibleTwoCharacterSymbols = new string(next);

            if (SymbolToken.TwoCharacterSymbolTokens.ContainsKey(possibleTwoCharacterSymbols))
            {
                _scanner.Read();
                _scanner.Read();

                return SymbolToken.TwoCharacterSymbolTokens[possibleTwoCharacterSymbols];
            }

            if (SymbolToken.OneCharacterSymbolTokens.ContainsKey(c))
            {
                return SymbolToken.OneCharacterSymbolTokens[_scanner.Read()];
            }

            if (IdentifierToken.IsIdentifierStartCharacter(c))
            {
                var chars = new List<char>();

                while (IdentifierToken.IsIdentifierCharacter(c))
                {
                    chars.Add(_scanner.Read());
                    c = _scanner.Peek();
                }

                var str = new string(chars.ToArray());

                var maybeBoolToken = BoolToken.IsBoolToken(str);

                if (maybeBoolToken != null) return maybeBoolToken;

                if (KeywordToken.KeywordTokens.ContainsKey(str))
                {
                    return KeywordToken.KeywordTokens[str];
                }
                else
                {
                    return new IdentifierToken(str);
                }
            }

            if (char.IsDigit(c))
            {
                var chars = new List<char>();

                while (char.IsDigit(c))
                {
                    chars.Add(_scanner.Read());
                    c = _scanner.Peek();
                }

                if (_scanner.Peek() == '.')
                {
                    chars.Add(_scanner.Read());

                    c = _scanner.Peek();

                    if (!char.IsDigit(c))
                    {
                        var pos = _scanner.PeekPosition();
                        throw new InvalidOperationException($"Expected digit at row {pos.Row}, column {pos.Column}");
                    }

                    while (char.IsDigit(c))
                    {
                        chars.Add(_scanner.Read());
                        c = _scanner.Peek();
                    }

                    var s = new string(chars.ToArray());

                    if (_scanner.Peek() == 'F')
                    {
                        _scanner.Read();
                        return new FloatToken(float.Parse(s));
                    }
                    else
                    {
                        return new DoubleToken(double.Parse(s));
                    }
                }
                else
                {
                    var s = new string(chars.ToArray());

                    if (_scanner.Peek() == 'L')
                    {
                        _scanner.Read();
                        return new LongToken(long.Parse(s));
                    }
                    else
                    {
                        return new IntToken(int.Parse(s));
                    }
                }
            }

            if (c == '\'')
            {
                _scanner.Read();

                c = _scanner.Read();

                if (_scanner.Read() != '\'')
                {
                    var pos = _scanner.PeekPosition();
                    throw new InvalidOperationException($"Expected apostrophe (') at row {pos.Row}, column {pos.Column}");
                }

                return new CharToken(c);
            }

            if (c == '"')
            {
                _scanner.Read();

                var chars = new List<char>();

                c = _scanner.Read();

                while (c != '"')
                {
                    chars.Add(c);
                    c = _scanner.Read();
                }
                
                return new StringToken(new string(chars.ToArray()));
            }

            var position = _scanner.PeekPosition();

            throw new InvalidOperationException($"Lexing failed at row {position.Row}, column {position.Column}");
        }

        private static char[] WhiteSpaceChars = { ' ', '\t', '\n' };

        private void SkipWhitespace()
        {
            char c = _scanner.Peek();

            while (WhiteSpaceChars.Contains(c))
            {
                c = _scanner.Read();
                c = _scanner.Peek();
            }
        }
    }
}
