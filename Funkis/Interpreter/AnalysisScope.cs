using System.Collections.Generic;
using System.Linq;

namespace IronJS.Interpreter
{
    public class AnalysisScope
    {
        private Dictionary<string, Declaration> _declarations =
            new Dictionary<string, Declaration>();

        private Dictionary<ASTNode, ASTDeclaration> _nodesToDeclarations =
            new Dictionary<ASTNode, ASTDeclaration>();

        public bool Add(string name, ASTNode node)
        {
            if (_declarations.ContainsKey(name)) return false;

            var decl = new ASTDeclaration(node);

            _declarations.Add(name, decl);
            _nodesToDeclarations.Add(node, decl);

            return true;
        }

        public bool Remove(string name)
        {
            if (!_declarations.ContainsKey(name)) return false;

            var decl = _declarations[name];

            _declarations.Remove(name);

            if (decl is ASTDeclaration)
            {
                var node = (decl as ASTDeclaration).Node;

                _nodesToDeclarations.Remove(node);
            }

            return true;
        }

        public Declaration Get(string name)
        {
            return _declarations[name];
        }

        public void ReplaceNode(ASTNode oldNode, ASTNode newNode)
        {
            var decl = _nodesToDeclarations[oldNode];

            if (decl != null)
            {
                decl.Node = newNode;
            }
        }

        private static readonly string[] NativeTypes = {
            "int",
            "long",
            "float",
            "double",
            "bool",
            "char",
            "string",
            "unit"
        };

        public bool IsTypeKnown(TypeNode type)
        {
            if (type is TypeUnitNode) return true;

            if (type is TypePropertyNode)
            {
                var property = (type as TypePropertyNode).Property;
                var asString = property.PropertyAsString();

                return NativeTypes.Contains(asString);
            }

            if (type is TypeFunctionNode)
            {
                var typeFunc = type as TypeFunctionNode;

                var knownTypes = typeFunc.Parameters.Where(x => IsTypeKnown(x));

                return 
                    IsTypeKnown(typeFunc.ReturnType) 
                    && knownTypes.Count() == typeFunc.Parameters.Count();
            }

            return false;
        }
    }
}
