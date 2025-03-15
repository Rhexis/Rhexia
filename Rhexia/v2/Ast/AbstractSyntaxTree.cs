namespace Rhexia.v2.Ast;

public class AbstractSyntaxTree
{
    public List<Statement> Statements { get; set; } = [];
    
    public override string ToString()
    {
        return $"[ {string.Join(", ", Statements)} ]";
    }
}