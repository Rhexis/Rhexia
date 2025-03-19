namespace Rhexia.Runtime.Values;

public record StringValue(string Value) : Value(ValueType.String);