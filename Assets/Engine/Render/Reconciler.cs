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
            var rootElementId = this.rootComponent.Create().Render();
            rootElement = Element.GetById(rootElementId);

            var stack = new Stack<Element>();
            stack.Push(rootElement);

            // build tree non-recursively
            while (stack.Count > 0)
            {
                var element = stack.Pop();

                if (element.Props?.ContainsKey("children") == false)
                {
                    continue;
                }

                if (!(element.Props?["children"] is Table children))
                {
                    continue;
                }

                foreach (var child in children.Values)
                {
                    var childElement = Element.GetById(child.CastToString());

                    element.Children.Add(childElement);
                    stack.Push(childElement);
                }
            }
        }
    }
}
