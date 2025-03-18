using Rhexia.v2.Runtime.Values;
using ValueType = Rhexia.v2.Runtime.Values.ValueType;

namespace Rhexia.v2.Runtime.StandardLib;

public delegate Value NativeFunctionCall(List<Value> args);

public record NativeFunctionValue(NativeFunctionCall Call) : Value(ValueType.Native);