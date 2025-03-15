using Rhexia.v2.Lex;

namespace Rhexia;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
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
            print("Name is: " + name);
            
            if (age == 30)
            {
                print("You are 30 years old!");
            }
            else
            {
                print(age + " is " + age * 365 + " days old");
            }
        
            for(i in range(age))
            {
                print(i);
            }
        """;

        var lex = new Lexer(code);
        var tokens = new List<Token>();
        
        var token = lex.Analyse();
        do
        {
            tokens.Add(token);
            token = lex.Analyse();
        }
        while (token.Kind != TokenKind.EndOfFile);

        tokens.ForEach(Console.WriteLine);
    }
}