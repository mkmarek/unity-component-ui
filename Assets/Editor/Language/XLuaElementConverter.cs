using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Editor.Language
{
    public class XLuaElementConverter
    {
        private class Element
        {
            public Token Start { get; set; }
            public Token End { get; set; }
            public string Name { get; set; }
            public List<Element> Children { get; set; }

            public string Replace(string source, ref int offset)
            {
                var componentString = GetComponentMarkup();
                var result = source
                    .Remove(Start.Start + offset, End.End - Start.Start)
                    .Insert(Start.Start + offset, componentString);

                // adjust the offset because of changed string size
                offset = offset + componentString.Length - (End.End - Start.Start);

                return result;
            }

            private string GetComponentMarkup()
            {
                return $"Create(\"{Name}\"{GetPropsMarkup()})";
            }

            private string GetPropsMarkup()
            {
                var childrenMarkup = GetChildrenMarkup();

                return string.IsNullOrWhiteSpace(childrenMarkup)
                    ? string.Empty
                    : $", {{ {childrenMarkup} }}";
            }

            private string GetChildrenMarkup()
            {
                if (Children.Count == 0)
                {
                    return null;
                }

                var childrenComponents = string.Join(", ", Children
                    .Select(e => e.GetComponentMarkup())
                    .ToArray());

                return $"children = {{ {childrenComponents} }}";
            }
        }

        public string Convert(string source)
        {
            var lexer = new Lexer(source, 0);
            var elements = new List<Element>();
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

        private Element TryParseElement(Token startingToken, ref Lexer lexer)
        {
            var children = new List<Element>();

            var nameToken = lexer.NextToken();

            if (nameToken.Kind != TokenKind.NAME)
            {
                return null;
            }

            var terminatorToken = lexer.NextToken();
            var selfTerminating = true;

            if (terminatorToken.Kind == TokenKind.GT)
            {
                selfTerminating = false;

                TryParseChildren(ref lexer, children);
            }
            else if (terminatorToken.Kind != TokenKind.FWD_SLASH)
            {
                return null;
            }

            Token endToken;

            if (!selfTerminating)
            {
                endToken = lexer.NextToken();

                if (endToken.Kind != TokenKind.LT)
                {
                    return null;
                }

                endToken = lexer.NextToken();

                if (endToken.Kind != TokenKind.FWD_SLASH)
                {
                    return null;
                }

                endToken = lexer.NextToken();

                if (endToken.Kind != TokenKind.NAME || endToken.Value != nameToken.Value)
                {
                    return null;
                }

                endToken = lexer.NextToken();

                if (endToken.Kind != TokenKind.GT)
                {
                    return null;
                }
            }
            else
            {
                endToken = lexer.NextToken();

                if (endToken.Kind != TokenKind.GT)
                {
                    Debug.Log(endToken.Kind);
                    return null;
                }
            }

            return new Element()
            {
                Start = startingToken,
                End = endToken,
                Name = nameToken.Value,
                Children = children
            };
        }

        private void TryParseChildren(ref Lexer lexer, List<Element> children)
        {
            Element nested;

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
