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
        public static SymbolToken Comma = new SymbolToken();
        public static SymbolToken Pipe = new SymbolToken();
        public static SymbolToken Newline = new SymbolToken();

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
            { ';', SemiColon },
            { ',', Comma },
            { '|', Pipe },
            { '\n', Newline }
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
        public static SymbolToken DecrementWith = new SymbolToken();
        public static SymbolToken DivideWith = new SymbolToken();
        public static SymbolToken MultiplyWith = new SymbolToken();
        public static SymbolToken Cons = new SymbolToken();
        public static SymbolToken Arrow = new SymbolToken();
        public static SymbolToken Unit = new SymbolToken();

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
            { "-=", DecrementWith },
            { "/=", DivideWith },
            { "*=", MultiplyWith },
            { "::", Cons },
            { "->", Arrow },
            { "()", Unit }
        };

        public static SymbolToken EOF = new SymbolToken();
    }

    public class KeywordToken : Token
    {
        public static KeywordToken Let = new KeywordToken();
        public static KeywordToken Func = new KeywordToken();
        public static KeywordToken If = new KeywordToken();
        public static KeywordToken Then = new KeywordToken();
        public static KeywordToken Else = new KeywordToken();
        public static KeywordToken Match = new KeywordToken();
        public static KeywordToken Var = new KeywordToken();

        public static Dictionary<string, KeywordToken> KeywordTokens = new Dictionary<string, KeywordToken>()
        {
            { "let", Let },
            { "func", Func },
            { "if", If },
            { "then", Then },
            { "else", Else },
            { "match", Match }
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

    public class LiteralToken<T> : Token
    {
        public T Value { get; private set; }

        public LiteralToken(T value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LiteralToken<T>;

            if (other == null) return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class IntToken : LiteralToken<long>
    {
        public IntToken(long value) : base(value) { }
    }

    public class FloatToken : LiteralToken<double>
    {
        public FloatToken(double value) : base(value) { }
    }

    public class CharToken : LiteralToken<char>
    {
        public CharToken(char value) : base(value) { }
    }

    public class BoolToken : LiteralToken<bool>
    {
        public BoolToken(bool value) : base(value) { }

        public static BoolToken TrueToken = new BoolToken(true);

        public static BoolToken FalseToken = new BoolToken(false);

        public static BoolToken IsBoolToken(string s)
        {
            switch (s)
            {
                case "true":
                    return TrueToken;
                case "false":
                    return FalseToken;
                default:
                    return null;
            }
        }
    }

    public class StringToken : LiteralToken<string>
    {
        public StringToken(string value) : base(value) { }
    }
}
