using Assets.Package.Runtime.Engine.Hooks;
using UnityComponentUI.Engine.Render;
using MoonSharp.Interpreter;

namespace UnityComponentUI.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;

        public string Name { get; }

        public UIComponent(string componentName, Script state, ConnectedSystem system)
        {
            this.state = state;
            this.Name = componentName;
        }

        public void Render(IRootElementBuilder parent, Element container, int? key = null)
        {
            HookComponentRegistration.CurrentComponent = this;
            HookComponentRegistration.CurrentParent = parent;
            HookComponentRegistration.CurrentContainer = container;
            HookComponentRegistration.CurrentKey = key;

            HookComponentRegistration.ResetHookCounter();

            var elementId = state.Call(state.Globals["render"], container.Props).CastToString();

            HookComponentRegistration.CurrentComponent = null;
            HookComponentRegistration.CurrentParent = null;
            HookComponentRegistration.CurrentContainer = null;
            HookComponentRegistration.CurrentKey = null;

            var element = Element.GetById(elementId);

            element?.Render(parent, key);
        }
    }
}
