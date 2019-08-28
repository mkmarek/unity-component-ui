using Assets.Editor.Language;
using NUnit.Framework;

namespace Tests
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

        [Test]
        public void ParsesThroughCSharp()
        {
            var lexer = new Lexer(@"
using System;
using System.Collections.Generic;
using Assets.Engine;
using Assets.Engine.Hierarchy;
using Assets.Engine.Utils;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class UIComponent : ScriptableObject, IBaseUIComponent
    {
        [SerializeField]
        private string componentName;

        [SerializeField]
        private List<HierarchyElement> elements;

        [SerializeField]
        private string rootId;

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        public List<HierarchyElement> Elements
        {
            get => elements;
            set => elements = value;
        }

        public string RootId
        {
            get => rootId;
            set => rootId = value;
        }

        public void Render()
        {
            var rootElement = elements.Find(e => e.Id == RootId);
            rootElement.Render(ComponentPool.Instance);
        }
    }
}

", 0);

            Token next;
            do
            {
                next = lexer.NextToken();
            } while (next.Kind != TokenKind.EOF);

        }
    }
}
