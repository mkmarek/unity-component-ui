using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Engine.Render
{
    public class Reconciler
    {
        private readonly UIComponentDefinition rootComponent;
        private Element rootElement;

        public Reconciler(UIComponentDefinition rootComponent)
        {
            this.rootComponent = rootComponent;
        }

        public void BuildTree()
        {
            rootElement = Element.Create(this.rootComponent.Create());

            var stack = new Stack<Element>();
            stack.Push(rootElement);

            // build tree non-recursively
            while (stack.Count > 0)
            {
                var element = stack.Pop();

                element.Render();

                foreach (var child in element.Children)
                {
                    stack.Push(child);
                }
            }
        }
    }
}
