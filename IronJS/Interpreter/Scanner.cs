using System.Linq;

namespace IronJS.Interpreter
{

    public class Scanner
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        private int _position = -1;

        public const char _eof = unchecked((char)-1);

        private string _text;

        public Scanner(string text)
        {
            _text = new string(text.Where(c => c != '\r').ToArray());
            Row = 0;
            Column = -1;
        }

        public char Read()
        {
            if (IsCurrentCharacterNewline())
            {
                Row += 1;
                Column = -1;
            }

            _position++;

            char c = ReadNextChar();

            if (c != _eof) Column++;

            return c;
        }

        public char Peek() => PeekWithOffset(1);

        public char PeekWithOffset(int offset) => ReadNextChar(_position + 1);

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
