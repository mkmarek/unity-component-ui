using NUnit.Framework;
using UnityComponentUI.Editor.Language;

namespace UnityComponentUI.Tests
{
    public class LexerTests
    {
        [Test]
        public void TemplateWithOneComponent()
        {
            var lexer = new Lexer(@"
template: {
    <Container />
}
", 0);

            var next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("template", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.COLON, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_L, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.LT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Container", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.FWD_SLASH, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.GT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_R, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.EOF, next.Kind);
        }

        [Test]
        public void TemplateWithNesterComponent()
        {
            var lexer = new Lexer(@"
template: {
    <Container><Nested /></Container>
}
", 0);

            var next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("template", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.COLON, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_L, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.LT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Container", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.GT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.LT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Nested", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.FWD_SLASH, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.GT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.LT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.FWD_SLASH, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Container", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.GT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_R, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.EOF, next.Kind);
        }

        [Test]
        public void TemplateWithPropsAndOneComponent()
        {
            var lexer = new Lexer(@"
props: {
    iterations: Int32
}
template: {
    <Container />
}
", 0);

            var next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("props", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.COLON, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_L, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("iterations", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.COLON, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Int32", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_R, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("template", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.COLON, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_L, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.LT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.NAME, next.Kind);
            Assert.AreEqual("Container", next.Value);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.FWD_SLASH, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.GT, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.BRACE_R, next.Kind);

            next = lexer.NextToken();
            Assert.AreEqual(TokenKind.EOF, next.Kind);
        }
    }
}
