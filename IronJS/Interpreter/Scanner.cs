using System.Linq;

namespace IronJS.Interpreter
{

    public class Scanner
    {
        private int _column = -1;
        private int _row = -1;
        private int _position = -1;

        public const char _eof = unchecked((char)-1);

        private string _text;

        public Scanner(string text)
        {
            _text = new string(text.Where(c => c != '\r').ToArray());
            _row = 0;
            _column = -1;
        }

        public char Read()
        {
            if (IsCurrentCharacterNewline())
            {
                _row += 1;
                _column = -1;
            }

            _position++;

            char c = ReadNextChar();

            if (c != _eof) _column++;

            return c;
        }

        public Position PeekPosition()
        {
            if (IsCurrentCharacterNewline())
            {
                return new Position(0, _row + 1);
            }
            else if (Peek() == _eof)
            {
                return new Position(-1, -1);
            }
            else
            {
                return new Position(_column + 1, _row);
            }
        }

        public char Peek() => PeekWithOffset(1);

        public char PeekWithOffset(int offset) => ReadNextChar(_position + offset);

        private char ReadNextChar() => ReadNextChar(_position);

        private char ReadNextChar(int position)
        {
            if (_text.Length <= position) return _eof;

            return _text[position];
        }

        private bool IsCurrentCharacterNewline()
        {
            return 
                _position != -1 && 
                _position < _text.Length && 
                _text[_position] == '\n';
        }
    }
}
