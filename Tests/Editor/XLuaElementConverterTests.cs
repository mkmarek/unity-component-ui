using NUnit.Framework;
using UnityComponentUI.Editor.Language;

namespace UnityComponentUI.Tests
{
    public class XLuaElementConverterTests
    {
        private readonly XLuaElementConverter converter = new XLuaElementConverter();

        [Test]
        public void TemplateWithOneComponentSelfTerminating()
        {
            var original = @"
function render()
	return <something />
end";
            var expected = @"
function render()
	return Create(""something"")
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithMultipleComponentsSelfTerminating()
        {
            var original = @"
function render()
	local a = <a />
    local b = <b />
    local c = <c />
end";
            var expected = @"
function render()
	local a = Create(""a"")
    local b = Create(""b"")
    local c = Create(""c"")
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentWithOneChild()
        {
            var original = @"
function render()
	return <something><nested /></something>
end";
            var expected = @"
function render()
	return Create(""something"", { children = { Create(""nested"") } })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentWithMultipleChildren()
        {
            var original = @"
function render()
	return <something><nested /><nested2 /><nested3 /><nested4 /></something>
end";
            var expected = @"
function render()
	return Create(""something"", { children = { Create(""nested""), Create(""nested2""), Create(""nested3""), Create(""nested4"") } })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentWithEmptyBody()
        {
            var original = @"
function render()
	return <something></something>
end";
            var expected = @"
function render()
	return Create(""something"")
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentWithMultipleLevelNesting()
        {
            var original = @"
function render()
	return <something><nested><nested2 /><nested3><nested5 /></nested3><nested4 /></nested></something>
end";
            var expected = @"
function render()
	return Create(""something"", { children = { Create(""nested"", { children = { Create(""nested2""), Create(""nested3"", { children = { Create(""nested5"") } }), Create(""nested4"") } }) } })
end";
            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentSelfTerminatingWithProp()
        {
            var original = @"
function render()
	return <something value={100} />
end";
            var expected = @"
function render()
	return Create(""something"", { value = 100 })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithOneComponentSelfTerminatingWithMultipleProps()
        {
            var original = @"
function render()
	return <something value={100} value2={200} />
end";
            var expected = @"
function render()
	return Create(""something"", { value = 100, value2 = 200 })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithCodeInside()
        {
            var original = @"
function render()
	return <something>{foo()}</something>
end";
            var expected = @"
function render()
	return Create(""something"", { children = { foo() } })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }

        [Test]
        public void TemplateWithPropWithExpression()
        {
            var original = @"
function render()
	return <something prop={screenSize.width / 2 - 150} />
end";
            var expected = @"
function render()
	return Create(""something"", { prop = screenSize.width / 2 - 150 })
end";

            Assert.AreEqual(expected, converter.Convert(original));
        }
    }
}
