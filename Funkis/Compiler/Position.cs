namespace Funkis.Compiler
{
    public class Position
    {
        public int Column { get; private set; }

        public int Row { get; private set; }

        public Position(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;

            if (other == null) return false;

            return Column == other.Column && Row == other.Row;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Column;
            hash *= 31 + Row;

            return hash;
        }
    }
}
