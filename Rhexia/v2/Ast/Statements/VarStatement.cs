using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public record VarStatement(string Name, Expr Expr) : Statement(StatementKind.Var)
{
    public override string ToString()
    {
        return $"Var: {{ Name: {Name}, Expr: {Expr} }}";
    }
}