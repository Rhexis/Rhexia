namespace Rhexia.v2.Ast.Statements;

public record FunctionStatement(string Name, List<Statement> Parameters, List<Statement> Body) 
    : Statement(StatementKind.Function)
{
    public override string ToString()
    {
        return $"Function: {{ Name: {Name}, Parameters: [{string.Join(", ", Parameters)}], Body: [{string.Join(", ", Body)}] }}";
    }
}