using Rhexia.v2.Ast;
using Rhexia.v2.Lex;
using Rhexia.v2.Runtime;

namespace Rhexia;

internal static class Program
{
    private static void Main(string[] args)
    {
        var code = """
            var name = "Rhexis";
            var age = 30;
            var daysOld = 10_950;
            
            function greet(name)
            {
                print("Hello, " + name);
            }
            
            greet(name);
            
            print("Age is: " + age);
            print("Last year your age was: " + age - 1);
            print("Name is: " + name);
            
            if (age == 30)
            {
                print("You are 30 years old!");
            }
            else
            {
                print(age + " is " + age * 365 + " days old");
            }
            
            if (!false) { print("true"); }
            
            function addOne(num)
            {
                return num + 1;
            }
            
            print(addOne(5));
        
            var func = function()
            {
                print("I am a closure");
            }
            
            func();
            
            var arr = [1, 2, 3];
            arr[0] = 100;
            print(arr[0]);
            
            var idx = 0;
            while (idx < arr.Length)
            {
                print(arr[idx]);
                idx++;
            }
        
            for (var i = 0; i < 10; i++)
            {
                print("For:[" + i + "]");
            }
        
            struct object {
                name: "Rhexis",
                age: 30,
            }
        """;

        // TODO :: Fix issue with order of operations.
        // In this example, it tries to multiply age by "365 days old".
        var code2 = """
            var age = 32;            
                    
            if (age == 30)
            {
                print("You are 30 years old!");
            }
            else
            {
                print(age + " is " + age * 365 + " days old");
            }
        """;
        
        var lex = new Lexer(code);
        var parser = new Parser(lex);
        var ast = parser.Parse();
        var interpreter = new Interpreter(ast);

        interpreter.Execute();
        
        Console.WriteLine(ast);
    }
}