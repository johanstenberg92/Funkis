using Funkis.Standard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Funkis.Compiler
{
    public class AnalysisScope
    {
        private IDictionary<string, Declaration> _declarations =
            new Dictionary<string, Declaration>();

        private IDictionary<ASTNode, ASTDeclaration> _nodesToDeclarations =
            new Dictionary<ASTNode, ASTDeclaration>();

        private ISet<string> _imports = new HashSet<string>();

        private string Namespace;

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
            if (_declarations.ContainsKey(name)) return _declarations[name];

            return null;
        }

        public void ReplaceNode(ASTNode oldNode, ASTNode newNode)
        {
            var decl = _nodesToDeclarations[oldNode];

            if (decl != null)
            {
                decl.Node = newNode;
            }
        }

        public void AddImport(string import)
        {
            _imports.Add(import);
        }

        public void SetNamespace(string ns)
        {
            Namespace = ns;
        }

        private static readonly IDictionary<string, Type> NativeTypes = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
            { "long", typeof(long) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "bool", typeof(bool) },
            { "char", typeof(char) },
            { "string", typeof(string) },
            { "unit", typeof(Unit) }
        };

        private static readonly int FuncMaxLength = 17;

        private static readonly Type[] FuncTypes = {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,,>),
        };

        public CLRTypeNode ConvertToCLRTypeNode(TypeNode type)
        {
            var position = type.Position;

            if (type is TypeUnitNode) return new CLRTypeNode(typeof(Unit), position);

            if (type is TypePropertyNode)
            {
                var property = (type as TypePropertyNode).Property;
                var res = GetPropertyType(property);

                if (res == null) return null;

                return new CLRTypeNode(res, position);
            }

            if (type is TypeFunctionNode)
            {
                var typeFunc = type as TypeFunctionNode;

                var types = new List<Type>();

                foreach (TypeNode parameter in typeFunc.Parameters)
                {
                    var conv = ConvertToCLRTypeNode(parameter);
                    if (conv == null) return null;
                    types.Add(conv.Type);
                }

                var returnTypeConverted = ConvertToCLRTypeNode(typeFunc.ReturnType);
                if (returnTypeConverted == null) return null;
                types.Add(returnTypeConverted.Type);

                var res = GetFuncType(types.ToArray());

                return new CLRTypeNode(res, position);
            }

            return null;
        }

        public Type GetPropertyType(PropertyNode property)
        {
            var asString = property.PropertyAsString();

            if (NativeTypes.ContainsKey(asString))
            {
                return NativeTypes[asString];
            }
            else
            {
                // Cases:
                //
                // It is a) static method from assembly -> (always X.Y.Z.METHOD, not just METHOD)
                //
                // It is b) identifier from own program
                // all imports must be checked here
                // to find it
                var t = Type.GetType(asString);

                return t;
            }
        }

        public Type GetFuncType(Type[] types)
        {
            var numberOfTypes = types.Length;

            if (numberOfTypes > FuncMaxLength || numberOfTypes == 0) return null;

            var typeArgs = types.ToArray();

            var funcType = FuncTypes[numberOfTypes - 1];

            var dummy = funcType.MakeGenericType(typeArgs);

            return dummy.GetType();
        }
    }
}
