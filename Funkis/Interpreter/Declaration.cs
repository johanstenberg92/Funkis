namespace IronJS.Interpreter
{
    public abstract class Declaration
    {
    }

    public class ASTDeclaration : Declaration
    {
        public ASTNode Node { get; set; }

        public ASTDeclaration(ASTNode node)
        {
            Node = node;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ASTDeclaration;

            if (other == null) return false;

            return Node.Equals(other.Node);
        }

        public override int GetHashCode()
        {
            int hash = 31;
            hash *= Node.GetHashCode();

            return hash;
        }
    }
}
