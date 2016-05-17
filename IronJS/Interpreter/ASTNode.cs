using System.Linq;

namespace IronJS.Interpreter
{
    public abstract class ASTNode
    {
    }

    public class ProgramNode : ASTNode
    {
        public StatementNode[] Statements { get; private set; }

        public ProgramNode(StatementNode[] statements)
        {
            Statements = statements;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProgramNode;

            if (other == null) return false;

            return Statements.SequenceEqual(other.Statements);
        }

        public override int GetHashCode()
        {
            return Statements.GetHashCode();
        }
    }

    public abstract class StatementNode : ASTNode
    {

    }

    public class VarStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public VarStatementNode(string identifier, ExpressionNode expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VarStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier && Expression == other.Expression;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class IfStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }

        public StatementNode Statement { get; private set; }

        public IfStatementNode(ExpressionNode expression, StatementNode statement)
        {
            Expression = expression;
            Statement = statement;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IfStatementNode;

            if (other == null) return false;

            return Expression == other.Expression && Statement == other.Statement;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statement.GetHashCode();

            return hash;
        }
    }

    public class WhileStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }

        public StatementNode Statement { get; private set; }

        public WhileStatementNode(ExpressionNode expression, StatementNode statement)
        {
            Expression = expression;
            Statement = statement;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WhileStatementNode;

            if (other == null) return false;

            return Expression == other.Expression && Statement == other.Statement;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statement.GetHashCode();

            return hash;
        }
    }

    public class AssignmentStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public AssignmentStatementNode(string identifier, ExpressionNode expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AssignmentStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier && Expression == other.Expression;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class ExpressionNode : ASTNode
    {
    }
}
