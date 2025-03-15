using Rhexia.v2.Ast.Statements;
using Rhexia.v2.Lex;

namespace Rhexia.v2.Ast;

public class Parser
{
    public Lexer Lexer { get; }
    
    private Token _current = new (TokenKind.EndOfFile, Lexer.EOF);
    private Token _next = new (TokenKind.EndOfFile, Lexer.EOF);

    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        Next();
    }

    private void Next()
    {
        _current = _next;
        _next = Lexer.Analyse();
    }

    public AbstractSyntaxTree Parse()
    {
        var tree = new AbstractSyntaxTree();

        while (true)
        {
            Next();
            if (_current.Kind == TokenKind.EndOfFile) break;
            var statement = ParseStatement();
            if (statement == null) break;
            tree.Statements.Add(statement);
        }
        
        return tree;
    }

    private Statement? ParseStatement()
    {
        if (_current.Kind is TokenKind.EndOfFile or TokenKind.Semicolon)
        {
            return null;
        }
        
        return _current.Kind switch
        {
            TokenKind.For => ParseFor(),
            TokenKind.Function => ParseFunction(),
            TokenKind.If => ParseIfElse(),
            TokenKind.Return => ParseReturn(),
            TokenKind.Var => ParseVar(),
            TokenKind.While => ParseWhile(),
            _ => ParseExpr()
        };
    }

    private VarStatement ParseVar()
    {
        return null;
    }

    private ForStatement ParseFor()
    {
        return null;
    }

    private IfElseStatement ParseIfElse()
    {
        return null;
    }

    private ReturnStatement ParseReturn()
    {
        return null;
    }

    private FunctionStatement ParseFunction()
    {
        return null;
    }

    private WhileStatement ParseWhile()
    {
        return null;
    }

    private ExprStatement ParseExpr()
    {
        return null;
    }
}