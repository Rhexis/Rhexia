using Rhexia.v2.Ast;
using Rhexia.v2.Ast.Expressions;
using Rhexia.v2.Ast.Statements;

namespace Rhexia.v2.Runtime.Values;

public record FunctionValue
(
    string Name,
    List<Parameter> Parameters,
    List<Statement> Body,
    Environment? Env = null,
    Expr? Context = null
) : Value(ValueType.Function);