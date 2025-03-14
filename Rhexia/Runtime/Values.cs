using Rhexia.Core;

namespace Rhexia.Runtime;

public enum ValueTypes
{
    Null,
    Number,
    Bool,
    Object,
    Function,
}

public record Value(ValueTypes Type);

public record NullValue() : Value(ValueTypes.Null)
{
    public string Value { get; init; } = "null";
}

public record NumberValue(int Value) : Value(ValueTypes.Number);

public record BoolValue(bool Value) : Value(ValueTypes.Bool);

public record ObjectValue(Dictionary<string, Value> Properties) : Value(ValueTypes.Object);

public record FunctionValue(string Name, List<string> Parameters, Env Env, List<Ast.Statement> Body) : Value(ValueTypes.Function);