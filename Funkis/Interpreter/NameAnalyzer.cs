using System;
using System.Collections.Generic;

namespace IronJS.Interpreter
{
    public class NameAnalyzer
    {
        private ProgramNode _ast;

        private AnalysisScope _scope;

        public NameAnalyzer(ProgramNode ast, AnalysisScope scope)
        {
            _ast = ast;
            _scope = scope;
        }

        public Tuple<ProgramNode, AnalysisScope> Analyze()
        {
            AddImportsToScope(_ast.Imports);

            // TODO namespace!

            var declarations = new List<DeclarationNode>();

            foreach (DeclarationNode node in _ast.Declarations)
            {
                declarations.Add(AnalyzeDeclarationNode(node));
            }

            var res = new ProgramNode(
                _ast.Imports,
                _ast.Namespace,
                declarations.ToArray()
            );

            return new Tuple<ProgramNode, AnalysisScope>(res, _scope);
        }

        private void AddImportsToScope(ImportNode[] imports)
        {
            // TODO
        }

        private DeclarationNode AnalyzeDeclarationNode(DeclarationNode node)
        {
            if (node is LetDeclarationNode)
                return AnalyzeLetDeclarationNode(node as LetDeclarationNode);
            else if (node is FuncDeclarationNode)
                return AnalyzeFuncDeclarationNode(node as FuncDeclarationNode);
            
            ThrowNameAnalyzerError(node);
            return null;
        }

        private LetDeclarationNode AnalyzeLetDeclarationNode(LetDeclarationNode node)
        {
            if (node.Name != null)
            {
                AddToScope(node.Name, node);
            }

            var expr = AnalyzeExpressionNode(node.Expression);

            var newNode = new LetDeclarationNode(node.Name, expr, node.Position);

            _scope.ReplaceNode(node, newNode);

            return newNode;
        }

        private FuncDeclarationNode AnalyzeFuncDeclarationNode(FuncDeclarationNode node)
        {
            var name = node.Name;

            RequireUnique(node, node.Parameters, name);

            AddToScope(name, node);

            foreach (IdentifierNode id in node.Parameters)
            {
                AddToScope(id.Identifier, id);
            }

            var expr = AnalyzeExpressionNode(node.Expression);

            foreach (IdentifierNode id in node.Parameters)
            {
                _scope.Remove(id.Identifier);
            }

            var newNode = new FuncDeclarationNode(
                name,
                node.Parameters,
                expr,
                node.Position
            );

            _scope.ReplaceNode(node, newNode);

            return newNode;
        }

        private ExpressionNode AnalyzeExpressionNode(ExpressionNode node)
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

            return null;
        }

        private ListExpressionNode AnalyzeListExpressionNode(ListExpressionNode node)
        {
            var exprs = new List<ExpressionNode>();

            foreach (ExpressionNode expr in node.Expressions)
            {
                exprs.Add(AnalyzeExpressionNode(expr));
            }

            return new ListExpressionNode(
                exprs.ToArray(),
                node.Position
            );
        }

        private TupleExpressionNode AnalyzeTupleExpressionNode(TupleExpressionNode node)
        {
            var exprs = new List<ExpressionNode>();

            foreach (ExpressionNode expr in node.Expressions)
            {
                exprs.Add(AnalyzeExpressionNode(expr));
            }

            return new TupleExpressionNode(
                exprs.ToArray(),
                node.Position
            );
        }

        private MatchExpressionNode AnalyzeMatchExpressionNode(MatchExpressionNode node)
        {
            var matchExpr = AnalyzeExpressionNode(node.MatchExpression);

            var patterns = new List<PatternNode>();
            var exprs = new List<ExpressionNode>();

            for (var i = 0; i < node.Patterns.Length; ++i)
            {
                var p = node.Patterns[i];
                var e = node.PatternExpressions[i];

                var analyzedExpr = AnalyzeExpressionNode(e);

                patterns.Add(p);
                exprs.Add(e);
            }

            return new MatchExpressionNode(
                matchExpr,
                patterns.ToArray(),
                exprs.ToArray(),
                node.Position
            );
        }

        private IfExpressionNode AnalyzeIfExpressionNode(IfExpressionNode node)
        {
            var ifExpr = AnalyzeExpressionNode(node.IfExpression);
            var thenExpr = AnalyzeExpressionNode(node.ThenExpression);
            var elseExpr = AnalyzeExpressionNode(node.ElseExpression);

            return new IfExpressionNode(
                ifExpr,
                thenExpr,
                elseExpr,
                node.Position
            );
        }

