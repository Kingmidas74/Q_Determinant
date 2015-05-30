using Core.Atoms;

namespace Core.Interfaces
{
    public interface ISyntaxTree:IStatus
    {
        Graph SyntaxTree { get; }

        Graph GetSyntaxTree(string filePath);

        Graph GetSyntaxTree(Graph implementationPlan);
    }
}
