using Rhexia.v2.Ast.Expressions;

namespace Rhexia.v2.Ast.Statements;

public abstract record Statement(StatementKind Kind);

public abstract record StatementExpression(Expr Expr) : Statement(StatementKind.Expression);

public enum StatementKind
{
    Var,
    Function,
    For,
    While,
    Expression,
}