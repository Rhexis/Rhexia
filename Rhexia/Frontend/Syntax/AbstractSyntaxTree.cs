using Rhexia.Frontend.Syntax.Statements;

namespace Rhexia.Frontend.Syntax;

public class AbstractSyntaxTree
{
    public List<Statement> Statements { get; } = [];
    
    public override string ToString()
    {
        return $"AST: [ {string.Join(", ", Statements)} ]";
    }
}