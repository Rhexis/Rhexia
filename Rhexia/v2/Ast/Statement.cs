namespace Rhexia.v2.Ast;

public abstract class Statement
{
    protected Statement() {}
}

public enum StatementKind
{
    Var,
    Function,
    For,
    While,
    Expression,
}