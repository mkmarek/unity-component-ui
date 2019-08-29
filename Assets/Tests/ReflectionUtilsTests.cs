using Assets.Engine.Utils;
using NUnit.Framework;

namespace Tests
{
    public class ReflectionUtilsTests
    {
        private class TestClass
        {
            public TestClass Next { get; set; }
            public int SomeInteger { get; set; }
        }

        [Test]
        public void ChainWithOneElement()
        {
            Assert.AreEqual("SomeInteger", 
                ReflectionUtils.GetPropertyChain<TestClass, int>(e => e.SomeInteger));
        }

        [Test]
        public void ChainWithMoreElementsElement()
        {
            Assert.AreEqual("Next.Next.Next.SomeInteger",
                ReflectionUtils.GetPropertyChain<TestClass, int>(e => e.Next.Next.Next.SomeInteger));
        }
    }
}
