using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.UnityComponentUI.Editor.Language;

namespace UnityComponentUI.Editor.Language
{
    public class XLuaElementConverter
    {
        public string Convert(string source)
        {
            var lexer = new Lexer(source, 0);
            var elements = new List<LuaXElement>();
            var offset = 0;

            Token nextToken = null;

            do
            {
                nextToken = lexer.NextToken();

                if (nextToken.Kind == TokenKind.LT)
                {
                    var element = TryParseElement(nextToken, ref lexer);

                    if (element != null)
                    {
                        elements.Add(element);
                    }
                }

            } while (nextToken.Kind != TokenKind.EOF);

            foreach (var element in elements)
                source = element.Replace(source, ref offset);

            return source;
        }

        private LuaXElement TryParseElement(Token startingToken, ref Lexer lexer)
        {
            var children = new List<LuaXElement>();
            var props = new List<Tuple<string, string>>();


            if (startingToken.Kind == TokenKind.BRACE_L)
            {
                var nested = 1;
                var sb = new StringBuilder();
                Token token = null;

                do
                {
                    token = lexer.NextToken();

                    if (token.Kind == TokenKind.BRACE_L) nested++;
                    else if (token.Kind == TokenKind.BRACE_R) nested--;
                    else sb.Append(token.ToString());

                } while (nested > 0);

                return new LuaXCodeElement()
                {
                    Start = startingToken,
                    End = token,
                    Name = token.Value,
                    Code = sb.ToString()
                }; ;
            }

            var nameToken = lexer.NextToken();

            if (nameToken.Kind != TokenKind.NAME)
            {
                return null;
            }

            while (true)
            {
                var nextToken = lexer.NextToken();

                switch (nextToken.Kind)
                {
                    case TokenKind.GT:
                    {
                        TryParseChildren(ref lexer, children);

                        return new LuaXComponentElement()
                        {
                            Start = startingToken,
                            End = ParseTerminatingElement(lexer, nameToken.Value),
                            Name = nameToken.Value,
                            Children = children,
                            Props = props
                        };
                    }
                    case TokenKind.FWD_SLASH:
                    {
                        var endToken = lexer.NextToken();

                        if (endToken.Kind != TokenKind.GT)
                        {
                            return null;
                        }

                        return new LuaXComponentElement()
                        {
                            Start = startingToken,
                            End = endToken,
                            Name = nameToken.Value,
                            Children = children,
                            Props = props
                        };
                    }
                    case TokenKind.NAME:
                    {
                        props.Add(ParseProp(nextToken.Value, lexer));
                        break;
                    }
                    default:
                        throw new CSLSyntaxErrorException($"Unexpected token {nextToken.Kind}", nextToken.Start);
                }
            }
        }

        private Tuple<string, string> ParseProp(string propName, Lexer lexer)
        {
            var prop = new StringBuilder();
            var braceNum = 1;

            Check(lexer, TokenKind.EQUALS);
            Check(lexer, TokenKind.BRACE_L);

            var token = lexer.NextToken();
            while (token.Kind != TokenKind.EOF && braceNum > 0)
            {
                if (token.Kind == TokenKind.BRACE_L) braceNum++;
                if (token.Kind == TokenKind.BRACE_R) braceNum--;

                if (braceNum == 0)
                {
                    return new Tuple<string, string>(propName, prop.ToString());
                }

                prop.Append(token);
                token = lexer.NextToken();
            }

            return new Tuple<string, string>(propName, prop.ToString());
        }

        private Token ParseTerminatingElement(Lexer lexer, string elementName)
        {
            Check(lexer, TokenKind.LT);
            Check(lexer, TokenKind.FWD_SLASH);
            Check(lexer, TokenKind.NAME, elementName);

            return Check(lexer, TokenKind.GT);
        }

        private Token Check(Lexer lexer, TokenKind kind, string value = null)
        {
            var token = lexer.NextToken();

            if (token.Kind != kind)
            {
                throw new CSLSyntaxErrorException($"Expected {kind} got {token.Kind}", token.Start);
            }

            if (token.Value != value)
            {
                throw new CSLSyntaxErrorException($"Expected {kind} to have value {value}, got {token.Value}", token.Start);
            }

            return token;
        }

        private void TryParseChildren(ref Lexer lexer, List<LuaXElement> children)
        {
            LuaXElement nested;

            do
            {
                var nestedLexer = new Lexer(lexer);
                nested = TryParseElement(nestedLexer.NextToken(), ref nestedLexer);

                if (nested != null)
                {
                    children.Add(nested);
                    lexer = nestedLexer;
                }
            } while (nested != null);
        }
    }
}
