using Rhexia.v2.Ast.Statements;

namespace Rhexia.v2.Ast;

public class AbstractSyntaxTree
{
    public List<Statement> Statements { get; } = [];
    
    public override string ToString()
    {
        return $"AST: [ {string.Join(", ", Statements)} ]";
    }
}