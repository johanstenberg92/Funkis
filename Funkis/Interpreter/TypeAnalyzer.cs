using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronJS.Interpreter
{
    public class TypeAnalyzer
    {
        private IDictionary<ASTNode, Type> _types;

        private ProgramNode _ast;

        private AnalysisScope _scope;

        public TypeAnalyzer(ProgramNode program, AnalysisScope scope)
        {
            _ast = program;
            _scope = scope;
        }

        public IDictionary<ASTNode, Type> Analyze()
        {
            _types = new Dictionary<ASTNode, Type>();

            foreach (DeclarationNode node in _ast.Declarations)
            {
                AnalyzeDeclarationNode(node);
            }

            return _types;
        }

        private void AnalyzeDeclarationNode(DeclarationNode node)
        {
            if (node is LetDeclarationNode) AnalyzeLetDeclarationNode(node as LetDeclarationNode);
        }

        private void AnalyzeLetDeclarationNode(LetDeclarationNode node)
        {
            var type = AnalyzeExpressionNode(node.Expression);
            _types.Add(node, type);
        }

        private Type AnalyzeExpressionNode(ExpressionNode node)
        {
            if (node is ListExpressionNode)
                return AnalyzeListExpressionNode(node as ListExpressionNode);
            if (node is TupleExpressionNode)
                return AnalyzeTupleExpressionNode(node as TupleExpressionNode);
            if (node is MatchExpressionNode)
                return AnalyzeMatchExpressionNode(node as MatchExpressionNode);
            if (node is IfExpressionNode)
                return AnalyzeIfExpressionNode(node as IfExpressionNode);
            if (node is LambdaExpressionNode)
                return AnalyzeLambdaExpressionNode(node as LambdaExpressionNode);
            if (node is LetInExpressionNode)
                return AnalyzeLetInExpressionNode(node as LetInExpressionNode);
            if (node is TermExpressionNode)
                return AnalyzeTermExpressionNode(node as TermExpressionNode);

            ThrowNameAnalyzerError(node);
            return null;
        }

        private Type AnalyzeListExpressionNode(ListExpressionNode node)
        {
            if (node.Expressions.Length == 0) return typeof(object);

            var types = new List<Type>();

            foreach (ExpressionNode expr in node.Expressions)
            {
                types.Add(AnalyzeExpressionNode(expr));
            }

            AssertAllTypesAreSame(node, types.ToArray());

            var typeArgs = new Type[]{ types.First() };
            var immutableListType = typeof(ImmutableList<>);

            var dummy = immutableListType.MakeGenericType(typeArgs);

            var res = dummy.GetType();

            _types.Add(node, res);

            return res;
        }

        private static readonly int TupleMaxLength = 7;

        private static readonly Type[] TupleTypes = {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
        };

        private Type AnalyzeTupleExpressionNode(TupleExpressionNode node)
        {
            var types = new List<Type>();

            foreach (ExpressionNode expr in node.Expressions)
            {
                types.Add(AnalyzeExpressionNode(expr));
            }

            var numberOfTypes = types.Count;

            if (numberOfTypes > TupleMaxLength) ThrowNameAnalyzerError(node);
            
            var typeArgs = types.ToArray();

            var tupleType = TupleTypes[numberOfTypes - 1];

            var dummy = tupleType.MakeGenericType(typeArgs);

            var res = dummy.GetType();

            _types.Add(node, res);

            return res;
        }

        private Type AnalyzeMatchExpressionNode(MatchExpressionNode node)
        {
            var matchExprType = AnalyzeExpressionNode(node.MatchExpression);

            if (matchExprType != typeof(bool)) ThrowNameAnalyzerError(node);

            var types = new List<Type>();

            foreach (ExpressionNode expr in node.PatternExpressions)
            {
                types.Add(AnalyzeExpressionNode(expr));
            }

            AssertAllTypesAreSame(node, types.ToArray());

            var res = types.First();

            _types.Add(node, res);

            return res;
        }

        private Type AnalyzeIfExpressionNode(IfExpressionNode node)
        {
            var ifExprType = AnalyzeExpressionNode(node.IfExpression);

            if (ifExprType != typeof(bool)) ThrowNameAnalyzerError(node);

            var types = new List<Type>();

            var thenType = AnalyzeExpressionNode(node.ThenExpression);
            types.Add(thenType);

            var elseType = AnalyzeExpressionNode(node.ElseExpression);
            types.Add(elseType);

            AssertAllTypesAreSame(node, thenType, elseType);

            var res = types.First();

            _types.Add(node, res);

            return res;
        }

        private Type AnalyzeLambdaExpressionNode(LambdaExpressionNode node)
        {
            return null;
        }

        private Type AnalyzeLetInExpressionNode(LetInExpressionNode node)
        {
            return null;
        }

        private Type AnalyzeTermExpressionNode(TermExpressionNode node)
        {
            return null;
        }

        private Type AnalyzeTermNode(TermNode node)
        {
            return null;
        }

        private Type AnalyzeFactorNode(FactorNode node)
        {
            if (node is PropertyNode)
                return AnalyzePropertyNode(node as PropertyNode);
            if (node is FunctionCallNode)
                return AnalyzeFunctionCallNode(node as FunctionCallNode);
            if (node is ExpressionFactorNode)
                return AnalyzeExpressionFactorNode(node as ExpressionFactorNode);
            else
                return AnalyzeLiteralNode(node);
        }

        private Type AnalyzePropertyNode(PropertyNode node)
        {
            return null;
        }

        private Type AnalyzeFunctionCallNode(FunctionCallNode node)
        {
            return null;
        }

        private Type AnalyzeExpressionFactorNode(ExpressionFactorNode node)
        {
            return AnalyzeExpressionNode(node.Expression);
        }

        private Type AnalyzeLiteralNode(FactorNode node)
        {
            if (node is IntLiteralNode) return typeof(int);
            if (node is LongLiteralNode) return typeof(long);
            if (node is FloatLiteralNode) return typeof(float);
            if (node is LongLiteralNode) return typeof(long);
            if (node is BoolLiteralNode) return typeof(bool);
            if (node is CharLiteralNode) return typeof(char);
            if (node is StringLiteralNode) return typeof(string);
            if (node is UnitLiteralNode) return null; // TODO

            ThrowNameAnalyzerError(node);
            return null;
        }

        private void ThrowNameAnalyzerError(ASTNode node)
        {
            var position = node.Position;
            throw new InvalidOperationException(
                $"Type analysis failed at row {position.Row}, column {position.Column}"
            );
        }

        private void AssertAllTypesAreSame(ASTNode parent, params Type[] types)
        {
            if (types.Length > 0)
            {
                var set = new HashSet<Type>(types);

                if (set.Count != 1)
                {
                    ThrowNameAnalyzerError(parent);
                }
            }
        }
    }
}
