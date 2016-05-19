using System;
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

        public StatementNode[] Statements { get; private set; }

        public IfStatementNode(ExpressionNode expression, StatementNode[] statements, Position position) : base(position)
        {
            Expression = expression;
            Statements = statements;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IfStatementNode;

            if (other == null) return false;

            return Expression == other.Expression && Statements.SequenceEqual(other.Statements) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statements.GetHashCode();

            return hash;
        }
    }

    public class WhileStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; private set; }

        public StatementNode[] Statements { get; private set; }

        public WhileStatementNode(ExpressionNode expression, StatementNode[] statements, Position position) : base(position)
        {
            Expression = expression;
            Statements = statements;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WhileStatementNode;

            if (other == null) return false;

            return Expression == other.Expression && Statements.SequenceEqual(other.Statements) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + Statements.GetHashCode();

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

            return Identifier == other.Identifier 
                && Expression == other.Expression 
                && base.Equals(other as ASTNode);
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

    public class FunctionStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public string[] Parameters { get; private set; }

        public StatementNode[] Statements { get; private set; }

        public ExpressionNode ReturnExpr { get; private set; }

        public FunctionStatementNode(string identifier, string[] parameters, StatementNode[] statements, ExpressionNode returnExpr, Position position) : base(position)
        {
            Identifier = identifier;
            Parameters = parameters;
            Statements = statements;
            ReturnExpr = ReturnExpr;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier
                && Parameters.SequenceEqual(other.Parameters)
                && Statements.SequenceEqual(other.Statements)
                && ReturnExpr == other.ReturnExpr
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + Statements.GetHashCode();
            hash *= 31 + ReturnExpr.GetHashCode();

            return hash;
        }
    }

    public class OperatorEqualStatementNode : StatementNode
    {
        public string Identifier { get; private set; }

        public char Op { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public OperatorEqualStatementNode(string identifier, char op, ExpressionNode expression, Position position) : base(position)
        {
            Identifier = identifier;
            Op = op;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as OperatorEqualStatementNode;

            if (other == null) return false;

            return Identifier == other.Identifier
                && Op == other.Op
                && Expression == other.Expression
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Op.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallNode FunctionCall { get; private set; }

        public FunctionCallStatementNode(FunctionCallNode functionCall) : base(functionCall.Position)
        {
            FunctionCall = functionCall;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionCallStatementNode;

            if (other == null) return false;

            return FunctionCall == other.FunctionCall
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + FunctionCall.GetHashCode();

            return hash;
        }
    }

    public class FunctionCallNode : ASTNode
    {
        public string Identifier { get; private set; }

        public ExpressionNode[] Parameters { get; private set; }

        public FunctionCallNode(string identifier, ExpressionNode[] parameters, Position position) : base(position)
        {
            Identifier = identifier;
            Parameters = parameters;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionCallNode;

            if (other == null) return false;

            return Identifier == other.Identifier
                && Parameters.SequenceEqual(other.Parameters)
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();

            return hash;
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
        public ExpressionNode(Position position) : base(position) { }
    }

    public class TermsExpressionNode : ExpressionNode
    {
        public char OptionalFirstOp { get; private set; }

        public TermNode Term { get; private set; }

        public char[] OptionalOps { get; private set; }

        public TermNode[] OptionalTerms { get; private set; }

        public TermsExpressionNode(char optionalFirstOp, TermNode term, char[] optionalOps, TermNode[] optionalTerms, Position position) : base(position)
        {
            OptionalFirstOp = optionalFirstOp;
            Term = term;
            OptionalOps = optionalOps;
            OptionalTerms = optionalTerms;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TermsExpressionNode;

            if (other == null) return false;

            return OptionalFirstOp == other.OptionalFirstOp
                && Term == other.Term
                && OptionalOps.SequenceEqual(other.OptionalOps)
                && OptionalTerms.SequenceEqual(other.OptionalTerms)
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + OptionalFirstOp.GetHashCode();
            hash *= 31 + Term.GetHashCode();
            hash *= 31 + OptionalOps.GetHashCode();
            hash *= 31 + OptionalTerms.GetHashCode();

            return hash;
        }
    }

    public class BooleanExpressionNode : ExpressionNode
    {
        public ExpressionNode FirstExpr { get; private set; }

        public string Op { get; private set; }

        public ExpressionNode SecondExpr { get; private set; }

        public BooleanExpressionNode(ExpressionNode firstExpr, string op, ExpressionNode secondExpr, Position position) : base(position)
        {
            FirstExpr = firstExpr;
            Op = op;
            SecondExpr = secondExpr;
        }

        public override bool Equals(object obj)
        {
            var other = obj as BooleanExpressionNode;

            if (other == null) return false;

            return FirstExpr == other.FirstExpr
                && Op == other.Op
                && SecondExpr == other.SecondExpr
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + FirstExpr.GetHashCode();
            hash *= 31 + Op.GetHashCode();
            hash *= 31 + SecondExpr.GetHashCode();

            return hash;
        }
    }

    public class FunctionCallExpressionNode : ExpressionNode
    {
        public FunctionCallNode FunctionCall { get; private set; }

        public FunctionCallExpressionNode(FunctionCallNode functionCall) : base(functionCall.Position)
        {
            FunctionCall = functionCall;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionCallExpressionNode;

            if (other == null) return false;

            return FunctionCall == other.FunctionCall
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + FunctionCall.GetHashCode();

            return hash;
        }
    }
    
    public class TermNode : ASTNode
    {
        public FactorNode Factor { get; private set; }

        public char[] OptionalOps { get; private set; }

        public FactorNode[] OptionalFactors { get; private set; }

        public TermNode(FactorNode factor, char[] optionalOps, FactorNode[] optionalFactors, Position position) : base(position)
        {
            Factor = factor;
            OptionalOps = optionalOps;
            OptionalFactors = optionalFactors;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TermNode;

            if (other == null) return false;

            return Factor == other.Factor
                && OptionalOps.SequenceEqual(other.OptionalOps)
                && OptionalFactors.SequenceEqual(other.OptionalFactors)
                && base.Equals(other as ASTNode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Factor.GetHashCode();
            hash *= 31 + OptionalOps.GetHashCode();
            hash *= 31 + OptionalFactors.GetHashCode();

            return hash;
        }
    }

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
