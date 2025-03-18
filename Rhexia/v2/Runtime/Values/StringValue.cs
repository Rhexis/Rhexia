namespace Rhexia.v2.Runtime.Values;

public record StringValue(string Value) : Value(ValueType.String);