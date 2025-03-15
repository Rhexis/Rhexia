namespace Rhexia.v2.Ast.Statements;

public abstract record Statement(StatementKind Kind);

public enum StatementKind
{
    Var,
    Function,
    For,
    While,
    IfElse,
    Expression,
    Return,
}