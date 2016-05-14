using System.Linq;

namespace IronJS.Interpreter
{

    public class Scanner
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Position { get; private set; }

        public const char _eof = unchecked((char)-1);

        private string _text;

        public Scanner(string text)
        {
            _text = text.Where(c => c != '\r').ToString();
            Row = -1;
            Column = 0;
            Position = -1;
        }

        public char Read()
        {
            Position++;
            Column++;

            char c = ReadNextChar();

            if (c == '\n')
            {
                Row += 1;
                Column = 0;
            }

            return c;
        }

        public char Peek() => PeekWithOffset(1);

        public char PeekWithOffset(int offset) => ReadNextChar(Position + offset + 1);

        private char ReadNextChar() => ReadNextChar(Position);

        private char ReadNextChar(int position)
        {
            if (_text.Length == position) return _eof;

            return _text[Position];
        }
    }
}
