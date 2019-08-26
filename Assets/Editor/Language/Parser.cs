using System.Data;
using System.Diagnostics;
using Assets.Editor.Language.AST;

namespace Assets.Editor.Language
{
    public class Parser
    {
        public ASTDocument Parse(string source)
        {
            var document = new ASTDocument();
            var lexer = new Lexer(source, 0);

            var token = lexer.NextToken();

            if (token.Kind == TokenKind.NAME && token.Value == "template")
            {
                Check(lexer, TokenKind.COLON);
                document.Template = this.ParseTemplate(lexer);
            }
            else
            {
                throw new CSLSyntaxErrorException($"Expected template got {token}", token.Start);
            }

            return document;
        }

        private ASTTemplateDefinition ParseTemplate(Lexer lexer)
        {
            var definition = new ASTTemplateDefinition();

            Check(lexer, TokenKind.BRACE_L);

            definition.RootNode = this.ParseTemplateNode(lexer);

            Check(lexer, TokenKind.BRACE_R);

            return definition;
        }

        private ASTTemplateNode ParseTemplateNode(Lexer lexer)
        {
            var node = new ASTTemplateNode();

            Check(lexer, TokenKind.LT);

            node.Name = GetName(lexer);

            ParseNodeTermination(node, lexer);

            return node;
        }

        private void ParseNodeTermination(ASTTemplateNode node, Lexer lexer)
        {
            var token = lexer.NextToken();

            switch (token.Kind)
            {
                case TokenKind.FWD_SLASH:
                    Check(lexer, TokenKind.GT);
                    break;
                case TokenKind.GT:
                    ParseNestedNodes(node, lexer);
                    break;
                default:
                    throw new CSLSyntaxErrorException($"Expected node termination got {token}", token.Start);
            }
        }

        private void ParseNestedNodes(ASTTemplateNode node, Lexer lexer)
        {
            bool result;
            do
            {
                result = ParseNodeBody(node, lexer);
            } while (result);
        }

        private bool ParseNodeBody(ASTTemplateNode node, Lexer lexer)
        {
            Check(lexer, TokenKind.LT);

            var token = lexer.NextToken();

            switch (token.Kind)
            {
                case TokenKind.FWD_SLASH:
                    CheckName(lexer, node.Name);
                    Check(lexer, TokenKind.GT);
                    return false;
                case TokenKind.NAME:
                    var child = new ASTTemplateNode();
                    child.Name = token.Value;
                    ParseNodeTermination(child, lexer);
                    node.Children.Add(child);
                    return true;
                default:
                    throw new CSLSyntaxErrorException($"Expected node termination or body got {token}", token.Start);
            }
        }

        private string GetName(Lexer lexer)
        {
            var token = lexer.NextToken();

            if (token.Kind != TokenKind.NAME)
            {
                throw new CSLSyntaxErrorException($"Expected {TokenKind.NAME} got {token}", token.Start);
            }

            return token.Value;
        }

        private void Check(Lexer lexer, TokenKind kind)
        {
            var token = lexer.NextToken();

            if (token.Kind != kind)
            {
                throw new CSLSyntaxErrorException($"Expected {kind} got {token}", token.Start);
            }
        }

        private void CheckName(Lexer lexer, string value)
        {
            var token = lexer.NextToken();

            if (token.Kind != TokenKind.NAME)
            {
                throw new CSLSyntaxErrorException($"Expected {TokenKind.NAME} got {token}", token.Start);
            }

            if (token.Value != value)
            {
                throw new CSLSyntaxErrorException($"Expected {TokenKind.NAME} with value {value} got {token}", token.Start);
            }
        }
    }
}
