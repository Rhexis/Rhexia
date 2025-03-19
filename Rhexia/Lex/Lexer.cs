using System.Diagnostics.CodeAnalysis;

namespace Rhexia.Lex;

public class Lexer
{
    // ReSharper disable once InconsistentNaming
    public const char EOF = '\0';
    public string Source { get; }

    private const int Offset = -2;
    private int _index = Offset;
    private char _current = EOF;
    private char _next = EOF;
    
    public Lexer(string? source)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Next();
    }

    private void EatWhitespace()
    {
        while (_current != EOF && char.IsWhiteSpace(_current))
        {
            Next();
        }
    }
    
    private void Next()
    {
        _index++;
        _current = _next;

        if (_index <= Source.Length + Offset)
        {
            _next = Source[_index + 1];
        }
        else
        {
            _next = EOF;
        }
    }

    public Token Analyse()
    {
        Next();
        EatWhitespace();
        
        if (IsSimpleToken(out var simple))
        {
            return new Token(simple.Value, _current.ToString());
        }

        if (IsComplexToken(out var complex))
        {
            return complex;
        }

        if (IsLiteralToken(out var literal))
        {
            return literal;
        }
        
        return new Token(TokenKind.EndOfFile, EOF);
    }

    private bool IsSimpleToken([NotNullWhen(true)] out TokenKind? kind)
    {
        kind = _current switch
        {
            ';' => TokenKind.Semicolon,
            '(' => TokenKind.LeftRoundBracket,
            ')' => TokenKind.RightRoundBracket,
            '*' => TokenKind.Multiply,
            '/' => TokenKind.Divide,
            '%' => TokenKind.Modulus,
            ':' => TokenKind.Colon,
            ',' => TokenKind.Comma,
            '.' => TokenKind.Dot,
            '{' => TokenKind.LeftCurlyBracket,
            '}' => TokenKind.RightCurlyBracket,
            '[' => TokenKind.LeftSquareBracket,
            ']' => TokenKind.RightSquareBracket,
            EOF => TokenKind.EndOfFile,
            _ => null
        };
        return kind != null;
    }

    private bool IsComplexToken([NotNullWhen(true)] out Token? token)
    {
        token = null;

        if (_current == '=')
        {
            if (_next == '=')
            {
                token = new Token(TokenKind.EqualTo, $"{_current}{_next}");
                // Eat the first '='
                Next();
            }
            else
            {
                token = new Token(TokenKind.Assign, _current.ToString());
            }
        }
        else if (_current == '>')
        {
            if (_next == '=')
            {
                token = new Token(TokenKind.GreaterThanOrEqualTo, $"{_current}{_next}");
                // Eat the '>'
                Next();
            }
            else
            {
                token = new Token(TokenKind.GreaterThan, _current.ToString());
            }
        }
        else if (_current == '<')
        {
            if (_next == '=')
            {
                token = new Token(TokenKind.LessThanOrEqualTo, $"{_current}{_next}");
                // Eat the '<'
                Next();
            }
            else
            {
                token = new Token(TokenKind.LessThan, _current.ToString());
            }
        }
        else if (_current == '!')
        {
            if (_next == '=')
            {
                token = new Token(TokenKind.NotEqualTo, $"{_current}{_next}");
                // Eat the '!'
                Next();
            }
            else
            {
                token = new Token(TokenKind.Not, _current.ToString());
            }
        }
        else if (_current == '&')
        {
            if (_next == '&')
            {
                token = new Token(TokenKind.And, $"{_current}{_next}");
                // Eat the first '&'
                Next();
            }
            else
            {
                token = new Token(TokenKind.Ampersand, _current.ToString());
            }
        }
        else if (_current == '|')
        {
            if (_next == '|')
            {
                token = new Token(TokenKind.Or, $"{_current}{_next}");
                // Eat the first '|'
                Next();
            }
            else
            {
                token = new Token(TokenKind.Pipe, _current.ToString());
            }
        }
        else if (_current == '+')
        {
            if (_next == '+')
            {
                token = new Token(TokenKind.Increment, $"{_current}{_next}");
                // Eat the first '+'
                Next();
            }
            else
            {
                token = new Token(TokenKind.Plus, _current.ToString());
            }
        }
        else if (_current == '-')
        {
            if (_next == '-')
            {
                token = new Token(TokenKind.Decrement, $"{_current}{_next}");
                // Eat the first '-'
                Next();
            }
            else
            {
                token = new Token(TokenKind.Minus, _current.ToString());
            }
        }
        
        return token != null;
    }

    private bool IsLiteralToken([NotNullWhen(true)] out Token? token)
    {
        token = null;

        if (char.IsLetter(_current))
        {
            // Could be the start of a reserved word
            var lastToken = MakeIdentifierLiteral();

            if (IsReserved(lastToken.Literal.ToString() ?? string.Empty, out var kind))
            {
                // Must be a reserved token
                token = new Token(kind.Value, lastToken.Literal.ToString()!);
            }
            else
            {
                // Must be an identifier
                token = new Token(TokenKind.Identifier, lastToken.Literal.ToString() ?? string.Empty);
            }
        }
        else if (char.IsDigit(_current))
        {
            token = MakeNumericLiteral();
        }
        else if (_current == '\"')
        {
            token = MakeStringLiteral();
        }
        
        return token != null;
    }

    private Token MakeNumericLiteral()
    {
        // Snapshot current index
        var currentIndex = _index;
        var dots = 0;

        while (_current != EOF && "0123456789._".Contains(_next))
        {
            if (_current == '.')
            {
                dots++;
            }
            Next();
        }

        if (dots > 1) throw new FormatException("Invalid numeric literal.");
        
        // Get chars containing literal
        var literal = Source.Substring(currentIndex, _index - currentIndex + 1);
        // Allow '_' for number formatting but strip them out when internally.
        return new Token(TokenKind.NumericLiteral, literal.Replace("_", ""));
    }

    private Token MakeStringLiteral()
    {
        // Eat the opening double quote (")
        Next();
        // Snapshot current index
        var currentIndex = _index;

        // While still fetching chars within double quotes
        while (_current != EOF && _current != '\"')
        {
            Next();
        }
        
        // Get chars containing literal
        var literal = Source.Substring(currentIndex, _index - currentIndex);
        
        return new Token(TokenKind.StringLiteral, literal);
    }
    
    private Token MakeIdentifierLiteral()
    {
        // Snapshot current index
        var currentIndex = _index;
        
        // While
        //  not at end of file
        //  and not end of line 
        //  and still processing letters
        // Keep counting to get length of literal.
        while (_current != EOF 
               && _current != ';'
               && (
                   char.IsLetter(_next)
                   || _next == '_'
               ))
        {
            Next();
        }
        
        // Get chars containing literal
        var literal = Source.Substring(currentIndex, _index - currentIndex + 1);
        
        return new Token(TokenKind.Identifier, literal);
    }

    private bool IsReserved(string literal, [NotNullWhen(true)] out TokenKind? kind)
    {
        kind = literal.ToLowerInvariant() switch
        {
            "var" => TokenKind.Var,
            "for" => TokenKind.For,
            "while" => TokenKind.While,
            
            "true" => TokenKind.True,
            "false" => TokenKind.False,
            "null" => TokenKind.Null,
            
            "object" => TokenKind.Object,
            "function" => TokenKind.Function,
            "in" => TokenKind.In,
            "if" => TokenKind.If,
            "else" => TokenKind.Else,
            "return" => TokenKind.Return,
            
            _ => TokenKind.EndOfFile
        };
        
        return kind != TokenKind.EndOfFile;
    }
}