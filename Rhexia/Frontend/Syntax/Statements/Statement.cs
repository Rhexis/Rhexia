namespace Rhexia.Frontend.Syntax.Statements;

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
    Object,
}