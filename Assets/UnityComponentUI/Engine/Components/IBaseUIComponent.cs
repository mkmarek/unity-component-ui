using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components
{
    public interface IBaseUIComponent
    {
        IRootElementBuilder Render(Element container);
    }
}
