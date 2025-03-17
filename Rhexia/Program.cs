using Rhexia.v2.Ast;
using Rhexia.v2.Lex;

namespace Rhexia;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        // var code = """
        //     var name = "Rhexis";
        //     var age = 30;
        //     var daysOld = 10_950;
        //     
        //     function greet(name)
        //     {
        //         print("Hello, " + name);
        //     }
        //     
        //     greet(name);
        //     
        //     print("Age is: " + age);
        //     print("Last year your age was: " + age - daysOld);
        //     print("Name is: " + name);
        //     
        //     if (age == 30)
        //     {
        //         print("You are 30 years old!");
        //     }
        //     else
        //     {
        //         print(age + " is " + age * 365 + " days old");
        //     }
        //         
        //     var arr = [1, 2, 3];
        //
        //     for(i in range(age))
        //     {
        //         print(i);
        //     }
        // """;
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
            print("Last year your age was: " + age - daysOld);
            print("Name is: " + name);
            
            if (age == 30)
            {
                print("You are 30 years old!");
            }
            else
            {
                print(age + " is " + age * 365 + " days old");
            }
            
            var arr = [1, 2, 3];
            var idx = 0;
            while (idx < arr.Length)
            {
                print(arr[idx]);
                idx++;
            }
            
            if (!false) { print("true"); }
            
            for (var i = 0; i < 10; i++)
            {
                print("For:[" + i + "]");
            }
        """;

        var lex = new Lexer(code);
        var parser = new Parser(lex);
        var ast = parser.Parse();
        
        Console.WriteLine(ast);
    }
}