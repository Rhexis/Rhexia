using Rhexia.Frontend.Syntax.Expressions;

namespace Rhexia.Frontend.Syntax.Statements;

public record VarStatement(string Name, Expr Expr) : Statement(StatementKind.Var)
{
    public override string ToString()
    {
        return $"Var: {{ Name: {Name}, Expr: {Expr} }}";
    }
}