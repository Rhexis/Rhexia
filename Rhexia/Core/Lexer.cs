using System.Diagnostics.CodeAnalysis;

namespace Rhexia.Core;

public static class Lexer
{
    public enum TokenType
    {
        // Literal Types
        Number,
        Bool,
        Identifier,
    
        // Keywords
        Var,
        Val,
        Null,
    
        // Grouping
        OpenRoundBracket,   // (
        CloseRoundBracket,  // )
        OpenSquareBracket,  // [
        CloseSquareBracket, // ]
        OpenCurlyBracket,   // {
        CloseCurleyBracket, // }
    
        // Operators
        Equals,
        Colon,
        Comma,
        Dot,
        BinaryOperator,
        
        // Terminators
        Semicolon,
        EOF,
    }

    public record Token(string Value, TokenType Type);
    
    public static List<Token> Tokenize(string src)
    {
        var tokens = new List<Token>();
        int index = 0;

        while (index < src.Length)
        {
            if (TryTokenizeSingleChar(src, ref index, out var single))
            {
                tokens.Add(single);
            }
            else if (TryTokenizeMultiDigit(src, ref index, out var digit))
            {
                tokens.Add(digit);
            }
            else if (TryTokenizeMultiLetter(src, ref index, out var letter))
            {
                tokens.Add(letter);
            }
            else if (char.IsWhiteSpace(src[index]))
            {
                index++;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Unrecognized token: [{src[index]}]");
            }
        }
        
        tokens.Add(new Token("EOF", TokenType.EOF));
        
        return tokens;
    }
    
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        {"var", TokenType.Var},
        {"val", TokenType.Val},
        {"null", TokenType.Null},
        {"true", TokenType.Bool},
        {"false", TokenType.Bool},
    };
    
    private static bool TryTokenizeSingleChar(string src, ref int index, [NotNullWhen(true)] out Token? token)
    {
        token = null;
        TokenType? type = src[index] switch
        {
            // Grouping
            '(' => TokenType.OpenRoundBracket,
            ')' => TokenType.CloseRoundBracket,
            '[' => TokenType.OpenSquareBracket,
            ']' => TokenType.CloseSquareBracket,
            '{' => TokenType.OpenCurlyBracket,
            '}' => TokenType.CloseCurleyBracket,
            
            // Operators
            '=' => TokenType.Equals,
            ':' => TokenType.Colon,
            ',' => TokenType.Comma,
            '.' => TokenType.Dot,
            '+' or '-' or '*' or '/' or '%' => TokenType.BinaryOperator,
            
            // Terminator
            ';' => TokenType.Semicolon,
            _ => null
        };

        if (type.HasValue)
        {
            token = new Token(src[index].ToString(), type.Value);
            index++;
        }

        return token != null;
    }

    private static bool TryTokenizeMultiDigit(string src, ref int index, [NotNullWhen(true)] out Token? token)
    {
        token = null;
        if (!char.IsDigit(src[index])) return false;
        
        var num = "";
        while (index < src.Length && char.IsDigit(src[index]))
        {
            num += src[index];
            index++;
        }
        
        token = new Token(num, TokenType.Number);

        return true;
    }

    private static bool TryTokenizeMultiLetter(string src, ref int index, [NotNullWhen(true)] out Token? token)
    {
        token = null;
        if (!char.IsLetter(src[index])) return false;
        
        var identifier = "";
        while (index < src.Length && char.IsLetter(src[index]))
        {
            identifier += src[index];
            index++;
        }
                
        // Check for reserved keywords
        if (Keywords.TryGetValue(identifier, out TokenType reserved))
        {
            token = new Token(identifier, reserved);
        }
        else
        {
            token = new Token(identifier, TokenType.Identifier);
        }

        return true;
    }
}