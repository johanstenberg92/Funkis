using System.Linq;

namespace IronJS.Interpreter
{
    public abstract class ASTNode
    {
        public Position Position { get; private set; }

        public ASTNode(Position position)
        {
            Position = position;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ASTNode;

            if (other == null) return false;

            return Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    public class ProgramNode : ASTNode
    {
        public StatementNode[] Statements { get; private set; }

        public ProgramNode(StatementNode[] statements, Position position) : base(position)
        {
            Statements = statements;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProgramNode;

            if (other == null) return false;

            return Statements.SequenceEqual(other.Statements) && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 31 + Statements.GetHashCode();
        }
    }

    public abstract class StatementNode : ASTNode
    {
        public StatementNode(Position position) : base(position) { }
    }

    public class VarStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public VarStatementNode(string identifier, ExpressionNode expression, Position position) : base(position)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VarStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier && Expression == other.Expression && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class IfStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }

        public StatementNode Statement { get; private set; }

        public IfStatementNode(ExpressionNode expression, StatementNode statement, Position position) : base(position)
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
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statement.GetHashCode();

            return hash;
        }
    }

    public class WhileStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }

        public StatementNode Statement { get; private set; }

        public WhileStatementNode(ExpressionNode expression, StatementNode statement, Position position) : base(position)
        {
            Expression = expression;
            Statement = statement;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WhileStatementNode;

            if (other == null) return false;

            return Expression == other.Expression && Statement == other.Statement && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statement.GetHashCode();

            return hash;
        }
    }

    public class AssignmentStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public AssignmentStatementNode(string identifier, ExpressionNode expression, Position position) : base(position)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AssignmentStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier && Expression == other.Expression && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class ExpressionNode : ASTNode
    {
    }

    //public class FunctionStatementNode : StatementNode
    //{
    //    public FunctionStatementNode(string identifier, string[] parameters, StatementNode body, )
    //}

    public abstract class FactorNode : ASTNode
    {
        public FactorNode(Position position) : base(position) { }
    }

    public class IdentifierFactorNode : FactorNode
    {
        public string Value { get; private set; }

        public IdentifierFactorNode(string value, Position position) : base(position)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IdentifierFactorNode;

            if (other == null) return false;

            return Value == other.Value && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Value.GetHashCode();

            return hash;
        }
    }

    public class NumberFactorNode : FactorNode
    {
        public int Value { get; private set; }

        public NumberFactorNode(int value, Position position) : base(position)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as NumberFactorNode;

            if (other == null) return false;

            return Value == other.Value && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Value.GetHashCode();

            return hash;
        }
    }

    public class ExpressionFactorNode : FactorNode
    {
        public ExpressionNode Value { get; private set; }

        public ExpressionFactorNode(ExpressionNode value, Position position) : base(position)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ExpressionFactorNode;

            if (other == null) return false;

            return Value == other.Value && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Value.GetHashCode();

            return hash;
        }
    }
}
