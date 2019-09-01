using Assets.Engine.Render;

namespace Assets
{
    public interface IBaseUIComponent
    {
        IRootElementBuilder Render(Element container);
    }
}
