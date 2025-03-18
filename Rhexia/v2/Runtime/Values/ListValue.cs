namespace Rhexia.v2.Runtime.Values;

public record ListValue(List<Value> Values) : Value(ValueType.List);