using System;

namespace UnityComponentUI.Editor.Language
{
    public enum TokenKind
    {
        EOF = 1,
        BANG = 2,
        DOLLAR = 3,
        PAREN_L = 4,
        PAREN_R = 5,
        SPREAD = 6,
        COLON = 7,
        EQUALS = 8,
        AT = 9,
        BRACKET_L = 10,
        BRACKET_R = 11,
        BRACE_L = 12,
        PIPE = 13,
        BRACE_R = 14,
        NAME = 15,
        INT = 16,
        FLOAT = 17,
        STRING = 18,
        LT = 19,
        GT = 20,
        FWD_SLASH = 21,
        SEMICOLON = 22,
        DOT = 23,
        STAR = 24,
        MINUS = 25,
        PLUS = 26,
        WHITESPACE = 27,
        TILDA = 28
    }

    public class Token
    {
        public int End { get; set; }
        public TokenKind Kind { get; set; }
        public int Start { get; set; }
        public string Value { get; set; }

        public static string GetTokenKindDescription(TokenKind kind, string value)
        {
            switch (kind)
            {
                case TokenKind.EOF: return "EOF";
                case TokenKind.BANG: return "!";
                case TokenKind.DOLLAR: return "$";
                case TokenKind.PAREN_L: return "(";
                case TokenKind.PAREN_R: return ")";
                case TokenKind.SPREAD: return "...";
                case TokenKind.COLON: return ":";
                case TokenKind.EQUALS: return "=";
                case TokenKind.AT: return "@";
                case TokenKind.BRACKET_L: return "[";
                case TokenKind.BRACKET_R: return "]";
                case TokenKind.BRACE_L: return "{";
                case TokenKind.PIPE: return "|";
                case TokenKind.BRACE_R: return "}";
                case TokenKind.NAME: return value;
                case TokenKind.INT: return value;
                case TokenKind.FLOAT: return value;
                case TokenKind.STRING: return $"\"{value}\"";
                case TokenKind.LT: return "<";
                case TokenKind.GT: return ">";
                case TokenKind.FWD_SLASH: return "/";
                case TokenKind.STAR: return "*";
                case TokenKind.SEMICOLON: return ";";
                case TokenKind.DOT: return ".";
                case TokenKind.MINUS: return "-";
                case TokenKind.PLUS: return "+";
                case TokenKind.TILDA: return "~";
                case TokenKind.WHITESPACE: return value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public override string ToString()
        {
            return GetTokenKindDescription(this.Kind, this.Value);
        }
    }
}
