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

        public Token[] Tokenize()
        {
            var tokens = new List<Token>();

            Token token = null;

            do
            {
                token = GetToken();
                tokens.Add(token);
            } while (token != SymbolToken.EOF);
            
            return tokens.ToArray();
        }

        private Token GetToken()
        {
            SkipWhitespace();

            char c = _scanner.Peek();

            if (c == Scanner._eof) return SymbolToken.EOF;

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

                var str = chars.ToString();

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

                var str = chars.ToString();

                return new NumericToken(Int32.Parse(str));
            }

            if (c == '"')
            {
                var chars = new List<char>();

                while (c != '"')
                {
                    chars.Add(_scanner.Read());
                    c = _scanner.Peek();
                }

                _scanner.Read();

                return new StringToken(chars.ToString());
            }

            char[] next = { _scanner.Peek(), _scanner.PeekWithOffset(1) };
            
            var possibleTwoCharacterSymbols = new string(next);

            if (SymbolToken.TwoCharacterSymbolTokens.ContainsKey(possibleTwoCharacterSymbols))
            {
                return SymbolToken.TwoCharacterSymbolTokens[possibleTwoCharacterSymbols];
            }

            throw new InvalidOperationException($"Token parsing failed at row {_scanner.Row}, column {_scanner.Column}");
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
