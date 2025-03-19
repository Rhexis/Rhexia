using Rhexia.Ast.Statements;

namespace Rhexia.Ast;

public class AbstractSyntaxTree
{
    public List<Statement> Statements { get; } = [];
    
    public override string ToString()
    {
        return $"AST: [ {string.Join(", ", Statements)} ]";
    }
}