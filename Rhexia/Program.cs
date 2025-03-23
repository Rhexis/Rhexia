using Rhexia.Ast;
using Rhexia.Lex;
using Rhexia.Runtime;

namespace Rhexia;

internal static class Program
{
    private static int Main(string[] args)
    {
        // var sample = "Code/order_of_ops.ria";
        var sample = "Code/sample.ria";
        // var sample = "Code/fizz_buzz.ria";
        var source = File.ReadAllText(sample);
        
        var lex = new Lexer(source);
        var parser = new Parser(lex);
        var ast = parser.Parse();
        var interpreter = new Interpreter(ast);

        return interpreter.Execute();
    }
}