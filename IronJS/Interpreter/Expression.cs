using System.Linq;

namespace IronJS.Interpreter
{
    public abstract class Expression
    {
    }

    public class ProgramExpression : Expression
    {
        public StatementExpression[] Statements { get; private set; }

        public ProgramExpression(StatementExpression[] statements)
        {
            Statements = statements;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProgramExpression;

            if (other == null) return false;

            return Statements.SequenceEqual(other.Statements);
        }

        public override int GetHashCode()
        {
            return Statements.GetHashCode();
        }
    }

    public class StatementExpression : Expression
    {

    }


    public  class IdExpression : Expression
    {
    }
}
