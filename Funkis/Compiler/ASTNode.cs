using System;
using System.Linq;
using System.Text;

namespace Funkis.Compiler
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
        public ImportNode[] Imports { get; private set; }
        
        public PropertyNode Namespace { get; private set; }

        public DeclarationNode[] Declarations { get; private set; }

        public ProgramNode(
            ImportNode[] imports,
            DeclarationNode[] declarations
        ) : this(
            imports,
            null, 
            declarations
        ) { }

        public ProgramNode(DeclarationNode[] declarations) : this(
            new ImportNode[] { },
            null,
            declarations
            ) { }

        public ProgramNode(
            ImportNode[] imports, 
            PropertyNode ns, 
            DeclarationNode[] declarations) : base(new Position(0, 0))
        {
            Imports = imports;
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

    public class ImportNode : ASTNode
    {
        public PropertyNode Property { get; private set; }

        public ImportNode(PropertyNode property, Position position) : base(position)
        {
            Property = property;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ImportNode;

            if (other == null) return false;

            return
                Property.Equals(other.Property)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Property.GetHashCode();

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

        public TypeNode Type { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public LetDeclarationNode(
            TypeNode type,
            ExpressionNode expression,
            Position position) : this(null, type, expression, position) { }

        public LetDeclarationNode(
            string name,
            TypeNode type,
            ExpressionNode expression, 
            Position position) : base(position) {
            Name = name;
            Type = type;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LetDeclarationNode;

            if (other == null) return false;

            return
                (Name == null) ? (other.Name == null) : Name.Equals(other.Name)
                && Type.Equals(other.Type)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            if (Name != null) hash *= 31 + Name.GetHashCode();
            hash *= 31 + Type.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public interface FunctionASTNode
    {
        IdentifierNode[] Parameters { get; }

        TypeNode[] ParameterTypes { get; }

        TypeNode ReturnType { get; }

        ExpressionNode Expression { get; }
    }

    public class FuncDeclarationNode : DeclarationNode, FunctionASTNode
    {
        public string Name { get; private set; }

        public TypeNode ReturnType { get; private set; }

        public IdentifierNode[] Parameters { get; private set; }

        public TypeNode[] ParameterTypes { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public FuncDeclarationNode(
            string name,
            TypeNode returnType,
            ExpressionNode expression,
            Position position) : this(
                name,
                returnType,
                new IdentifierNode[] { },
                new TypeNode[] { },
                expression, 
                position
            ) { }

        public FuncDeclarationNode(
            string name,
            TypeNode returnType,
            IdentifierNode[] parameters,
            TypeNode[] parameterTypes,
            ExpressionNode expression,
            Position position) : base(position) {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            ParameterTypes = parameterTypes;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FuncDeclarationNode;

            if (other == null) return false;

            return
                Name.Equals(other.Name)
                && ReturnType.Equals(other.ReturnType)
                && Parameters.SequenceEqual(other.Parameters)
                && ParameterTypes.SequenceEqual(other.ParameterTypes)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Name.GetHashCode();
            hash *= 31 + ReturnType.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + ParameterTypes.GetHashCode();
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

    public class LambdaExpressionNode : ExpressionNode, FunctionASTNode
    {
        public TypeNode ReturnType { get; private set; }

        public IdentifierNode[] Parameters { get; private set; }

        public TypeNode[] ParameterTypes { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public LambdaExpressionNode(
            TypeNode returnType,
            ExpressionNode expression,
            Position position
            ) : this(
                returnType,
                new IdentifierNode[] { },
                new TypeNode[] { },
                expression,
                position
            ) { }

        public LambdaExpressionNode(
            TypeNode returnType,
            IdentifierNode[] parameters,
            TypeNode[] parameterTypes,
            ExpressionNode expression,
            Position position
            ) : base(position)
        {
            ReturnType = returnType;
            Parameters = parameters;
            ParameterTypes = parameterTypes;
            Expression = expression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LambdaExpressionNode;

            if (other == null) return false;

            return
                ReturnType.Equals(other.ReturnType)
                && Parameters.SequenceEqual(other.Parameters)
                && ParameterTypes.SequenceEqual(other.ParameterTypes)
                && Expression.Equals(other.Expression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + ReturnType.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + ReturnType.GetHashCode();
            hash *= 31 + Expression.GetHashCode();

            return hash;
        }
    }

    public class LetInExpressionNode : ExpressionNode
    {
        public string Name { get; private set; }

        public TypeNode Type { get; private set; }

        public ExpressionNode Expression { get; private set; }

        public ExpressionNode InExpression { get; private set; }

        public LetInExpressionNode(
            string name,
            TypeNode type,
            ExpressionNode expression,
            ExpressionNode inExpression,
            Position position
            ) : base(position)
        {
            Name = name;
            Type = type;
            Expression = expression;
            InExpression = inExpression;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LetInExpressionNode;

            if (other == null) return false;

            return
                Name.Equals(other.Name)
                && Type.Equals(other.Type)
                && Expression.Equals(other.Expression)
                && InExpression.Equals(other.InExpression)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Name.GetHashCode();
            hash *= 31 + Type.GetHashCode();
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

        public PatternLiteralNode(PatternLiteral literal) : base((literal as ASTNode).Position)
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

    public class TermExpressionNode : ExpressionNode
    {
        public char Op { get; private set; }

        public TermNode Term { get; private set; }

        public TermExpressionNode(
            TermNode term
        ) : this(
            unchecked ((char) -1),
            term,
            term.Position
        ) { }

        public TermExpressionNode(
            char op,
            TermNode term,
            Position position
            ) : base(position)
        {
            Op = op;
            Term = term;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TermExpressionNode;

            if (other == null) return false;

            return
                Op.Equals(other.Op)
                && Term.Equals(other.Term)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Op.GetHashCode();
            hash *= 31 + Term.GetHashCode();

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

    public class FunctionCallNode : FactorNode
    {
        public PropertyNode Property { get; private set; }

        public ExpressionNode[] Expressions { get; private set; }

        public FunctionCallNode(
            PropertyNode property
        ) : this(
            property,
            new ExpressionNode[] { }
        )
        { }

        public FunctionCallNode(
            PropertyNode property,
            ExpressionNode expression
        ) : this(
            property,
            new ExpressionNode[] { expression }
        ) { }

        public FunctionCallNode(
            PropertyNode property,
            ExpressionNode[] expressions) : base(property.Position)
        {
            Property = property;
            Expressions = expressions;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionCallNode;

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

    public class IntLiteralNode : LiteralNode<int>
    {
        public IntLiteralNode(int value, Position position) : base(value, position) { }
    }

    public class LongLiteralNode : LiteralNode<long>
    {
        public LongLiteralNode(long value, Position position) : base(value, position) { }
    }

    public class FloatLiteralNode : LiteralNode<float>
    {
        public FloatLiteralNode(float value, Position position) : base(value, position) { }
    }

    public class DoubleLiteralNode : LiteralNode<double>
    {
        public DoubleLiteralNode(double value, Position position) : base(value, position) { }
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
        public static readonly object UnitObject = new object();

        public UnitLiteralNode(Position position) : base(UnitObject, position) { }
    }

    public class PropertyNode : FactorNode, PatternLiteral
    {
        public string[] Identifiers { get; private set; }

        public string PropertyAsString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < Identifiers.Length; ++i)
            {
                sb.Append(Identifiers[i]);

                if (i != Identifiers.Length - 1) sb.Append(".");
            }

            return sb.ToString();
        }

        public PropertyNode(
            string identifier,
            Position position
        ) : this(
            new string[] { identifier },
            position
        ) { }

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

    public class IdentifierNode : ASTNode
    {
        public string Identifier { get; private set; }

        public IdentifierNode(string identifier, Position position) : base(position)
        {
            Identifier = identifier;
        }

        public override bool Equals(object obj)
        {
            var other = obj as IdentifierNode;

            if (other == null) return false;

            return 
                Identifier.SequenceEqual(other.Identifier)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifier.GetHashCode();

            return hash;
        }
    }

    public abstract class TypeNode : ASTNode
    {
        public TypeNode(Position position) : base(position) { }
    }

    public class CLRTypeNode : TypeNode
    {
        public Type Type { get; private set; }

        public CLRTypeNode(Type type, Position position) : base(position)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CLRTypeNode;

            if (other == null) return false;

            return
                Type.Equals(other.Type)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Type.GetHashCode();

            return hash;
        }
    }

    public class TypePropertyNode : TypeNode
    {
        public PropertyNode Property { get; private set; }

        public TypePropertyNode(PropertyNode property) : base(property.Position)
        {
            Property = property;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TypePropertyNode;

            if (other == null) return false;

            return
                Property.Equals(other.Property)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Property.GetHashCode();

            return hash;
        }
    }

    public class TypeUnitNode : TypeNode
    {
        public TypeUnitNode(Position position) : base(position) { }

        public override bool Equals(object obj)
        {
            var other = obj as TypeUnitNode;

            if (other == null) return false;

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();

            return hash;
        }
    }

    public class TypeFunctionNode : TypeNode
    {
        public TypeNode[] Parameters { get; private set; }

        public TypeNode ReturnType { get; private set; }

        public TypeFunctionNode(
            TypeNode[] parameters, 
            TypeNode returnType, 
            Position position) : base(position)
        {
            Parameters = parameters;
            ReturnType = returnType;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TypeFunctionNode;

            if (other == null) return false;

            return
                Parameters.SequenceEqual(other.Parameters)
                && ReturnType.Equals(other.ReturnType)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Parameters.GetHashCode();
            hash *= 31 + ReturnType.GetHashCode();

            return hash;
        }
    }

    public class PropertyWithDeclNode : PropertyNode
    {
        public Declaration Declaration { get; private set; }

        public PropertyWithDeclNode(
            string[] identifiers,
            Declaration declaration,
            Position position) : base(identifiers, position)
        {
            Declaration = declaration;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PropertyWithDeclNode;

            if (other == null) return false;

            return 
                Identifiers.SequenceEqual(other.Identifiers)
                && Declaration.Equals(other.Declaration)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Identifiers.GetHashCode();
            hash *= 31 + Declaration.GetHashCode();

            return hash;
        }
    }

    public class FunctionCallWithDeclNode : FunctionCallNode
    {
        public Declaration Declaration { get; private set; }

        public FunctionCallWithDeclNode(
            PropertyNode property,
            ExpressionNode[] expressions,
            Declaration declaration) : base(property, expressions)
        {
            Declaration = declaration;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionCallWithDeclNode;

            if (other == null) return false;

            return
                Property.Equals(other.Property)
                && Expressions.SequenceEqual(other.Expressions)
                && Declaration.Equals(other.Declaration)
                && base.Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= 31 + base.GetHashCode();
            hash *= 31 + Property.GetHashCode();
            hash *= 31 + Expressions.GetHashCode();
            hash *= 31 + Declaration.GetHashCode();

            return hash;
        }
    }
}
