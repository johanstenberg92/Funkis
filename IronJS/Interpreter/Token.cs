using System.Collections.Generic;

namespace IronJS.Interpreter
{
    public abstract class Token
    {

    }
    
    public class SymbolToken : Token
    {
        public static SymbolToken Assign = new SymbolToken();
        public static SymbolToken Parenthesis = new SymbolToken();
        public static SymbolToken ClosingParenthesis = new SymbolToken();
        public static SymbolToken LessThan = new SymbolToken();
        public static SymbolToken LargerThan = new SymbolToken();
        public static SymbolToken Divide = new SymbolToken();
        public static SymbolToken Multiply = new SymbolToken();
        public static SymbolToken Add = new SymbolToken();
        public static SymbolToken Subtract = new SymbolToken();
        public static SymbolToken Bracket = new SymbolToken();
        public static SymbolToken ClosingBracket = new SymbolToken();
        public static SymbolToken Dot = new SymbolToken();
        public static SymbolToken SemiColon = new SymbolToken();

        public static Dictionary<char, SymbolToken> OneCharacterSymbolTokens = new Dictionary<char, SymbolToken>()
        {
            { '=', Assign },
            { '(', Parenthesis },
            { ')', ClosingParenthesis },
            { '<', LessThan },
            { '>', LargerThan },
            { '/', Divide },
            { '*', Multiply },
            { '+', Add },
            { '-', Subtract },
            { '{', Bracket },
            { '}', ClosingBracket },
            { '.', Dot },
            { ';', SemiColon }
        };

        public static SymbolToken Equal = new SymbolToken();
        public static SymbolToken LargerThanOrEqual = new SymbolToken();
        public static SymbolToken LessThanOrEqual = new SymbolToken();
        public static SymbolToken NotEqual = new SymbolToken();
        public static SymbolToken IncrementWithOne = new SymbolToken();
        public static SymbolToken DecrementWithOne = new SymbolToken();
        public static SymbolToken Or = new SymbolToken();
        public static SymbolToken And = new SymbolToken();
        public static SymbolToken IncrementWith = new SymbolToken();
        public static SymbolToken DecrementWIth = new SymbolToken();
        public static SymbolToken DivideWith = new SymbolToken();
        public static SymbolToken MultiplyWith = new SymbolToken();

        public static Dictionary<string, SymbolToken> TwoCharacterSymbolTokens = new Dictionary<string, SymbolToken>()
        {
            { "==", Equal },
            { ">=", LargerThanOrEqual },
            { "<=", LessThanOrEqual },
            { "!=", NotEqual },
            { "++", IncrementWithOne },
            { "--", DecrementWithOne },
            { "||", Or },
            { "&&", And },
            { "+=", IncrementWith },
            { "-=", DecrementWIth },
            { "/=", DivideWith },
            { "*=", MultiplyWith }
        };

        public static SymbolToken EOF = new SymbolToken();
    }

    public class KeywordToken : Token
    {
        public static KeywordToken Function = new KeywordToken();
        public static KeywordToken If = new KeywordToken();
        public static KeywordToken Else = new KeywordToken();
        public static KeywordToken While = new KeywordToken();
        public static KeywordToken Return = new KeywordToken();
        public static KeywordToken Var = new KeywordToken();

        public static Dictionary<string, KeywordToken> KeywordTokens = new Dictionary<string, KeywordToken>()
        {
            { "function", Function },
            { "if", If },
            { "else", Else },
            { "while", While },
            { "return", Return },
            { "var", Var }
        };
    }

    public class IdentifierToken : Token
    {
        public string Value { get; private set; }

        public IdentifierToken(string value)
        {
            Value = value;
        }

        public static bool IsIdentifierStartCharacter(char c)
        {
            return char.IsLetter(c);
        }

        public static bool IsIdentifierCharacter(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        public override bool Equals(object obj)
        {
            var other = obj as IdentifierToken;

            if (other == null) return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class NumericToken : Token
    {
        public int Value { get; private set; }

        public NumericToken(int value)
        {
            Value = value;
        }

        public static bool IsNumericCharacter(char c)
        {
            return char.IsDigit(c);
        }

        public override bool Equals(object obj)
        {
            var other = obj as NumericToken;

            if (other == null) return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class StringToken : Token
    {
        public string Value { get; private set; }

        public StringToken(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as StringToken;

            if (other == null) return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
