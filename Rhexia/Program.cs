﻿using Rhexia.Ast;
using Rhexia.Lex;
using Rhexia.Runtime;

namespace Rhexia;

internal static class Program
{
    private static void Main(string[] args)
    {
        var sample = "Code/sample.ria";
        //var orderOfOps = "Code/order_of_ops.ria"; // TODO :: fix this order of ops bug.
        var source = File.ReadAllText(sample);
        
        var lex = new Lexer(source);
        var parser = new Parser(lex);
        var ast = parser.Parse();
        var interpreter = new Interpreter(ast);

        interpreter.Execute();
    }
}