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

            return Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + Position.GetHashCode();

            return hash;
        }
    }

    public class ProgramNode : ASTNode
    {
        public PropertyNode Namespace { get; private set; }

        public DeclarationNode[] Declarations { get; private set; }

        public ProgramNode(DeclarationNode[] declarations) : this(null, declarations) { }

        public ProgramNode(PropertyNode ns, DeclarationNode[] declarations) : base(new Position(0, 0))
        {
            Namespace = ns;
            Declarations = declarations;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ProgramNode;

            if (other == null) return false;

            return
                Namespace == null ? (other.Namespace == null) : Namespace.Equals(other.Namespace)
                && Declarations.SequenceEqual(other.Declarations) 
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            if (Namespace != null) hash *= 31 + Namespace.GetHashCode();
            hash *= 31 + Declarations.GetHashCode();

            return hash;
        }
    }

    public abstract class DeclarationNode : ASTNode
    {
        public DeclarationNode(Position position) : base(position) { }
    }

    public class LetDeclarationNode : DeclarationNode
    {
        public string Name { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public LetDeclarationNode(
            ExpressionNode expression,
            Position position) : this(null, expression, position) { }

        public LetDeclarationNode(
            string name,
            ExpressionNode expression, 
            Position position) : base(position) {
            Name = name;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LetDeclarationNode;

            if (other == null) return false;

            return
                (Name == null) ? (other.Name == null) : Name.Equals(other.Name)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            if (Name != null) hash *= 31 + Name.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class FuncDeclarationNode : DeclarationNode
    {
        public string Name { get; private set; }

        public string[] Parameters { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public FuncDeclarationNode(
            string name,
            ExpressionNode expression,
            Position position) : this(
                name,
                new string[] { },
                expression, 
                position
            ) { }

        public FuncDeclarationNode(
            string name,
            string[] parameters,
            ExpressionNode expression,
            Position position) : base(position) {
            Name = name;
            Parameters = parameters;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FuncDeclarationNode;

            if (other == null) return false;

            return
                Name.Equals(other.Name)
                && Parameters.SequenceEqual(other.Parameters)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Name.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
        public ExpressionNode(Position position) : base(position) { }
    }

    public class ListExpressionNode : ExpressionNode
    {
        public ExpressionNode[] Expressions { get; private set; }

        public ListExpressionNode(ExpressionNode[] expressions, Position position) : base(position)
        {
            Expressions = expressions;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ListExpressionNode;

            if (other == null) return false;

            return
                Expressions.SequenceEqual(other.Expressions)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expressions.GetHashCode();

            return hash;
        }
    }

    public class TupleExpressionNode : ExpressionNode
    {
        public ExpressionNode[] Expressions { get; private set; }

        public TupleExpressionNode(ExpressionNode[] expressions, Position position) : base(position)
        {
            Expressions = expressions;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TupleExpressionNode;

            if (other == null) return false;

            return
                Expressions.SequenceEqual(other.Expressions)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expressions.GetHashCode();

            return hash;
        }
    }

    public class MatchExpressionNode : ExpressionNode
    {
        public ExpressionNode MatchExpression { get; private set; }

        public PatternNode[] Patterns { get; private set; }

        public ExpressionNode[] PatternExpressions { get; private set; }

        public MatchExpressionNode(
            ExpressionNode matchExpression,
            PatternNode[] patterns,
            ExpressionNode[] patternExpressions,
            Position position
            ) : base(position)
        {
            MatchExpression = matchExpression;
            Patterns = patterns;
            PatternExpressions = patternExpressions;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MatchExpressionNode;

            if (other == null) return false;

            return
                MatchExpression.Equals(other.MatchExpression)
                && Patterns.SequenceEqual(other.Patterns)
                && PatternExpressions.SequenceEqual(other.PatternExpressions)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + MatchExpression.GetHashCode();
            hash *= 31 + Patterns.GetHashCode();
            hash *= 31 + PatternExpressions.GetHashCode();

            return hash;
        }
    }

    public class IfExpressionNode : ExpressionNode
    {
        public ExpressionNode IfExpression { get; private set; }

        public ExpressionNode ThenExpression { get; private set; }

        public ExpressionNode ElseExpression { get; private set; }

        public IfExpressionNode(
            ExpressionNode ifExpression,
            ExpressionNode thenExpression,
            ExpressionNode elseExpression,
            Position position
            ) : base(position)
        {
            IfExpression = ifExpression;
            ThenExpression = thenExpression;
            ElseExpression = elseExpression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IfExpressionNode;

            if (other == null) return false;

            return
                IfExpression.Equals(other.IfExpression)
                && ThenExpression.Equals(other.ThenExpression)
                && ElseExpression.Equals(other.ElseExpression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + IfExpression.GetHashCode();
            hash *= 31 + ThenExpression.GetHashCode();
            hash *= 31 + ElseExpression.GetHashCode();

            return hash;
        }
    }

    public class LambdaExpressionNode : ExpressionNode
    {
        public string[] Parameters { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public LambdaExpressionNode(
            ExpressionNode expression,
            Position position
            ) : this(
                new string[] { },
                expression,
                position
            ) { }

        public LambdaExpressionNode(
            string[] parameters,
            ExpressionNode expression,
            Position position
            ) : base(position)
        {
            Parameters = parameters;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LambdaExpressionNode;

            if (other == null) return false;

            return
                Parameters.SequenceEqual(other.Parameters)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class LetInExpressionNode : ExpressionNode
    {
        public string[] Identifiers { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public ExpressionNode InExpression { get; private set; }

        public LetInExpressionNode(
            string[] identifiers,
            ExpressionNode expression,
            ExpressionNode inExpression,
            Position position
            ) : base(position)
        {
            Identifiers = identifiers;
            Expression = expression;
            InExpression = inExpression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LetInExpressionNode;

            if (other == null) return false;

            return
                Identifiers.SequenceEqual(other.Identifiers)
                && Expression.Equals(other.Expression)
                && InExpression.Equals(other.InExpression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifiers.GetHashCode();
            hash *= 31 + Expression.GetHashCode();
            hash *= 31 + InExpression.GetHashCode();

            return hash;
        }
    }

    public abstract class PatternNode : ASTNode
    {
        public PatternNode(Position position) : base(position) { }
    }

    public class PatternLiteralNode : PatternNode
    {
        public PatternLiteral Literal { get; private set; }

        PatternLiteralNode(PatternLiteral literal) : base((literal as ASTNode).Position)
        {
            Literal = literal;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PatternLiteralNode;

            if (other == null) return false;

            return Literal.Equals(other.Literal)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Literal.GetHashCode();

            return hash;
        }
    }

    public class TermNode : ASTNode
    {
        public FactorNode Factor { get; private set; }

        public string[] OptionalOps { get; private set; }

        public FactorNode[] OptionalFactors { get; private set; }

        public TermNode(
            FactorNode factor
        ) : this(
            factor,
            new string[0],
            new FactorNode[0]
        )
        { }

        public TermNode(FactorNode factor, string[] optionalOps, FactorNode[] optionalFactors) : base(factor.Position)
        {
            Factor = factor;
            OptionalOps = optionalOps;
            OptionalFactors = optionalFactors;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TermNode;

            if (other == null) return false;

            return Factor.Equals(other.Factor)
                && OptionalOps.SequenceEqual(other.OptionalOps)
                && OptionalFactors.SequenceEqual(other.OptionalFactors)
                && base.Equals(other);
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

    public class PropertyFactorNode : FactorNode
    {
        public PropertyNode Property { get; private set; }

        public ExpressionNode[] Expressions { get; private set; }

        public PropertyFactorNode(
            PropertyNode property
        ) : this(
            property,
            new ExpressionNode[] { }
        ) { }

        public PropertyFactorNode(
            PropertyNode property, 
            ExpressionNode[] expressions) : base(property.Position)
        {
            Property = property;
            Expressions = expressions;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PropertyFactorNode;

            if (other == null) return false;

            return 
                Property.Equals(other.Property)
                && Expressions.SequenceEqual(other.Expressions)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Property.GetHashCode();
            hash *= 31 + Expressions.GetHashCode();

            return hash;
        }
    }

    public class ExpressionFactorNode : FactorNode
    {
        public ExpressionNode Expression { get; private set; }

        public ExpressionFactorNode(
            ExpressionNode expression,
            Position position) : base(position)
        {
            Expression = expression;
        }
     
        public override bool Equals(object obj)
        {
            var other = obj as ExpressionFactorNode;

            if (other == null) return false;

            return
                Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public interface PatternLiteral { }

    public abstract class LiteralNode<T> : FactorNode, PatternLiteral
    {
        public T Value { get; private set; }

        public LiteralNode(T value, Position position) : base(position)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LiteralNode<T>;

            if (other == null) return false;

            return
                Value.Equals(other.Value)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Value.GetHashCode();

            return hash;
        }
    }

    public class IntLiteralNode : LiteralNode<long>
    {
        public IntLiteralNode(long value, Position position) : base(value, position) { }
    }

    public class FloatLiteralNode : LiteralNode<double>
    {
        public FloatLiteralNode(double value, Position position) : base(value, position) { }
    }

    public class BoolLiteralNode : LiteralNode<bool>
    {
        public BoolLiteralNode(bool value, Position position) : base(value, position) { }
    }

    public class CharLiteralNode : LiteralNode<char>
    {
        public CharLiteralNode(char value, Position position) : base(value, position) { }
    }

    public class StringLiteralNode : LiteralNode<string>
    {
        public StringLiteralNode(string value, Position position) : base(value, position) { }
    }

    public class UnitLiteralNode : LiteralNode<object>
    {
        private static object UnitObject = new object();

        public UnitLiteralNode(Position position) : base(UnitObject, position) { }
    }

    public class PropertyNode : ASTNode, PatternLiteral
    {
        public string[] Identifiers { get; private set; }

        public PropertyNode(string[] identifiers, Position position) : base(position)
        {
            Identifiers = identifiers;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PropertyNode;

            if (other == null) return false;

            return Identifiers.SequenceEqual(other.Identifiers)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifiers.GetHashCode();

            return hash;
        }
    }

}
