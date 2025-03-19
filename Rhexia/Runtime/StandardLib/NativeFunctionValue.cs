using Rhexia.Runtime.Values;
using ValueType = Rhexia.Runtime.Values.ValueType;

namespace Rhexia.Runtime.StandardLib;

public delegate Value NativeFunctionCall(List<Value> args);

public record NativeFunctionValue(NativeFunctionCall Call) : Value(ValueType.Native);