namespace Rhexia.Runtime.Values;

public record NumericValue(double Value) : Value(ValueType.Numeric);