        private LambdaExpressionNode AnalyzeLambdaExpressionNode(LambdaExpressionNode node)
        {
            RequireUnique(node, node.Parameters);

            foreach (IdentifierNode id in node.Parameters)
            {
                AddToScope(id.Identifier, id);
            }

            var expr = AnalyzeExpressionNode(node.Expression);

            foreach (IdentifierNode id in node.Parameters)
            {
                _scope.Remove(id.Identifier);
            }

            return new LambdaExpressionNode(expr, node.Position);
        }

        private LetInExpressionNode AnalyzeLetInExpressionNode(LetInExpressionNode node)
        {
            var name = node.Name;

            RequireUnique(node, node.Parameters, name);

            foreach (IdentifierNode id in node.Parameters)
            {
                AddToScope(id.Identifier, id);
            }

            var expr = AnalyzeExpressionNode(node.Expression);

            foreach (IdentifierNode id in node.Parameters)
            {
                _scope.Remove(id.Identifier);
            }

            AddToScope(name, node);

            var inExpr = AnalyzeExpressionNode(node.InExpression);

            var newNode = new LetInExpressionNode(
                name,
                node.Parameters,
                expr,
                inExpr,
                node.Position
            );

            _scope.ReplaceNode(node, newNode);

            _scope.Remove(node.Name);

            return newNode;
        }

        private TermExpressionNode AnalyzeTermExpressionNode(TermExpressionNode node)
        {
            var term = AnalyzeTermNode(node.Term);

            return new TermExpressionNode(
                node.Op,
                term,
                node.Position
            );
        }

        private TermNode AnalyzeTermNode(TermNode node)
        {
            var firstFactor = AnalyzeFactorNode(node.Factor);

            var otherFactors = new List<FactorNode>();

            for (var i = 0; i < node.OptionalFactors.Length; ++i)
            {
                var oldFactor = node.OptionalFactors[i];

                otherFactors.Add(AnalyzeFactorNode(oldFactor));
            }

            return new TermNode(
                firstFactor,
                node.OptionalOps,
                otherFactors.ToArray()
            );
        }

        private FactorNode AnalyzeFactorNode(FactorNode node)
        {
            if (node is PropertyNode)
                return AnalyzePropertyNode(node as PropertyNode);
            if (node is FunctionCallNode)
                return AnalyzeFunctionCallNode(node as FunctionCallNode);
            if (node is ExpressionFactorNode)
                return AnalyzeExpressionFactorNode(node as ExpressionFactorNode);
            else
                return node;
        }

        private PropertyWithDeclNode AnalyzePropertyNode(PropertyNode node)
        {
            var asString = node.PropertyAsString();

            var decl = _scope.Get(asString);

            if (decl == null) ThrowNameAnalyzerError(node);

            return new PropertyWithDeclNode(
                node.Identifiers,
                decl,
                node.Position
            );
        }

        private FunctionCallWithDeclNode AnalyzeFunctionCallNode(FunctionCallNode node)
        {
            var asString = node.Property.PropertyAsString();

            var decl = _scope.Get(asString);

            if (decl == null) ThrowNameAnalyzerError(node);

            var parameterExpressions = new List<ExpressionNode>();

            foreach (ExpressionNode param in node.Expressions)
            {
                parameterExpressions.Add(AnalyzeExpressionNode(param));
            }

            return new FunctionCallWithDeclNode(
                node.Property,
                parameterExpressions.ToArray(),
                decl
            );
        }

        private ExpressionFactorNode AnalyzeExpressionFactorNode(ExpressionFactorNode node)
        {
            var expr = AnalyzeExpressionNode(node.Expression);

            return new ExpressionFactorNode(
                expr,
                node.Position
            );
        }

        private void ThrowNameAnalyzerError(ASTNode node)
        {
            var position = node.Position;
            throw new InvalidOperationException(
                $"Name analysis failed at row {position.Row}, column {position.Column}"
            );
        }

        private void AddToScope(string name, ASTNode node)
        {
            if (!_scope.Add(name, node))
                ThrowAlreadyDefinedError(name, node);
        }

        private void ThrowAlreadyDefinedError(string name, ASTNode node)
        {
            var position = node.Position;
            throw new InvalidOperationException(
                $"Name analysis failed at row {position.Row}, column {position.Column}, {name} already defined!"
            );
        }

        private void RequireUnique(ASTNode parent, IdentifierNode[] identifiers, string other = null)
        {
            var size = identifiers.Length;

            var set = new HashSet<string>();

            foreach (IdentifierNode node in identifiers)
            {
                set.Add(node.Identifier);
            }

            if (other != null)
            {
                set.Add(other);
                size++;
            }

            if (set.Count != size) ThrowNameAnalyzerError(parent);
        }
    }
}
