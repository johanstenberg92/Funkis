using System;
using System.Collections.Generic;
using System.Linq;

namespace IronJS.Interpreter
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

                if (KeywordToken.KeywordTokens.ContainsKey(str))
                {
                    return KeywordToken.KeywordTokens[str];
                }
                else
                {
                    return new IdentifierToken(str);
                }
            }

            if (NumericToken.IsNumericCharacter(c))
            {
                var chars = new List<char>();

                while (NumericToken.IsNumericCharacter(c))
                {
                    chars.Add(_scanner.Read());
                    c = _scanner.Peek();
                }

                var str = new string(chars.ToArray());

                return new NumericToken(Int32.Parse(str));
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

        private bool IsNumber(char c)
        {
            return c >= '0' && c <= '9';
        }

        private Token ReadNumber()
        {
            return null;
        }

        private Token ReadString()
        {
            return null;
        }
    }
}
