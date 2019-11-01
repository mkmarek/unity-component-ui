using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityComponentUI.Editor.Language;
using UnityComponentUI.Engine;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;

namespace Assets.Package.Tests.Engine
{
    public class ElementTreeTests
    {
        private readonly XLuaElementConverter converter = new XLuaElementConverter();
        private MockComponentIndex index;

        [SetUp]
        public void SetUp()
        {
            this.index = new MockComponentIndex();
            ComponentPool.Initialize(index);
        }

        [Test]
        public void ComponentWithOneElement_CreatesCorrectElementTree()
        {
            var component = UIComponentDefinition.Create("Test", converter.Convert(@"
            function render(props)
	            return (
		            <Text
			            textAnchor={""MiddleCenter""}
                        alignByGeometry={""false""}
                        text={props.test}
                    />
                )
            end"));

            var rootElement = Element.Create(component, new PropCollection(new Dictionary<string, object>(){{"test", "some text"}}));

            rootElement.Render();

            Assert.AreEqual(1, rootElement.Children.Count);
            Assert.AreEqual("TextComponent", rootElement.Children.First().Component.Name);
            Assert.AreEqual("MiddleCenter", rootElement.Children.First().Props["textAnchor"]);
            Assert.AreEqual("false", rootElement.Children.First().Props["alignByGeometry"]);
            Assert.AreEqual("some text", rootElement.Children.First().Props["text"]);
        }

        [Test]
        public void ComponentWithTwoNestedElements_CreatesCorrectElementTree()
        {
            var component = UIComponentDefinition.Create("Test", converter.Convert(@"
            function render(props)
	            return (
                    <Button>
		                <Text
			                textAnchor={""MiddleCenter""}
                            alignByGeometry={""false""}
                            text={props.test}
                        />
                    </Button>
                )
            end"));

            var rootElement = Element.Create(component, new PropCollection(new Dictionary<string, object>() { { "test", "some text" } }));
            rootElement.Render();

            Assert.AreEqual(1, rootElement.Children.Count);
            Assert.AreEqual("ButtonComponent", rootElement.Children.First().Component.Name);

            var nested = rootElement.Children.First().Children.First();
            Assert.AreEqual("TextComponent", nested.Component.Name);
            Assert.AreEqual("MiddleCenter", nested.Props["textAnchor"]);
            Assert.AreEqual("false", nested.Props["alignByGeometry"]);
            Assert.AreEqual("some text", nested.Props["text"]);
        }

        [Test]
        public void MultipleNestedCustomComponents_CreatesCorrectElementTree()
        {
            var component1 = UIComponentDefinition.Create("Test1", converter.Convert(@"
            function render(props)
	            return (
                    <Button id={""level1""}> 
                        <Button id={""level2""}>
                            {props.children}
                        </Button>
                    </Button>
                )
            end"));
            this.index.Components.Add("Test1", component1);

            var component2 = UIComponentDefinition.Create("Test2", converter.Convert(@"
            function render(props)
	            return (
                    <Test1>
                        <Button>
		                    <Text
			                    textAnchor={""MiddleCenter""}
                                alignByGeometry={""false""}
                                text={props.test}
                            />
                        </Button>
                    </Test1>
                )
            end"));

            var rootElement = Element.Create(component2, new PropCollection(new Dictionary<string, object>() { { "test", "some text" } }));
            Reconciler.CreateElementTree(rootElement);

            Assert.AreEqual(1, rootElement.Children.Count);
            Assert.AreEqual("Test1", rootElement.Children.First().Component.Name);
        }
    }
}
