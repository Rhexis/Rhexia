namespace Rhexia.Runtime.Values;

public abstract record Value(ValueType Type);

public enum ValueType
{
    Numeric,
    String,
    Bool,
    Null,
    Function,
    Struct,
    List,
    Native,
}