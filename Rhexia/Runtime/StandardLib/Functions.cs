using System.Globalization;
using Rhexia.Runtime.Values;
using ValueType = Rhexia.Runtime.Values.ValueType;

namespace Rhexia.Runtime.StandardLib;

public static class Functions
{
    public static Value Print(List<Value> args)
    {
        var arg = args.First();
        Console.WriteLine(arg.AsString());
        return new NullValue();
    }
    
    public static string AsString(this Value value)
    {
        return value.Type switch
        {
            ValueType.String => ((StringValue)value).Value,
            ValueType.Numeric => ((NumericValue)value).Value.ToString(CultureInfo.InvariantCulture),
            ValueType.Bool => ((BoolValue)value).Value.ToString(),
            ValueType.Null => "",
            // ValueType.List => ((ListValue)value).Values,
            
            _ => value.ToString()
        };
    }
}