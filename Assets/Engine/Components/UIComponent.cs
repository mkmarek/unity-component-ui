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

        public IRootElementBuilder Render(Element container)
        {
            var elementId = state.Call(state.Globals["render"]).CastToString();
            var element = Element.GetById(elementId);

            return new PassThroughElementBuilder(element.Render());
        }
    }
}
