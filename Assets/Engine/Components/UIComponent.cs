using Assets.Engine.Render;
using MoonSharp.Interpreter;

namespace Assets.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;

        public UIComponent(Script state)
        {
            this.state = state;
        }

        public void Render(Element container)
        {
            var elementId = state.Call(state.Globals["render"]).CastToString();

            container.Children.Add(Element.GetById(elementId));
        }
    }
}
