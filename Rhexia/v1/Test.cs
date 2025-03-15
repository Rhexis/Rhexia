namespace Rhexia.v1;

public static class Test
{
    public static void Run()
    {
        var code = 
                // "45 - 5;"
                // "var x = 45 - 5 * (46 / (foo * bar) * (x + y));"
                // "var x = 45 - 5;x = x + 10;x = x + 1;"
                "val foo = 50 / 2;" +
                "" +
                "val obj = {" +
                "    x: 100," +
                "    y: 200," +
                "    z: 300," +
                "    foo," +
                "    complex: {bar: true}," +
                "};" +
                "" +
                "var f = obj.complex.bar;" +
                // "foo = obj.foo + 5;" +
                ""
            ;
        
        var parser = new v1.Parser();
        var program = parser.Parse(code);
        Console.WriteLine(program);
        
        var evaluation = v1.Interpreter.Evaluate(program, new v1.Env());
        Console.WriteLine(evaluation);
    }
}