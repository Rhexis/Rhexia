namespace Rhexia.Runtime.Values;

public record ListValue(List<Value> Values) : Value(ValueType.List);