namespace Rhexia.Frontend.Semantic.Runtime.Values;

public record ListValue(List<Value> Values) : Value(ValueType.List);