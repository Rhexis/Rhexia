using Rhexia.Ast.Statements;

namespace Rhexia.Ast.Expressions;

public record ClosureExpr(List<Parameter> Parameters, List<Statement> Body) : Expr
{
    public override string ToString()
    {
        return $"Closure: {{ Parameters: [{string.Join(", ", Parameters)}], Body: [{string.Join(", ", Body)}] }}";
    }
}