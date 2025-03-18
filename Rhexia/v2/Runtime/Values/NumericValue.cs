namespace Rhexia.v2.Runtime.Values;

public record NumericValue(double Value) : Value(ValueType.Numeric);