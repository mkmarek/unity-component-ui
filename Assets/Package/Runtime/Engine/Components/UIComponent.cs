using System.Collections.Generic;
using Assets.Package.Runtime.Engine.Hooks;
using UnityComponentUI.Engine.Render;
using MoonSharp.Interpreter;

namespace UnityComponentUI.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;

        public string Name { get; }

        public UIComponent(string componentName, Script state)
        {
            this.state = state;
            this.Name = componentName;
        }

        public (List<Element> children, GameObjectElementBuilder builder) Render(Element container, PropCollection props, List<Element> children)
        {
            HookComponentRegistration.CurrentComponent = this;
            HookComponentRegistration.CurrentContainer = container;
            HookComponentRegistration.ResetHookCounter();

            var elementId = state.Call(state.Globals[$"{Name}_render"], props).CastToString();

            HookComponentRegistration.CurrentComponent = null;
            HookComponentRegistration.CurrentContainer = null;

            var element = Element.GetById(elementId);

            if (element == null)
            {
                return (new List<Element>(), null);
            }

            element.Parent = container;

            return (new List<Element>() {element}, null);
        }
    }
}
