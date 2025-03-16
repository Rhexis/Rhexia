using Rhexia.v2.Ast.Statements;

namespace Rhexia.v2.Ast.Expressions;

public record ClosureExpr(List<Parameter> Parameters, List<Statement> Body) : Expr
{
    public override string ToString()
    {
        return $"Closure: {{ Parameters: [{string.Join(", ", Parameters)}], Body: [{string.Join(", ", Body)}] }}";
    }
}